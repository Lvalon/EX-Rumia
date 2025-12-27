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
using LBoL.Core.Cards;

namespace lvalonexrumia.Cards
{
	public sealed class cardshadyDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
			config.UpgradedCost = new ManaGroup() { Any = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //graze
			config.Value2 = 1; //dark blood token

			config.UpgradedKeywords = Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(Graze), nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(Graze), nameof(sedarkblood) };

			config.RelativeCards = new List<string>() { nameof(carddarkblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood) };

			config.Illustrator = "くーげる";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardshadyDef))]
	public sealed class cardshady : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<Graze>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield break;
		}

	}
}


