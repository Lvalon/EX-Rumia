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
	public sealed class cardquickrestoreDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red, ManaColor.Green };
			config.Cost = new ManaGroup() { Red = 1, Green = 1 };
			config.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 9 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //ability se num

			config.Illustrator = "蒲谷カバヂ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardquickrestoreDef))]
	public sealed class cardquickrestore : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (Battle.Player.HasStatusEffect<sequickrestore>()) { yield break; }
			yield return new ApplyStatusEffectAction<sequickrestore>(Battle.Player, Battle.Player.Hp, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


