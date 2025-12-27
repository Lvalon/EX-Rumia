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
	public sealed class cardcorruptedbloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 2, Black = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //ability se num
			config.Value2 = 2;

			config.Keywords = Keyword.Replenish;
			config.UpgradedKeywords = Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedarkblood) };

			config.RelativeCards = new List<string>() { nameof(carddarkblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood) };

			config.Illustrator = "画框子";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardcorruptedbloodDef))]
	public sealed class cardcorruptedblood : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<secorruptedblood>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			if (IsUpgraded)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<carddarkblood>(Value2, false));
			}
			yield break;
		}
	}
}


