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
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.Core.Cards;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardtenshadedDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 3, Black = 2 };
			config.UpgradedCost = new ManaGroup() { Any = 2, Black = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.RandomEnemy;

			config.GunName = GunNameID.GetGunFromId(520);
			config.GunNameBurst = GunNameID.GetGunFromId(520);

			config.Damage = 1;

			config.Value1 = 5; //decrease percentage
			config.Value2 = 10; //random attack times

			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark) };

			config.RelativeCards = new List<string>() { nameof(Shadow) };
			config.UpgradedRelativeCards = new List<string>() { nameof(Shadow) };

			config.Illustrator = "それ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtenshadedDef))]
	public sealed class cardtenshaded : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override int BaseValue3 => 3;
		protected override int BaseUpgradedValue3 => 3;
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return PerformAction.Spell(Battle.Player, "extenshaded");
			yield return new ChangeLifeAction(-heal);
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd || !IsUpgraded) { continue; }
				yield return new ApplyStatusEffectAction<sebloodmark>(unit, 1, 0, 0, 0, 0.2f);
			}
			for (int i = 0; i < Value2; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new DamageAction(Battle.Player, Battle.RandomAliveEnemy, Damage, GunName, GunType.Single);
			}
			yield return new AddCardsToHandAction(Library.CreateCards<Shadow>(Value3, false));
			yield break;
		}
	}
}


