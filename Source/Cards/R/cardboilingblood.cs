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
	public sealed class cardboilingbloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.All;

			config.GunName = GunNameID.GetGunFromId(23051);
			config.GunNameBurst = GunNameID.GetGunFromId(23052);

			config.Damage = 5;
			config.Value1 = 5; //heal
			config.Value2 = 30; //atk increase
			config.UpgradedValue2 = 40;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed), nameof(seatkincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed), nameof(seatkincrease) };

			config.Illustrator = "Yuki Nanami";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardboilingbloodDef))]
	public sealed class cardboilingblood : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			if (!IsUpgraded)
			{
				if (Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>()).Count() > 0)
				{
					yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>()).ToArray(), Damage, GunName, GunType.Single);
				}
			}
			else
			{
				yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, GunName, GunType.Single);
			}
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


