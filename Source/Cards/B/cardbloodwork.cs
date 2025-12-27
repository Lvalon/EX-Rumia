using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonexrumia.StatusEffects;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodworkDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 2 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Block = 10;
			config.UpgradedBlock = 13;
			config.Value1 = 3; //blood clot
			config.UpgradedValue1 = 5;

			config.RelativeEffects = new List<string>() { nameof(sebloodclot) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodclot) };

			config.Illustrator = "Pingu_Nether";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodworkDef))]
	public sealed class cardbloodwork : lvalonexrumiaCard
	{

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return DefenseAction(true);
			yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1, 0, 0, 0, 0.2f);
		}
	}
}


