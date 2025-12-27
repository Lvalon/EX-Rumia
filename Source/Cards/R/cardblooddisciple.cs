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
using LBoL.EntityLib.Cards.Enemy;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;

namespace lvalonexrumia.Cards
{
	public sealed class cardblooddiscipleDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //heal
			config.Value2 = 2; //exile

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Echo;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot) };

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "くーげる";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardblooddiscipleDef))]
	public sealed class cardblooddisciple : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		public override Interaction Precondition()
		{
			List<Card> list2 = Battle.HandZone.Where((Card card) => card != this).ToList();
			if (!list2.Empty())
			{
				return new SelectCardInteraction(0, Value2, list2);
			}

			return null;
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			if (precondition != null)
			{
				IReadOnlyList<Card> cards = ((SelectCardInteraction)precondition).SelectedCards;
				if (cards.Count > 0)
				{
					yield return new ExileManyCardAction(cards);
					yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, cards.Count, 0, 0, 0, 0);
					yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(cards.Count, false));
				}
			}
			yield break;
		}
	}
}


