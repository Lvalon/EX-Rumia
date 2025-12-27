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
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class semarkbloodDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.Order = 2;
			config.RelativeEffects = new List<string>() { nameof(sebloodmark) };
			return config;
		}
	}

	[EntityLogic(typeof(semarkbloodDef))]
	public sealed class semarkblood : StatusEffect
	{
		// public int heal
		// {
		// 	get
		// 	{
		// 		if (Owner == null)
		// 		{
		// 			return 0;
		// 		}
		// 		if (Level == 0)
		// 		{
		// 			return 0;
		// 		}
		// 		return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 5, true) * Level;
		// 	}
		// }
		// public int Value
		// {
		// 	get
		// 	{
		// 		if (Owner == null)
		// 		{
		// 			return 5;
		// 		}
		// 		if (Level == 0)
		// 		{
		// 			return 5;
		// 		}
		// 		return 5 * Level;
		// 	}
		// }
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Owner.TurnStarted, OnTurnStarted);
			ReactOwnerEvent(Owner.DamageDealt, OnDamageDealt);
		}

		private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
		{
			// if (Battle.BattleShouldEnd) { yield break; }
			// NotifyActivating();
			// foreach (Unit unit in Battle.AllAliveEnemies)
			// {
			// 	if (unit.IsAlive && !Battle.BattleShouldEnd)
			// 	{
			// 		yield return new ApplyStatusEffectAction<sebloodmark>(unit, Level);
			// 	}
			// }
			yield break;
		}

		private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
		{
			if (!Battle.BattleShouldEnd && args.Target.IsAlive)
			{
				DamageInfo damageInfo = args.DamageInfo;
				if (damageInfo.DamageType == DamageType.Attack && !args.Target.HasStatusEffect<sebloodmark>())
				{
					NotifyActivating();
					yield return new ApplyStatusEffectAction<sebloodmark>(args.Target, Level);
					// yield return new ChangeLifeAction(heal);
				}
			}
			yield break;
		}
	}
}