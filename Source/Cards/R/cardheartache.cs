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

namespace lvalonexrumia.Cards
{
	public sealed class cardheartacheDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Block = 22;
			config.UpgradedBlock = 28;
			config.Value1 = 5; //heal

			config.UpgradedKeywords = Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease) };

			config.Illustrator = "you";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardheartacheDef))]
	public sealed class cardheartache : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return DefenseAction(true);
			yield break;
		}
	}
}


