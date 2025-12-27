using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sedarkbloodDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Order = 2;
			config.Type = StatusEffectType.Positive;
			return config;
		}
	}

	[EntityLogic(typeof(sedarkbloodDef))]
	public sealed class sedarkblood : StatusEffect
	{
		bool go = false;
		bool turnending = false;
		public ManaGroup mana
		{
			get
			{
				return ManaGroup.Blacks(1);
			}
		}
		protected override void OnAdded(Unit unit)
		{
			Highlight = true;
			go = false;
			turnending = false;
			HandleOwnerEvent(Battle.Player.TurnEnding, OnTurnEnding);
			ReactOwnerEvent(Battle.Player.TurnEnded, OnTurnEnded);
			ReactOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			ReactOwnerEvent(Battle.Player.DamageReceived, OnDamageReceived);
			HandleOwnerEvent(Battle.Player.TurnStarting, OnOwnerTurnStarting);
		}

		private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
		{
			if (go)
			{
				foreach (var action in dosmth())
				{
					yield return action;
				}
				go = false;
			}
			turnending = false;
			yield break;
		}

		private void OnTurnEnding(UnitEventArgs args)
		{
			turnending = true;
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			if (!Battle.BattleShouldEnd && Highlight && args.DamageInfo.Damage > 0)
			{
				if (turnending)
				{
					go = true;
				}
				else
				{
					foreach (var action in dosmth())
					{
						yield return action;
					}
				}
			}
			yield break;
		}

		private void OnOwnerTurnStarting(UnitEventArgs args)
		{
			Highlight = true;
		}

		private IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (!Battle.BattleShouldEnd && Highlight && (args.argsunit == Battle.Player || args.argsunit == null) && args.Amount < 0)
			{
				if (turnending)
				{
					go = true;
				}
				else
				{
					foreach (var action in dosmth())
					{
						yield return action;
					}
				}
			}
			yield break;
		}
		private IEnumerable<BattleAction> dosmth()
		{
			NotifyActivating();
			if (Level <= 0)
			{
				Level = 0;
				yield break;
			}
			Level--;
			Highlight = false;
			if (Battle.HandZone.Count < Battle.MaxHand)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<carddarkblood>(1, false));
				if (Level <= 0)
				{
					Level = 0;
					yield break;
				}
			}
			yield return new GainManaAction(ManaGroup.Blacks(Level));
			Level = 0;
			yield break;
		}
	}
}