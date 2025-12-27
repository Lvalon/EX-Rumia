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

namespace lvalonexrumia.Cards
{
	public sealed class cardsunnydryDef : lvalonexrumiaCardTemplate
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

			config.Value1 = 5; //heal
			config.Value2 = 4; //blood clot
			config.UpgradedValue2 = 6;

			config.UpgradedKeywords = Keyword.Replenish;

			config.Mana = new ManaGroup() { Red = 3, Philosophy = 1 };
			config.UpgradedMana = new ManaGroup() { Red = 2, Philosophy = 2 };

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot) };

			config.RelativeCards = new List<string>() { nameof(Riguang) };
			config.UpgradedRelativeCards = new List<string>() { nameof(Riguang) };

			config.Illustrator = "六合ダイスケ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardsunnydryDef))]
	public sealed class cardsunnydry : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new GainManaAction(Mana);
			yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value2, 0, 0, 0, 0);
			yield return new AddCardsToHandAction(Library.CreateCards<Riguang>(1, false));
			yield break;
		}
	}
}


