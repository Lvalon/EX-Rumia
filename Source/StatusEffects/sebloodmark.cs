using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebloodmarkDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Order = 9;
			config.Type = StatusEffectType.Negative;
			return config;
		}
	}

	[EntityLogic(typeof(sebloodmarkDef))]
	public sealed class sebloodmark : StatusEffect
	{
		public int Value
		{
			get
			{
				if (Owner == null)
				{
					return 20;
				}
				if (Level == 0)
				{
					return 20;
				}
				return 20 * Level;
			}
		}
		protected override void OnAdded(Unit unit)
		{
			HandleOwnerEvent(unit.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnDamageReceiving));
			ReactOwnerEvent(unit.TurnStarting, OnTurnEnding);
		}

		private void OnDamageReceiving(DamageEventArgs args)
		{
			DamageInfo damageInfo = args.DamageInfo;
			if (damageInfo.DamageType == DamageType.Attack)
			{
				damageInfo.Damage = damageInfo.Amount * (1f + Value / 100f);
				args.DamageInfo = damageInfo;
				args.AddModifier(this);
			}
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