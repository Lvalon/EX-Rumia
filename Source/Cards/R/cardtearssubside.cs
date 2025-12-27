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
	public sealed class cardtearssubsideDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 2 };
			config.UpgradedCost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 3; //draw to clot
			config.Value2 = 3; //draw
			config.UpgradedValue2 = 5;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot) };

			config.Illustrator = "kaeruhitode";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtearssubsideDef))]
	public sealed class cardtearssubside : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 5, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			DrawManyCardAction drawAction = new DrawManyCardAction(Value2);
			yield return drawAction;
			IReadOnlyList<Card> drawnCards = drawAction.DrawnCards;
			int num = drawnCards.Count((Card card) => card.Config.Colors.Contains(ManaColor.Red));
			if (num > 0)
			{
				yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1 * num, 0, 0, 0, 0);
			}
			yield break;
		}
	}
}


