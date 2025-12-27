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
	public sealed class cardbloodcircleDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 2, Black = 1, Red = 1, Hybrid = 1, HybridColor = 7 };
			config.UpgradedCost = new ManaGroup() { Any = 1, Black = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //gain on upgrade
			config.Value2 = 1; //ability se num

			config.RelativeCards = new List<string>() { nameof(carddarkblood), nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood), nameof(cardredblood) };

			config.Illustrator = "retishia";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodcircleDef))]
	public sealed class cardbloodcircle : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sebloodcircle>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			// if (IsUpgraded)
			// {
			// 	yield return new ApplyStatusEffectAction<sebloodcircle2>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			// }
			yield break;
		}

	}
}


