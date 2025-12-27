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
	public sealed class cardbloodsuckerDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 2 };
			config.UpgradedCost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.GunName = GunNameID.GetGunFromId(7020);
			config.GunNameBurst = GunNameID.GetGunFromId(7021);

			config.Damage = 10;
			config.Value1 = 1; //red blood

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "真塚ケイ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodsuckerDef))]
	public sealed class cardbloodsucker : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value1, false));
			yield break;
		}
	}
}


