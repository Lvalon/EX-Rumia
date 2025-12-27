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
	public sealed class secorruptedbloodDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Order = 1;
			config.Type = StatusEffectType.Positive;
			return config;
		}
	}

	[EntityLogic(typeof(secorruptedbloodDef))]
	public sealed class secorruptedblood : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			//Highlight = true;
			ReactOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			ReactOwnerEvent(Battle.Player.DamageReceived, OnDamageReceived);
			//HandleOwnerEvent(Battle.Player.TurnStarting, OnOwnerTurnStarting);
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			if (!Battle.BattleShouldEnd && args.DamageInfo.Damage > 0) // && Highlight
			{
				foreach (var action in dosmth())
				{
					yield return action;
				}
			}
			yield break;
		}

		// private void OnOwnerTurnStarting(UnitEventArgs args)
		// {
		// 	Highlight = true;
		// }

		private IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (!Battle.BattleShouldEnd && (args.argsunit == Battle.Player || args.argsunit == null) && args.Amount < 0) // && Highlight
			{
				foreach (var action in dosmth())
				{
					yield return action;
				}
			}
			yield break;
		}
		private IEnumerable<BattleAction> dosmth()
		{
			NotifyActivating();
			// if (!Highlight)
			// {
			// 	yield break;
			// }
			// Highlight = false;
			yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Level, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}