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

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodyhellDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //atk increase
			config.UpgradedKeywords = Keyword.Retain | Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(seatkincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seatkincrease) };

			config.Illustrator = "アイル";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodyhellDef))]
	public sealed class cardbloodyhell : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sebloodyhell>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


