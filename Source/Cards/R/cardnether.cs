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
using System.Linq;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.Black;

namespace lvalonexrumia.Cards
{
	public sealed class cardnetherDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Shield = 10;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //blood clot
			config.UpgradedValue1 = 6;
			config.Value2 = 2; //shadows
			config.UpgradedValue2 = 1;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sebloodclot) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodclot) };

			config.RelativeCards = new List<string>() { nameof(Shadow) };
			config.UpgradedRelativeCards = new List<string>() { nameof(Shadow) };

			config.Illustrator = "招き猫っぽい犬";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardnetherDef))]
	public sealed class cardnether : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return DefenseAction(true);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new AddCardsToHandAction(Library.CreateCards<Shadow>(Value2, false));
			yield break;
		}
	}
}


