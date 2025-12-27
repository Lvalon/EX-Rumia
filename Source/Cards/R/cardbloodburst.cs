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
using System.Linq;
using LBoL.Core.Cards;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodburstDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.All;

			config.GunName = GunNameID.GetGunFromId(4532);
			config.GunNameBurst = GunNameID.GetGunFromId(4532);

			config.Damage = 5;
			config.Value1 = 5; //decrease life
			config.Value2 = 2; //bleed
			config.UpgradedValue2 = 3;

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };

			config.Illustrator = "さとくら";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodburstDef))]
	public sealed class cardbloodburst : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				for (int i = 0; i < Value2; i++)
				{
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new ApplyStatusEffectAction<sebleed>(unit, 1, 0, 0, 0, 0.2f);
				}
			}
			for (int i = 0; i < 3; i++)
			{
				if (Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>()).Count() > 0)
				{
					if (Battle.BattleShouldEnd) { yield break; }
					yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>()).ToArray(), Damage, GunName, GunType.Single);
				}
			}
			yield break;
		}
	}
}


