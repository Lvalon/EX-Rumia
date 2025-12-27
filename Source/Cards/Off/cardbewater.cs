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
	public sealed class cardbewaterDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Blue };
			config.Cost = new ManaGroup() { Any = 2, Blue = 1 };
			config.UpgradedCost = new ManaGroup() { Blue = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //ability se num

			config.RelativeCards = new List<string>() { nameof(carddarkblood), nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood), nameof(cardredblood) };

			config.Illustrator = "ごま＝ソトース";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbewaterDef))]
	public sealed class cardbewater : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sebewater>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


