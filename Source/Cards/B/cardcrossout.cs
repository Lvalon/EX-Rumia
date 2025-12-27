using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonexrumia.StatusEffects;
using lvalonmeme.StatusEffects;
using LBoL.Core.StatusEffects;
using lvalonexrumia.Patches;

namespace lvalonexrumia.Cards
{
	public sealed class cardcrossoutDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.IsXCost = true;
			config.Rarity = Rarity.Common;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Block = 5;
			config.Value1 = 1; //blood clot
			config.UpgradedValue1 = 2;

			config.Mana = new ManaGroup() { Any = 1 };

			config.RelativeKeyword = Keyword.Synergy;
			config.UpgradedRelativeKeyword = Keyword.Synergy;

			config.RelativeEffects = new List<string>() { nameof(sebloodclot) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodclot) };

			config.Illustrator = "Yu-Gi-Oh!";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardcrossoutDef))]
	public sealed class cardcrossout : lvalonexrumiaCard
	{
		public ManaGroup Mana2
		{
			get
			{
				return new ManaGroup() { Any = 1 };
			}
		}
		public override ManaGroup GetXCostFromPooled(ManaGroup pooledMana)
		{
			return pooledMana;
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			int Black = SynergyAmount(consumingMana, ManaColor.Any);
			//int Black2 = SynergyAmount(consumingMana, ManaColor.Any, IsUpgraded ? 1 : 2);
			if (Black > 0)
			{
				bool cast = true;
				for (int i = 0; i < Black; i++)
				{
					yield return new CastBlockShieldAction(Battle.Player, Battle.Player, Block, cast);
					cast = false;
					yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1, 0, 0, 0, 0.2f);
				}
			}
			// if (Black2 > 0)
			// {
			// 	for (int i = 0; i < Black2; i++)
			// 	{
			// 		yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			// 	}
			// }
			yield break;
		}
	}
}


