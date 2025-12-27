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
	public sealed class sebloodstormDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Special;
			return config;
		}
	}

	[EntityLogic(typeof(sebloodstormDef))]
	public sealed class sebloodstorm : StatusEffect
	{
		public int lim
		{
			get
			{
				return 4;
			}
		}
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(unit.StatusEffectAdded, OnStatusEffectAdded);
		}

		private IEnumerable<BattleAction> OnStatusEffectAdded(StatusEffectApplyEventArgs args)
		{
			if (args.Effect is sebloodstorm)
			{
				if (Level >= lim)
				{
					NotifyActivating();
					yield return new AddCardsToHandAction(Library.CreateCards<cardbloodstorm>(Level / lim, false));
					Level %= lim;
					if (Level == 0)
					{
						yield return new RemoveStatusEffectAction(this, true);
					}
				}
			}
			yield break;
		}
	}
}