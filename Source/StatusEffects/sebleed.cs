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
using lvalonexrumia.Patches;
using lvalonmeme.StatusEffects;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebleedDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Negative;
			config.HasCount = true;
			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			return config;
		}
	}

	[EntityLogic(typeof(sebleedDef))]
	public sealed class sebleed : StatusEffect
	{
		public int truecount
		{
			get
			{
				if (Owner != null)
				{
					return (int)Math.Ceiling(Owner.MaxHp * 0.01f);
				}
				else
				{
					return 0;
				}
			}
		}
		public int lifenow
		{
			get
			{
				if (Owner != null)
				{
					return Owner.Hp;
				}
				else
				{
					return 0;
				}
			}
		}
		protected override void OnAdded(Unit unit)
		{
			Count = (int)Math.Ceiling(unit.MaxHp * 0.01f);
			ReactOwnerEvent(unit.DamageReceived, OnDamageReceived);
			ReactOwnerEvent(unit.TurnEnding, OnTurnEnding);
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			DamageInfo damageInfo = args.DamageInfo;
			if (damageInfo.DamageType == DamageType.Attack)
			{
				NotifyActivating();
				if (args.Target.IsNotAlive) { yield break; }
				yield return new ChangeLifeAction(-(int)Math.Ceiling(args.Target.MaxHp * 0.01f), args.Target);
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
		{
			if (Battle.BattleShouldEnd)
			{
				yield break;
			}
			NotifyActivating();
			if (Level > 1)
			{
				Level--;
			}
			else
			{
				yield return new RemoveStatusEffectAction(this);
			}
			//Highlight = false;
			yield break;
		}
	}
}