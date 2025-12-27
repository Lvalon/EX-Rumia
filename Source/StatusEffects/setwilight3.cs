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
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class setwilight3Def : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.HasCount = true;
			config.Order = 2;
			return config;
		}
	}

	[EntityLogic(typeof(setwilight3Def))]
	public sealed class setwilight3 : StatusEffect
	{
		bool go = false;
		private readonly List<(Card, CardUsingEventArgs)> AttackEchoArgs = new List<(Card, CardUsingEventArgs)>();
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
					return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 15, true);
				}
			}
		}
		void updatecounter()
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
		}
		protected override void OnAdded(Unit unit)
		{
			go = false;
			updatecounter();
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
			HandleOwnerEvent(Battle.Player.DamageReceived, OnDamageReceived);
			HandleOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			HandleOwnerEvent(Battle.Player.HealingReceived, OnHealingReceived);
			ReactOwnerEvent(Battle.BattleEnded, OnBattleEnded, 0);
		}
		private IEnumerable<BattleAction> OnBattleEnded(GameEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
		}

		private void OnHealingReceived(HealEventArgs args)
		{
			if (go || Battle.BattleShouldEnd) { return; }
			updatecounter();
		}

		private void OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (go) { return; }
			updatecounter();
		}

		private void OnDamageReceived(DamageEventArgs args)
		{
			if (go) { return; }
			updatecounter();
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			go = Owner.Hp < lifeneed;
			if (go)
			{
				yield return new ApplyStatusEffectAction<Charging>(Battle.Player, Level, 0, 0, 0, 0.2f);
			}
			go = false;
			updatecounter();
			yield break;
		}
	}
}