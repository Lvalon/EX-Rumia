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
	public sealed class cardmarkbloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.All;

			config.Value1 = 5; //heal
			config.Value2 = 1; //ability se num, blood mark

			config.RelativeEffects = new List<string>() { nameof(sebloodmark) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodmark) };

			config.Illustrator = "Nekominase";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardmarkbloodDef))]
	public sealed class cardmarkblood : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<semarkblood>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			if (IsUpgraded)
			{
				foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
				{
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value2, 0, 0, 0, 0.2f);
				}
			}
			yield break;
		}
	}
}


