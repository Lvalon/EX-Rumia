using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.Base.Extensions;
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
	public sealed class segojoDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Order = int.MaxValue;
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(ExtraTurn) };
			return config;
		}
	}

	[EntityLogic(typeof(segojoDef))]
	public sealed class segojo : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
		protected override void OnAdded(Unit unit)
		{
			HandleOwnerEvent(unit.DamageTaking, OnDamageTaking);
			ReactOwnerEvent(Battle.RoundEnded, OnTurnEnded);
		}

		private IEnumerable<BattleAction> OnTurnEnded(GameEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
			yield break;
		}

		private void OnDamageTaking(DamageEventArgs args)
		{
			int num = args.DamageInfo.Damage.RoundToInt();
			if (num > 0)
			{
				NotifyActivating();
				args.DamageInfo = args.DamageInfo.ReduceActualDamageBy(num);
				args.AddModifier(this);
			}
		}
	}
}