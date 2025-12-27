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
	public sealed class semagiaDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sebloodsword) };
			return config;
		}
	}

	[EntityLogic(typeof(semagiaDef))]
	public sealed class semagia : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Owner.DamageReceived, OnDamageReceived);
			ReactOwnerEvent(Battle.RoundEnded, OnTurnEnding);
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			DamageInfo damageInfo = args.DamageInfo;
			if (damageInfo.DamageType == DamageType.Attack)
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<sebloodsword>(Battle.Player, Level, 0, 0, 0, 0.2f);
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnTurnEnding(GameEventArgs args)
		{
			if (Battle.BattleShouldEnd)
			{
				yield break;
			}
			yield return new RemoveStatusEffectAction(this);
			yield break;
		}
	}
}