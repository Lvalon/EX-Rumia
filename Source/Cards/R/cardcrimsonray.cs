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
	public sealed class cardcrimsonrayDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 1, Red = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 20;
			config.UpgradedDamage = 30;
			config.Value1 = 5; //heal
			config.Value2 = 1; //bleed

			config.GunName = GunNameID.GetGunFromId(12020);
			config.GunNameBurst = GunNameID.GetGunFromId(12021);

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebleed) };

			config.Illustrator = "ながくま ☆ ろこ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardcrimsonrayDef))]
	public sealed class cardcrimsonray : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<sebleed>(selector.SelectedEnemy, Value2, 0, 0, 0, 0.2f);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return AttackAction(selector, GunName);
			yield break;
		}
	}
}


