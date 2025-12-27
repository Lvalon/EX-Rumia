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
using LBoL.Core.Cards;
using System.Linq;

namespace lvalonexrumia.Cards
{
	public sealed class cardslicepastDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.White };
			config.Cost = new ManaGroup() { White = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //decrease percentage

			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(seincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seincrease) };

			config.Illustrator = "みつるび";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardslicepastDef))]
	public sealed class cardslicepast : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(heal);
			List<Card> list = Battle.EnumerateAllCards().Where((Card card) => card != this && card.Zone == CardZone.Hand && card.CanUpgradeAndPositive).ToList();
			if (list.Count == 0)
			{
				yield break;
			}
			yield return new UpgradeCardsAction(list);
			yield break;
		}
	}
}


