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
using System.Linq;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.Base.Extensions;

namespace lvalonexrumia.Cards
{
	public sealed class cardnightfallsDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 2, Black = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //gain per
			config.Value2 = 2; //cards search
			config.UpgradedValue2 = 3;

			config.Mana = new ManaGroup() { Philosophy = 1 };

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sebloodsword) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodsword) };

			config.RelativeCards = new List<string>() { nameof(cardbloodsword) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodsword) };

			config.Illustrator = "Spark621";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardnightfallsDef))]
	public sealed class cardnightfalls : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<senightfalls>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			if (Battle.DrawZone.Count <= 0 && Battle.HandIsFull)
			{
				yield break;
			}
			List<Card> list = Battle.DrawZoneToShow.Where((Card card) => card.Config.Colors.Contains(ManaColor.Black)).SampleManyOrAll(Value2, BattleRng).ToList();
			if (list.Count <= 0)
			{
				yield break;
			}
			foreach (Card item in list)
			{
				if (Battle.HandIsFull)
				{
					break;
				}
				yield return new MoveCardAction(item, CardZone.Hand);
				yield return new GainManaAction(Mana);
			}
			yield break;
		}
	}
}
