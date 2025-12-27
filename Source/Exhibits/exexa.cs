using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Exhibits;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Patches;
using lvalonexrumia.StatusEffects;

namespace lvalonexrumia.Exhibits
{
	public sealed class exexaDef : lvalonexrumiaExhibitTemplate
	{
		public override ExhibitConfig MakeConfig()
		{
			ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();

			exhibitConfig.Value1 = 1;
			exhibitConfig.Value2 = 5;
			exhibitConfig.Order = 4;
			exhibitConfig.HasCounter = true;
			exhibitConfig.Mana = new ManaGroup() { Black = 1 };
			exhibitConfig.BaseManaColor = ManaColor.Black;
			exhibitConfig.RelativeEffects = new List<string>() { nameof(sebloodmark) };

			return exhibitConfig;
		}
	}

	[EntityLogic(typeof(exexaDef))]
	public sealed class exexa : ShiningExhibit
	{
		void updatecounter()
		{
			Counter = Convert.ToInt32(Math.Ceiling((double)(GameRun.Player.MaxHp - GameRun.Player.Hp) * Value2 / 100));
		}
		protected override void OnAdded(PlayerUnit player)
		{
			updatecounter();
			HandleGameRunEvent(GameRun.Player.DamageReceived, OnGRDamageReceived);
			HandleGameRunEvent(GameRun.Player.HealingReceived, OnGRDamageReceived);
		}

		private void OnGRDamageReceived(HealEventArgs args)
		{
			//if (Battle.BattleShouldEnd) { return; }
			updatecounter();
		}

		private void OnGRDamageReceived(DamageEventArgs args)
		{
			updatecounter();
		}

		protected override void OnEnterBattle()
		{
			ReactBattleEvent(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnStarted));
			HandleBattleEvent(Battle.Player.DamageDealing, OnDamageDealing);
			HandleBattleEvent(Battle.Player.DamageReceived, OnDamageReceived);
			HandleBattleEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
		}

		private void OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (args.argsunit == Battle.Player || args.argsunit == null)
			{
				updatecounter();
			}
		}

		private void OnDamageReceived(DamageEventArgs args)
		{
			updatecounter();
		}

		private void OnDamageDealing(DamageDealingEventArgs args)
		{
			if (args.DamageInfo.DamageType == DamageType.Attack)
			{
				//fuck count cap
				args.DamageInfo = args.DamageInfo.IncreaseBy(Convert.ToInt32(Math.Ceiling((double)(GameRun.Player.MaxHp - GameRun.Player.Hp) * Value2 / 100)));
				args.AddModifier(this);
			}
		}

		private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
		{
			if (Battle.Player.TurnCounter == 1)
			{
				NotifyActivating();
				foreach (Unit unit in Battle.AllAliveEnemies)
				{
					yield return new ApplyStatusEffectAction<sebloodmark>(unit, Value1, 0, 0, 0, 0.2f);
				}
			}
			yield break;
		}
	}
}