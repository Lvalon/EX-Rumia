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
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardletsdanceDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 2, Black = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 5;
			config.UpgradedDamage = 8;
			config.Value1 = 1; // blood mark
			config.UpgradedValue1 = 2;
			config.Value2 = 2; // attack times and graze

			config.GunName = GunNameID.GetGunFromId(7070);
			config.GunNameBurst = GunNameID.GetGunFromId(7071);

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sebloodmark), nameof(Graze) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodmark), nameof(Graze) };

			config.Illustrator = "まめもち";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardletsdanceDef))]
	public sealed class cardletsdance : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			foreach (Unit enemy in Battle.AllAliveEnemies)
			{
				if (!Battle.BattleShouldEnd)
				{
					yield return new ApplyStatusEffectAction<sebloodmark>(enemy, Value1, 0, 0, 0, 0.2f);
				}
			}
			for (int i = 0; i < Value2; i++)
			{
				if (!Battle.BattleShouldEnd)
				{
					yield return AttackAction(selector, GunName);
				}
			}
			if (!Battle.BattleShouldEnd)
			{
				yield return new ApplyStatusEffectAction<Graze>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}


