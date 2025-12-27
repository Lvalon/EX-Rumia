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
	public sealed class cardnoradrenalineDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Hybrid = 2, HybridColor = 7 };
			config.UpgradedCost = new ManaGroup() { Hybrid = 2, HybridColor = 7 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.All;

			config.GunName = GunNameID.GetGunFromId(7060);
			config.GunNameBurst = GunNameID.GetGunFromId(7061);

			config.Damage = 5;
			config.Value1 = 5; //Percentage of max HP to heal
			config.Value2 = 2; //red blood

			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark), nameof(sebleed) };

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "楠なのは";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardnoradrenalineDef))]
	public sealed class cardnoradrenaline : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		public override bool Triggered => Battle != null && Battle.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			for (int i = 0; i < 3; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				if (!IsUpgraded)
				{
					if (Battle.AllAliveEnemies.Count(x => x.HasStatusEffect<sebleed>() || x.HasStatusEffect<sebloodmark>()) > 0)
					{
						yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies.Where(x => x.HasStatusEffect<sebleed>() || x.HasStatusEffect<sebloodmark>()).ToArray(), Damage, GunName, GunType.Single);
					}
				}
				else
				{
					yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, GunName, GunType.Single);
				}
			}
			if (Battle.BattleShouldEnd) { yield break; }
			if (TriggeredAnyhow)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value2, false));
			}
			yield break;
		}
	}
}


