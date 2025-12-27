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
using LBoL.Core.Cards;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardbloodstormDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 10;
			config.Value1 = 1; //bleed

			config.GunName = GunNameID.GetGunFromId(4532);
			config.GunNameBurst = GunNameID.GetGunFromId(4532);

			config.Keywords = Keyword.Exile | Keyword.AutoExile | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Exile | Keyword.AutoExile | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sebleed), nameof(sebloodstorm) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebleed), nameof(sebloodmark), nameof(sebloodstorm) };

			config.Illustrator = "キトボシ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodstormDef))]
	public sealed class cardbloodstorm : lvalonexrumiaCard
	{
		public override bool OnExileVisual => false;
		public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
		{
			if (Battle.BattleShouldEnd || srcZone != CardZone.Hand)
			{
				return null;
			}

			return LeaveHandReactor();
		}

		private IEnumerable<BattleAction> LeaveHandReactor()
		{
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new PlayCardAction(this);
			yield break;
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			foreach (Unit enemy in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd)
				{
					yield break;
				}
				if (enemy.IsAlive)
				{
					yield return new ApplyStatusEffectAction<sebleed>(enemy, Value1, 0, 0, 0, 0.2f);
					if (IsUpgraded && enemy.IsAlive)
					{
						yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value1, 0, 0, 0, 0.2f);
					}
				}
			}
			yield break;
		}
	}
}


