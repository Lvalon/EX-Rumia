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
	public sealed class cardintodarknessDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 5;
			config.Value1 = 1; //blood mark
			config.UpgradedValue1 = 2;

			config.GunName = GunNameID.GetGunFromId(7000);
			config.GunNameBurst = GunNameID.GetGunFromId(7001);

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sebloodmark) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodmark) };

			config.Illustrator = "cervus";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardintodarknessDef))]
	public sealed class cardintodarkness : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sebloodmark>(selector.SelectedEnemy, Value1, 0, 0, 0, 0.2f);
			if (Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebloodmark>()).Count() == 0)
			{
				yield break;
			}
			yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebloodmark>()).ToArray(), Damage, GunName, GunType.Single);
			yield break;
		}
	}
}


