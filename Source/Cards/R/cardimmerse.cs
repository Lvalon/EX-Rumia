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

namespace lvalonexrumia.Cards
{
	public sealed class cardimmerseDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 2, Red = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //decrease life
			config.Value2 = 2; //storrm tokrn, red blood

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodstorm) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodstorm) };

			config.RelativeCards = new List<string>() { nameof(cardbloodstorm), nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodstorm), nameof(cardredblood) };

			config.Illustrator = "かっさんどら";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardimmerseDef))]
	public sealed class cardimmerse : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<seimmerse>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value2, false));
			yield break;
		}
	}
}


