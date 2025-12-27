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
	public sealed class cardunstablebloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 2, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 14;
			config.UpgradedDamage = 18;
			config.Value1 = 5; //decrease life
			config.Value2 = 2;
			config.UpgradedValue2 = 3; //storm token

			config.GunName = GunNameID.GetGunFromId(23051);
			config.GunNameBurst = GunNameID.GetGunFromId(23052);

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed), nameof(sebloodstorm) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed), nameof(sebloodstorm) };

			config.RelativeCards = new List<string>() { nameof(cardbloodstorm) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodstorm) };

			config.Illustrator = "Chama@skeb募集中";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardunstablebloodDef))]
	public sealed class cardunstableblood : lvalonexrumiaCard
	{
		public int hascount = 0;
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			hascount = 0;
			yield return new ChangeLifeAction(-heal);
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				if (unit.HasStatusEffect<sebleed>()) { hascount += Value2; }
				yield return new ApplyStatusEffectAction<sebleed>(unit, 1, 0, 0, 0, 0.2f);
			}
			if (hascount > 0)
			{
				yield return new ApplyStatusEffectAction<sebloodstorm>(Battle.Player, hascount, 0, 0, 0, 0.2f);
			}
			yield return AttackAction(selector, GunName);
			yield break;
		}
	}
}


