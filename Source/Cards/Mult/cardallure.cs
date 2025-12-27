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
	public sealed class cardallureDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
			config.UpgradedCost = new ManaGroup() { Any = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.All;

			config.Scry = 3;

			config.Value1 = 5; //decrease percentage
			config.Value2 = 1; //draw and aoe bleed

			config.Keywords = Keyword.Scry;
			config.UpgradedKeywords = Keyword.Scry | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };

			config.Illustrator = "ファルケン@skebお仕募集中";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardallureDef))]
	public sealed class cardallure : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new ScryAction(Scry);
			yield return new DrawManyCardAction(Value2);
			foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new ApplyStatusEffectAction<sebleed>(enemy, Value2, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}


