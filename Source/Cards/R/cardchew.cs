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
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardchewDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.All;

			config.Damage = 5;
			config.Value1 = 5; //heal
			config.Value2 = 1; //growth
			config.UpgradedValue2 = 2;

			config.GunName = GunNameID.GetGunFromId(7020);
			config.GunNameBurst = GunNameID.GetGunFromId(7021);

			config.Keywords = Keyword.Accuracy;
			config.RelativeKeyword = Keyword.Grow;
			config.UpgradedRelativeKeyword = Keyword.Grow;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease) };

			config.Illustrator = "河辺";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardchewDef))]
	public sealed class cardchew : lvalonexrumiaCard
	{
		//public override int AdditionalDamage => GrowCount * Value2;
		public int battleatk => 2 + GrowCount * Value2;
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			for (int i = 0; i < battleatk; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, GunName, GunType.Single);
			}
			yield break;
		}
	}
}


