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

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebloodclotDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Order = 9;
			config.Type = StatusEffectType.Positive;
			return config;
		}
	}

	[EntityLogic(typeof(sebloodclotDef))]
	public sealed class sebloodclot : StatusEffect
	{
		public int Value
		{
			get
			{
				if (Owner == null)
				{
					return 5;
				}
				if (Level == 0)
				{
					return 5;
				}
				return Math.Min(5 * Level, 50);
			}
		}
		protected override void OnAdded(Unit unit)
		{
			HandleOwnerEvent(unit.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnDamageReceiving));
			ReactOwnerEvent(Battle.RoundEnded, OnRoundEnded);
		}

		private IEnumerable<BattleAction> OnRoundEnded(GameEventArgs args)
		{
			if (Battle.BattleShouldEnd)
			{
				yield break;
			}
			yield return new RemoveStatusEffectAction(this);
			yield break;
		}

		private void OnDamageReceiving(DamageEventArgs args)
		{
			DamageInfo damageInfo = args.DamageInfo;
			if (damageInfo.DamageType == DamageType.Attack)
			{
				damageInfo.Damage = damageInfo.Amount * (1f - Value / 100f);
				args.DamageInfo = damageInfo;
				args.AddModifier(this);
			}
		}
	}
}