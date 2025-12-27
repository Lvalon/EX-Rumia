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
using System;

namespace lvalonexrumia.Cards
{
	public sealed class cardinfuseddarknessDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.UpgradedCost = new ManaGroup() { Black = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //add ability
			config.Value2 = 1; //add dark blood

			config.Mana = new ManaGroup() { Philosophy = 1 };

			config.RelativeCards = new List<string>() { nameof(carddarkblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood) };

			config.Illustrator = "shukusuri";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardinfuseddarknessDef))]
	public sealed class cardinfuseddarkness : lvalonexrumiaCard
	{
		// protected override void OnEnterBattle(BattleController battle)
		// {
		// 	ReactBattleEvent(Battle.Player.TurnEnded, OnTurnEnded);
		// }

		// private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
		// {
		// 	if (Zone == CardZone.Exile)
		// 	{
		// 		yield return new MoveCardAction(this, CardZone.Hand);
		// 	}
		// }

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<seinfuseddarkness>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield return new AddCardsToHandAction(Library.CreateCards<carddarkblood>(Value2, false));
			yield break;
		}
	}
}


