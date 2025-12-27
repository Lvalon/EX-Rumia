using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Units;
using lvalonexrumia.StatusEffects;
using lvalonexrumia.Patches;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.Presentation;
using LBoL.Core.StatusEffects;

namespace lvalonexrumia.Cards
{
	public sealed class cardmutualdepletionDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Colorless };
			config.Cost = new ManaGroup() { Any = 4, Colorless = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 3, Colorless = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Nobody;

			config.Value1 = 1; //ability se num

			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease) };

			config.Illustrator = "にしこー@くるま屋さん";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardmutualdepletionDef))]
	public sealed class cardmutualdepletion : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (!Battle.Player.HasStatusEffect<sefuckyou>() && IsUpgraded)
			{
				yield return new ApplyStatusEffectAction<sefuckyou>(Battle.Player, Value1, 0, 200, 0, 0.2f);
			}
			yield return new ApplyStatusEffectAction<semutualdepletion>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


