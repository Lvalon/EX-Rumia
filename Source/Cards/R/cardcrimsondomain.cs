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
using System.Linq;

namespace lvalonexrumia.Cards
{
	public sealed class cardcrimsondomainDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.AllEnemies;

			config.Value1 = 1; //bleed, blood mark

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Echo;

			config.RelativeEffects = new List<string>() { nameof(sebleed), nameof(sebloodmark), nameof(sedeepbleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebleed), nameof(sebloodmark), nameof(sedeepbleed) };

			config.RelativeCards = new List<string>() { nameof(cardbloodstorm) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodstorm) };

			config.Illustrator = "宇佐见二狗";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardcrimsondomainDef))]
	public sealed class cardcrimsondomain : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				if (unit.IsAlive && !Battle.BattleShouldEnd)
				{
					if (!unit.HasStatusEffect<sebleed>())
					{
						yield return new ApplyStatusEffectAction<sebleed>(unit, Value1, 0, 0, 0, 0.2f);
					}
					else
					{
						yield return new ApplyStatusEffectAction<sedeepbleed>(unit, Value1, 0, 0, 0, 0.2f);
					}
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebloodmark>(unit, Value1, 0, 0, 0, 0.2f);
				}
			}
			yield return new AddCardsToHandAction(Library.CreateCards<cardbloodstorm>(Value1, false));
			yield break;
		}
	}
}


