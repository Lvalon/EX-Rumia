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
using System.Linq;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodpulseDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 3 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 5;
			config.Value1 = 5; //heal
			config.Value2 = 2; //atk times
			config.UpgradedValue2 = 3;

			config.GunName = GunNameID.GetGunFromId(4532);
			config.GunNameBurst = GunNameID.GetGunFromId(4532);

			config.Keywords = Keyword.Debut | Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Debut;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "黒夢";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodpulseDef))]
	public sealed class cardbloodpulse : lvalonexrumiaCard
	{
		public override Interaction Precondition()
		{
			if (DebutActive)
			{
				List<Card> list2 = base.Battle.HandZone.Where((Card card) => card != this).ToList();
				if (!list2.Empty())
				{
					return new SelectCardInteraction(0, Battle.MaxHand, list2);
				}
			}
			return null;
		}
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override string GetBaseDescription()
		{
			if (!base.DebutActive)
			{
				return base.GetExtraDescription1;
			}

			return base.GetBaseDescription();
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new ApplyStatusEffectAction<sebleed>(unit, 1, 0, 0, 0, 0.2f);
			}
			for (int i = 0; i < Value2; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return AttackAction(selector, GunName);
			}
			if (Battle.BattleShouldEnd) { yield break; }
			if (precondition != null && DebutActive)
			{
				IReadOnlyList<Card> cards = ((SelectCardInteraction)precondition).SelectedCards;
				if (cards.Count > 0)
				{
					yield return new ExileManyCardAction(cards);
					yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(cards.Count));
				}
			}
			yield break;
		}
	}
}


