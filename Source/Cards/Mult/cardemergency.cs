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
using LBoL.Core.Units;
using System.Linq;
using LBoL.Core.Intentions;
using LBoL.Presentation;

namespace lvalonexrumia.Cards
{
	public sealed class cardemergencyDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Block = 10;
			config.Value1 = 5; //decrease life, upgrade bloodclot
			config.Value2 = 1; //dark blood token

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sedarkblood), nameof(sebloodclot) };

			config.Illustrator = "アコア";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardemergencyDef))]
	public sealed class cardemergency : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return DefenseAction(true);
			yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			int attackCount = 0;
			EnemyUnit[] enemies = Battle.AllAliveEnemies.ToArray();
			// attackCount = enemies.Count((EnemyUnit enemy) => enemy.Intentions.Any((Intention i) => (i is AttackIntention || i is SpellCardIntention { Damage: not null }) ? true : false));
			attackCount = enemies.Count((EnemyUnit enemy) => enemy.Intentions.Any((Intention i) =>
				(i is AttackIntention) || (i is SpellCardIntention sci && sci.Damage != null)
			));
			if (attackCount > 0 && IsUpgraded)
			{
				yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}


