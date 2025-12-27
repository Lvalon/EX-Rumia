using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class setwilight1Def : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.HasCount = true;
			config.Order = 4;
			return config;
		}
	}

	[EntityLogic(typeof(setwilight1Def))]
	public sealed class setwilight1 : StatusEffect
	{
		int truecount = 0;
		public int truecounter
		{
			get
			{
				return truecount;
			}
		}
		public int lifeneed
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun == null)
				{
					return 0;
				}
				else
				{
					return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
				}
			}
		}
		void updatecounter()
		{
			if (Owner.Hp < lifeneed)
			{
				truecount = Convert.ToInt32(Math.Ceiling((double)(GameRun.Player.MaxHp - GameRun.Player.Hp) * Level / 100));
				Count = truecount;
				Highlight = true;
			}
			else
			{
				truecount = 0;
				Count = 0;
				Highlight = false;
			}
		}
		protected override void OnAdded(Unit unit)
		{
			updatecounter();
			ReactOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, new EventSequencedReactor<ChangeLifeEventArgs>(OnLifeChanged));
			foreach (Unit forunit in Battle.AllAliveUnits)
			{
				ReactOwnerEvent(forunit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
			}
			HandleOwnerEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
			HandleOwnerEvent(Battle.Player.DamageDealing, OnDamageDealing);
			HandleOwnerEvent(Battle.Player.HealingReceived, OnHealingReceived);
			ReactOwnerEvent(Battle.BattleEnded, OnBattleEnded, 0);
		}
		private IEnumerable<BattleAction> OnBattleEnded(GameEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
		}

		private void OnHealingReceived(HealEventArgs args)
		{
			if (Battle.BattleShouldEnd) { return; }
			updatecounter();
		}

		private void OnDamageDealing(DamageDealingEventArgs args)
		{
			if (args.DamageInfo.DamageType == DamageType.Attack && Count > 0)
			{
				//fuck count cap
				args.DamageInfo = args.DamageInfo.IncreaseBy(Convert.ToInt32(Math.Ceiling((double)(GameRun.Player.MaxHp - GameRun.Player.Hp) * Level / 100)));
				args.AddModifier(this);
			}
		}

		private void OnEnemySpawned(UnitEventArgs args)
		{
			ReactOwnerEvent(args.Unit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			updatecounter();
			yield break;
		}

		private IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			updatecounter();
			yield break;
		}
	}
}