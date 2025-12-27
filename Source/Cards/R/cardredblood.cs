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
	public sealed class cardredbloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 4;
			config.UpgradedDamage = 8;
			config.Value1 = 5;

			config.GunName = GunNameID.GetGunFromId(23010);
			config.GunNameBurst = GunNameID.GetGunFromId(23011);

			config.Mana = new ManaGroup() { Red = 1 };
			config.UpgradedMana = new ManaGroup() { Philosophy = 1 };

			config.Keywords = Keyword.Exile | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(seincrease), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seincrease), nameof(sebleed) };

			config.Illustrator = "とたけけ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardredbloodDef))]
	public sealed class cardredblood : lvalonexrumiaCard
	{
		public override bool Triggered => Battle != null && !IsUpgraded && GameMaster.Instance.CurrentGameRun.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (TriggeredAnyhow || IsUpgraded)
			{
				yield return new ChangeLifeAction(heal);
			}
			yield return new GainManaAction(Mana);
			if (Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>()).Count() == 0)
			{
				yield break;
			}
			DamageInfo damage2 = Damage;
			damage2.IsAccuracy = true;
			yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>()).ToArray(), damage2, GunName, GunType.Single);
			yield break;
		}
	}
}


