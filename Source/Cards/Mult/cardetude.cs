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
	public sealed class cardetudeDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Black = 1, Red = 1 };
			config.UpgradedCost = new ManaGroup() { Hybrid = 1, HybridColor = 7 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.RandomEnemy;

			config.Damage = 4;

			config.GunName = GunNameID.GetGunFromId(7041);
			config.GunNameBurst = GunNameID.GetGunFromId(7041);

			config.Value1 = 5; //decrease percentage
			config.Value2 = 30; //atk increase

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(seatkincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(seatkincrease) };

			config.Illustrator = "awa yume";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardetudeDef))]
	public sealed class cardetude : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			for (int i = 0; i < 3; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new DamageAction(Battle.Player, Battle.RandomAliveEnemy, Damage, GunName, GunType.Single);
			}
			yield return new ApplyStatusEffectAction<seetude>(Battle.Player, 4, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


