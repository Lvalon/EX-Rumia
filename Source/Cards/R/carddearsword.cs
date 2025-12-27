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
	public sealed class carddearswordDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Friend;
			config.TargetType = TargetType.Nobody;

			config.Value1 = 1; //decrease percentage, red blood, blood mark, bleed, blood storm
			config.Value2 = 2; //storm token, red blood 

			config.PassiveCost = 1;
			config.ActiveCost = -4;
			config.UpgradedActiveCost = -3;
			config.UltimateCost = -6;
			config.UpgradedUltimateCost = -5;
			config.Loyalty = 3;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodstorm), nameof(sebloodmark), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodstorm), nameof(sebloodmark), nameof(sebleed) };

			config.RelativeCards = new List<string>() { nameof(cardredblood), nameof(cardbloodstorm) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood), nameof(cardbloodstorm) };

			config.Illustrator = "sarise";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(carddearswordDef))]
	public sealed class carddearsword : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 5, true);
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
				yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value1, false));
				//yield return new ApplyStatusEffectAction<sebloodstorm>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			}
		}

		public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
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
					yield return new ApplyStatusEffectAction<sebloodmark>(unit, Value1, 0, 0, 0, 0.2f);
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebleed>(unit, Value1, 0, 0, 0, 0.2f);
				}
				yield return SkillAnime;
			}
			else
			{
				Loyalty += UltimateCost;
				UltimateUsed = true;
				yield return new ApplyStatusEffectAction<sedearsword>(Battle.Player, 1, 0, 0, 0, 0.2f);
				yield return SkillAnime;
			}
		}
	}
}


