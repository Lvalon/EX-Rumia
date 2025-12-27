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
using System;

namespace lvalonexrumia.Cards
{
	public sealed class cardgojoDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Black = 2, Red = 2, Hybrid = 1, HybridColor = 7 };
			config.UpgradedCost = new ManaGroup() { Black = 1, Hybrid = 2, HybridColor = 7 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.All;

			config.Value1 = 5; //decrease percentage
			config.Value2 = 1; //extra turn

			config.Block = 0;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(ExtraTurn) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(ExtraTurn) };

			config.Illustrator = "カキイカダ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardgojoDef))]
	public sealed class cardgojo : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		// public int healnum2
		// {
		// 	get
		// 	{
		// 		if (GameMaster.Instance.CurrentGameRun != null)
		// 		{
		// 			int lifeafter = GameMaster.Instance.CurrentGameRun.Player.Hp - heal;
		// 			int lifeafter2 = Convert.ToInt32(Math.Round((double)lifeafter * Value1 / 100, MidpointRounding.AwayFromZero));
		// 			return lifeafter2;
		// 		}
		// 		return 0;
		// 	}
		// }
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return PerformAction.Spell(Battle.Player, "exgojo");
			yield return new ChangeLifeAction(-heal);
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<segojo>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield return PerformAction.Effect(Battle.Player, "ExtraTime");
			yield return new ApplyStatusEffectAction<ExtraTurn>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


