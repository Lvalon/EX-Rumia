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
	public sealed class cardbloodlaserDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 1, Black = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 7;
			config.UpgradedDamage = 10;
			config.Value1 = 5; //decrease percent
			config.Value2 = 1; //bloodmark

			config.GunName = GunNameID.GetGunFromId(23061);
			config.GunNameBurst = GunNameID.GetGunFromId(23061);

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy | Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sebloodmark) };

			config.Illustrator = "tyourou god";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbloodlaserDef))]
	public sealed class cardbloodlaser : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<sebloodmark>(selector.SelectedEnemy, Value2, 0, 0, 0, 0.2f);
			yield return AttackAction(selector, GunName);
			yield break;
		}
	}
}


