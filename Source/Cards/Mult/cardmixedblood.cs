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

namespace lvalonexrumia.Cards
{
	public sealed class cardmixedbloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Black = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 1;

			config.RelativeEffects = new List<string>() { nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedarkblood) };

			config.RelativeCards = new List<string>() { nameof(carddarkblood), nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood), nameof(cardredblood) };

			config.Illustrator = "万休(万事休す)";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardmixedbloodDef))]
	public sealed class cardmixedblood : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<semixedblood>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			if (IsUpgraded)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<carddarkblood>(Value1, false));
				yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value1, false));
			}
			yield break;
		}

	}
}


