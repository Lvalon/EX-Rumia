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
	public sealed class cardworldburnDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Black = 2, Red = 2 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.All;

			config.Value1 = 1; //bleed and blood mark

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sebloodmark), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodmark), nameof(sebleed) };

			config.Illustrator = "shinekalta";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardworldburnDef))]
	public sealed class cardworldburn : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<seworldburn>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			if (IsUpgraded)
			{
				foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
				{
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value1, 0, 0, 0, 0.2f);
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebleed>(enemy, Value1, 0, 0, 0, 0.2f);
				}
			}
			yield break;
		}
	}
}


