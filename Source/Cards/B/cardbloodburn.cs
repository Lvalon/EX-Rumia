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

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodburnDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 10; //decrease percentage

			config.Mana = new ManaGroup() { Black = 2, Red = 2 };
			config.UpgradedMana = new ManaGroup() { Philosophy = 2, Red = 2 };

			config.Keywords = Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Ethereal;

			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease) };

			config.Illustrator = "OMGitsANeevee";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodburnDef))]
	public sealed class cardbloodburn : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new GainManaAction(Mana);
			yield break;
		}
	}
}


