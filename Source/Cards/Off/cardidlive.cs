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
using System;

namespace lvalonexrumia.Cards
{
	public sealed class cardidliveDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Colorless };
			config.Cost = new ManaGroup() { Any = 2, Colorless = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Nobody;

			config.Value1 = 1; //ability se num

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(sequickrestore) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sequickrestore) };

			config.Illustrator = "ｼﾞｮﾝﾃﾞｨｰ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardidliveDef))]
	public sealed class cardidlive : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (!Battle.Player.HasStatusEffect<sequickrestore>())
			{
				yield return new ApplyStatusEffectAction<sequickrestore>(Battle.Player, Battle.Player.Hp, 0, (int)Math.Round(1.0 * Battle.Player.Hp / 2), 0, 0.2f);
			}
			yield return new ApplyStatusEffectAction<seidlive>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


