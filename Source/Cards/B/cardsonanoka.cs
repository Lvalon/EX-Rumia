using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonexrumia.StatusEffects;
using lvalonmeme.StatusEffects;
using LBoL.Core.StatusEffects;
using lvalonexrumia.Patches;
using LBoL.Presentation;

namespace lvalonexrumia.Cards
{
	public sealed class cardsonanokaDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Block = 5;
			config.Value1 = 5; //decrease life
			config.Value2 = 1; //graze

			config.Keywords = Keyword.Replenish;
			config.UpgradedKeywords = Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(Graze) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(Graze) };

			config.Illustrator = "b_Sanaeosi0730";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardsonanokaDef))]
	public sealed class cardsonanoka : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return DefenseAction(true);
			yield return new ApplyStatusEffectAction<Graze>(Battle.Player, Value2, 0, 0, 0, 0.2f);
		}
	}
}


