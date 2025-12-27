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
	public sealed class cardtornDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Black = 2, Red = 2 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 10; //atk, life increase
			config.Value2 = 2; //graze
			config.UpgradedValue2 = 4;

			config.RelativeEffects = new List<string>() { nameof(Graze), nameof(seatkincrease), nameof(seincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(Graze), nameof(seatkincrease), nameof(seincrease) };

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain | Keyword.Initial;

			config.Illustrator = "cato";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtornDef))]
	public sealed class cardtorn : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<setorn>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<Graze>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield break;
		}

	}
}


