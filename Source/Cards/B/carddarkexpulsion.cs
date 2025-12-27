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
using System.Linq;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.Cards.Neutral.Black;

namespace lvalonexrumia.Cards
{
	public sealed class carddarkexpulsionDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //graze
			config.Value2 = 4; //dark blood token
			config.UpgradedValue2 = 5;

			config.Keywords = Keyword.Replenish | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Replenish | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedarkblood) };

			config.RelativeCards = new List<string>() { nameof(carddarkblood), nameof(Shadow) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood), nameof(Shadow) };

			config.Illustrator = "arutana";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(carddarkexpulsionDef))]
	public sealed class carddarkexpulsion : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield return new AddCardsToHandAction(Library.CreateCards<Shadow>(Value1, false));
			yield break;
		}
	}
}