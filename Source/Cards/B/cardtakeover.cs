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
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardtakeoverDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.GunName = GunNameID.GetGunFromId(25010);
			config.GunNameBurst = GunNameID.GetGunFromId(25011);

			config.Damage = 5;
			config.Value1 = 5; //decrease life
			config.Value2 = 2; //fp down, atk times, decrease times

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark), nameof(sebleed), nameof(Weak), nameof(Vulnerable) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark), nameof(sebleed), nameof(Weak), nameof(FirepowerNegative), nameof(Vulnerable) };

			config.Illustrator = "pcs shousa";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtakeoverDef))]
	public sealed class cardtakeover : lvalonexrumiaCard
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
			yield return new ApplyStatusEffectAction<sebloodmark>(selector.SelectedEnemy, 1, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<sebleed>(selector.SelectedEnemy, 1, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<Weak>(selector.SelectedEnemy, 1, 1, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<Vulnerable>(selector.SelectedEnemy, 1, 1, 0, 0, 0.2f);
			if (IsUpgraded)
			{
				yield return new ApplyStatusEffectAction<FirepowerNegative>(selector.SelectedEnemy, Value2, 0, 0, 0, 0.2f);
			}
			for (int i = 0; i < Value2; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return AttackAction(selector, GunName);
			}
			yield break;
		}
	}
}


