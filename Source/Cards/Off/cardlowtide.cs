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
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;

namespace lvalonexrumia.Cards
{
	public sealed class cardlowtideDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Blue };
			config.Cost = new ManaGroup() { Any = 2, Black = 1, Blue = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 2, HybridColor = 4 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //ability se num
			config.Value2 = 1;

			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease) };

			config.Illustrator = "あらき　ﾟ ∀ ﾟ)。彡゜";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardlowtideDef))]
	public sealed class cardlowtide : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.DrawZone.Count > 0)
			{
				SelectCardInteraction interaction = new SelectCardInteraction(0, Value2, Battle.DrawZoneToShow)
				{
					Source = this
				};
				yield return new InteractionAction(interaction);
				IReadOnlyList<Card> cards = interaction.SelectedCards;
				if (cards.Count > 0)
				{
					foreach (Card card in cards)
					{
						//yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
						//card.Zone = CardZone.Draw;
						yield return new MoveCardAction(card, CardZone.Hand);
					}
				}
			}
			if (Battle.DiscardZone.Count > 0)
			{
				SelectCardInteraction interaction = new SelectCardInteraction(0, Value2, Battle.DiscardZone)
				{
					Source = this
				};
				yield return new InteractionAction(interaction);
				IReadOnlyList<Card> cards = interaction.SelectedCards;
				if (cards.Count > 0)
				{
					foreach (Card card in cards)
					{
						//yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
						//card.Zone = CardZone.Draw;
						yield return new MoveCardAction(card, CardZone.Hand);
					}
				}
			}
			if (Battle.ExileZone.Count > 0)
			{
				SelectCardInteraction interaction = new SelectCardInteraction(0, Value2, Battle.ExileZone)
				{
					Source = this
				};
				yield return new InteractionAction(interaction);
				IReadOnlyList<Card> cards = interaction.SelectedCards;
				if (cards.Count > 0)
				{
					foreach (Card card in cards)
					{
						//yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
						//card.Zone = CardZone.Draw;
						yield return new MoveCardAction(card, CardZone.Hand);
					}
				}
			}
			yield break;
		}
	}
}


