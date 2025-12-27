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
	public sealed class cardbiteDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 13;
			config.UpgradedDamage = 16;
			config.Value1 = 1; //dark blood token
			config.UpgradedValue1 = 2;

			config.GunName = GunNameID.GetGunFromId(7020);
			config.GunNameBurst = GunNameID.GetGunFromId(7021);

			config.RelativeEffects = new List<string>() { nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedarkblood) };

			config.RelativeCards = new List<string>() { nameof(carddarkblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood) };

			config.Illustrator = "えすれき＠垢移行";
			config.SubIllustrator = new List<string>() { "ワイテイ" };

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbiteDef))]
	public sealed class cardbite : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


