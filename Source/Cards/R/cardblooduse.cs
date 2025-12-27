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
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardblooduseDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 10;
			config.UpgradedDamage = 14;
			config.Value1 = 1; //red blood

			config.GunName = GunNameID.GetGunFromId(23051);
			config.GunNameBurst = GunNameID.GetGunFromId(23052);

			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "東天紅";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardblooduseDef))]
	public sealed class cardblooduse : lvalonexrumiaCard
	{
		public override bool Triggered => Battle != null && Battle.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			if (Battle.BattleShouldEnd) { yield break; }
			if (TriggeredAnyhow)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value1, false));
			}
			yield break;
		}
	}
}


