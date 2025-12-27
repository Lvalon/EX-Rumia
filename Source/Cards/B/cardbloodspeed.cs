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
using System;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodspeedDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //decrease percentage
			config.Value2 = 2; //draw
			config.UpgradedValue2 = 4;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(seaccel) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(seaccel) };

			config.Illustrator = "えんご";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodspeedDef))]
	public sealed class cardbloodspeed : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		// public int healnum2
		// {
		// 	get
		// 	{
		// 		if (GameMaster.Instance.CurrentGameRun != null)
		// 		{
		// 			int lifeafter = GameMaster.Instance.CurrentGameRun.Player.Hp - heal;
		// 			int lifeafter2 = Convert.ToInt32(Math.Round((double)lifeafter * Value1 / 100, MidpointRounding.AwayFromZero));
		// 			return lifeafter2;
		// 		}
		// 		return 0;
		// 	}
		// }
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<seaccel>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield return new DrawManyCardAction(Value2);
			yield break;
		}
	}
}


