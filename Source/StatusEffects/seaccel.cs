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
	public sealed class seaccelDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(Graze) };
			return config;
		}
	}

	[EntityLogic(typeof(seaccelDef))]
	public sealed class seaccel : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Owner.TurnStarted, OnOwnerTurnStarting);
		}

		private IEnumerable<BattleAction> OnOwnerTurnStarting(UnitEventArgs args)
		{
			if (!Battle.BattleShouldEnd)
			{
				NotifyActivating();
				Unit owner = Owner;
				if (owner != null && !owner.IsDead)
				{
					if (Level <= 0)
					{
						yield return new RemoveStatusEffectAction(this);
						yield break;
					}
					yield return new ApplyStatusEffectAction<Graze>(Owner, Level, 0, 0, 0, 0.2f);
					yield return new RemoveStatusEffectAction(this);
				}
			}
		}
	}
}