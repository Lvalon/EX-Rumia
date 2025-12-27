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
	public sealed class cardmagiaDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Defense;
			config.TargetType = TargetType.Self;

			config.Block = 10;
			config.Value1 = 1; //graze
			config.Value2 = 2; //blood sword token
			config.UpgradedValue2 = 3;

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(seaccel), nameof(sebloodsword) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seaccel), nameof(sebloodsword) };

			config.RelativeCards = new List<string>() { nameof(cardbloodsword) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodsword) };

			config.Illustrator = "shinh";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardmagiaDef))]
	public sealed class cardmagia : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return DefenseAction(true);
			yield return new ApplyStatusEffectAction<seaccel>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<semagia>(Battle.Player, Value2, 0, 0, 0, 0.2f);
		}
	}
}


