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
	public sealed class cardbloodcontractDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //ability se num

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodsword) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodsword) };

			config.RelativeCards = new List<string>() { nameof(cardbloodsword) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodsword) };

			config.Illustrator = "ちゅかさ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodcontractDef))]
	public sealed class cardbloodcontract : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sebloodcontract>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


