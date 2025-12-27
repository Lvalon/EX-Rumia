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
	public sealed class cardtwilightDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red | ManaColor.Colorless };
			config.Cost = new ManaGroup() { Any = 3, Red = 1, Colorless = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 1, Red = 1, Colorless = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //ability se num

			config.Keywords = Keyword.Initial | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Initial | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(Charging) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(Charging) };

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "Spark621";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtwilightDef))]
	public sealed class cardtwilight : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<setwilight1>(Battle.Player, 10, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<setwilight2>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<setwilight3>(Battle.Player, 2, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


