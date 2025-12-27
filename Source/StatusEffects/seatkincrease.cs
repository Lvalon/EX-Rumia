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

namespace lvalonexrumia.StatusEffects
{
	public sealed class seatkincreaseDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.Order = 20;
			config.HasCount = true;
			return config;
		}
	}

	[EntityLogic(typeof(seatkincreaseDef))]
	public sealed class seatkincrease : StatusEffect
	{
		int truecount = 0;
		public int truecounter
		{
			get
			{
				return truecount;
			}
		}
		public override bool ForceNotShowDownText => true;
		protected override void OnAdded(Unit unit)
		{
			if (truecount == 0)
			{
				truecount = Level;
				Count = truecount;
			}
			Level = 0;
			HandleOwnerEvent(Owner.DamageDealing, OnDamageDealing);
			HandleOwnerEvent(Owner.StatusEffectAdded, OnStatusEffectAdded);
			ReactOwnerEvent(Owner.TurnEnded, OnTurnEnded);
		}

		private void OnStatusEffectAdded(StatusEffectApplyEventArgs args)
		{
			if (args.Effect is seatkincrease)
			{
				truecount += args.Effect.Level;
				Count = truecount;
				Level = 0;
			}
		}

		private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
		{
			truecount = (int)(truecount / 10.0) * 5;
			Count = truecount;
			if (Count <= 0)
			{
				yield return new RemoveStatusEffectAction(this);
			}
			NotifyActivating();
		}

		private void OnDamageDealing(DamageDealingEventArgs args)
		{
			if (args.DamageInfo.DamageType == DamageType.Attack)
			{
				args.DamageInfo = args.DamageInfo.MultiplyBy((truecount * 1f / 100f) + 1);
				args.AddModifier(this);
				if (args.Cause != ActionCause.OnlyCalculate)
				{
					NotifyActivating();
				}
			}
		}
	}
}