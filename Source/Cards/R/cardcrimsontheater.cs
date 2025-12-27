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
	public sealed class cardcrimsontheaterDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 2 };
			config.UpgradedCost = new ManaGroup() { Red = 2 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 2; //graze
			config.Value2 = 1; //blood storm

			config.RelativeEffects = new List<string>() { nameof(Graze) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(Graze) };

			config.RelativeCards = new List<string>() { nameof(cardbloodstorm) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodstorm) };

			config.Illustrator = "kagura_mizuki";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardcrimsontheaterDef))]
	public sealed class cardcrimsontheater : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<Graze>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield return new AddCardsToHandAction(Library.CreateCards<cardbloodstorm>(Value2, false));
			yield break;
		}
	}
}


