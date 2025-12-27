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
	public sealed class cardbloodmanipDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.All;

			config.Value1 = 8; //blood clot
			config.UpgradedValue1 = 10;
			config.Value2 = 1; //blood mark, bleed
			config.UpgradedValue2 = 2;

			config.Block = 0;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot), nameof(sebloodmark), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodclot), nameof(sebloodmark), nameof(sebleed) };

			config.Illustrator = "Anderson_M3011";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodmanipDef))]
	public sealed class cardbloodmanip : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 5, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value2, 0, 0, 0, 0.2f);
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new ApplyStatusEffectAction<sebleed>(enemy, Value2, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}


