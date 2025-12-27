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

namespace lvalonexrumia.Cards
{
	public sealed class cardmissribbonDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Friend;
			config.TargetType = TargetType.Nobody;

			config.Value1 = 5; //decrease percentage
			config.Value2 = 1; //blood mark
			config.UpgradedValue2 = 2;

			config.PassiveCost = 1;
			config.ActiveCost = -3;
			config.UltimateCost = -5;
			config.Loyalty = 3;
			config.UpgradedLoyalty = 5;

			config.Mana = new ManaGroup() { Black = 1 };
			config.UpgradedMana = new ManaGroup() { Philosophy = 1 };

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark), nameof(sebloodsword) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark), nameof(sebloodsword) };

			config.RelativeCards = new List<string>() { nameof(cardbloodsword) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodsword) };

			config.Illustrator = "わんどろいど";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardmissribbonDef))]
	public sealed class cardmissribbon : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		// protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		// {
		// 	yield return new ChangeLifeAction(-heal);
		// 	yield break;
		// }
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
				yield return new ChangeLifeAction(-heal);
				yield return new GainManaAction(Mana);
			}
		}

		public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new GainManaAction(Mana);
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
				foreach (Unit unit in Battle.AllAliveEnemies)
				{
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebloodmark>(unit, Value2, 0, 0, 0, 0.2f);
				}
				yield return SkillAnime;
			}
			else
			{
				Loyalty += UltimateCost;
				UltimateUsed = true;
				yield return new ApplyStatusEffectAction<semissribbon>(Battle.Player, 1, 0, 0, 0, 0.2f);
				yield return SkillAnime;
			}
		}
	}
}


