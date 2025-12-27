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
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using System.Linq;
using System;

namespace lvalonexrumia.Cards
{
	public sealed class cardhakureimikoDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, White = 1, Red = 1, Black = 1, Hybrid = 1, HybridColor = 7 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Friend;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //decrease percentage

			config.PassiveCost = 1;
			config.ActiveCost = -2;
			config.UltimateCost = -9;
			config.Loyalty = 5;
			config.UpgradedLoyalty = 7;

			config.RelativeEffects = new List<string>() { nameof(seincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seincrease) };

			config.Illustrator = "草津太";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardhakureimikoDef))]
	public sealed class cardhakureimiko : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.CardsAddedToDiscard, OnAddCard);
			ReactBattleEvent(Battle.CardsAddedToHand, OnAddCard);
			ReactBattleEvent(Battle.CardsAddedToExile, OnAddCard);
			ReactBattleEvent(Battle.CardsAddedToDrawZone, OnCardsAddedToDrawZone);
		}

		private IEnumerable<BattleAction> OnCardsAddedToDrawZone(CardsAddingToDrawZoneEventArgs args)
		{
			if (Zone == CardZone.Hand)
			{
				yield return Upgrade(args.Cards);
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnAddCard(CardsEventArgs args)
		{
			if (Zone == CardZone.Hand)
			{
				yield return Upgrade(args.Cards);
			}
			yield break;
		}
		private BattleAction Upgrade(IEnumerable<Card> cards)
		{
			List<Card> list = cards.Where((Card card) => card.CanUpgradeAndPositive).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			NotifyActivating();
			return new UpgradeCardsAction(list);
		}

		public override IEnumerable<BattleAction> OnTurnStartedInHand()
		{
			return GetPassiveActions();
		}

		public override IEnumerable<BattleAction> GetPassiveActions()
		{
			if (!Summoned || Battle.BattleShouldEnd)
			{
				yield break;
			}

			NotifyActivating();
			Loyalty += PassiveCost;
			for (int i = 0; i < Battle.FriendPassiveTimes; i++)
			{
				if (Battle.BattleShouldEnd)
				{
					break;
				}
				yield return PerformAction.Sfx("FairySupport");
				//yield return PerformAction.Effect(Battle.Player, "LilyFairy");
				yield return new ChangeLifeAction(heal);
			}
		}

		public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			foreach (BattleAction item in base.SummonActions(selector, consumingMana, precondition))
			{
				yield return item;
			}
		}

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
			{
				Loyalty += ActiveCost;
				yield return SkillAnime;
				if (Battle.ExileZone.Count > 0)
				{
					SelectCardInteraction interaction = new SelectCardInteraction(1, 1, Battle.ExileZone)
					{
						Source = this
					};
					yield return new InteractionAction(interaction);
					Card card = interaction.SelectedCards.FirstOrDefault();
					if (card != null)
					{
						if (card.CanUpgradeAndPositive)
						{
							yield return new UpgradeCardAction(card);
						}
						yield return new MoveCardAction(card, CardZone.Hand);
					}
				}
				yield return new MoveCardAction(this, CardZone.Hand);
			}
			else
			{
				Loyalty += UltimateCost;
				UltimateUsed = true;
				if (!Battle.BattleShouldEnd)
				{
					List<Card> list = (from c in Battle.EnumerateAllCards()
									   where c.IsExile && c != this
									   select c).ToList();
					if (list.Count > 0)
					{
						foreach (Card card2 in list)
						{
							card2.IsExile = false;
						}
					}
					List<Card> list2 = (from c in Battle.EnumerateAllCards()
										where c.IsEthereal && c != this
										select c).ToList();
					if (list2.Count > 0)
					{
						foreach (Card card3 in list2)
						{
							card3.IsEthereal = false;
						}
					}
				}
			}
		}
	}
}


