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
	public sealed class cardbloodofmyswordDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //ability se num
			config.Value2 = 2; //cardbloodsword num

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(seincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seincrease) };

			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodsword) };

			config.Illustrator = "画框子";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodofmyswordDef))]
	public sealed class cardbloodofmysword : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sebloodofmysword>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			if (IsUpgraded)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<cardbloodsword>(Value2, false));
				// foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
				// {
				// 	if (Battle.BattleShouldEnd) { yield break; }
				// 	yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value1, 0, 0, 0, 0.2f);
				// }
			}
			yield break;
		}
	}
}


