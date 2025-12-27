using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.Cards.Template;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardexaaDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 2 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 10;
			config.UpgradedDamage = 14;
			config.GunName = GunNameID.GetGunFromId(4610);
			config.GunNameBurst = GunNameID.GetGunFromId(4610);

			//The card is treated as a basic card. 
			config.Keywords = Keyword.Basic;
			config.UpgradedKeywords = Keyword.Basic;

			config.Illustrator = "녕안";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardexaaDef))]
	public sealed class cardexaa : lvalonexrumiaCard
	{
		//By default, if config.Damage / config.Block / config.Shield are set:
		//The card will deal damage or gain Block/Barrier without having to set anything.
		//Here, this is is equivalent to the following code.

		/*protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction(true); 
        }*/
	}
}


