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
using LBoL.Core.Cards;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardpastpresentDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red, ManaColor.Green };
			config.Cost = new ManaGroup() { Black = 1, Red = 1, Green = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.GunName = GunNameID.GetGunFromId(13200);
			config.GunNameBurst = GunNameID.GetGunFromId(13201);

			config.Damage = 2;
			config.Value1 = 1;

			config.Keywords = Keyword.Accuracy | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Accuracy | Keyword.Retain | Keyword.Initial;

			config.Illustrator = "ポップル";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardpastpresentDef))]
	public sealed class cardpastpresent : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			yield return new ApplyStatusEffectAction<sepastpresent>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield break;
		}

	}
}


