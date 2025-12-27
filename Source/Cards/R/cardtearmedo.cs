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

namespace lvalonexrumia.Cards
{
	public sealed class cardtearmedoDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 2 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;
			config.TargetType = TargetType.Self;

			config.Value1 = 1; //blood storm token
			config.Value2 = 5; //atk increase

			config.UpgradedKeywords = Keyword.Initial | Keyword.Replenish;

			config.RelativeEffects = new List<string>() { nameof(sebloodstorm), nameof(seatkincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sebloodstorm), nameof(seatkincrease) };

			config.RelativeCards = new List<string>() { nameof(cardbloodstorm) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardbloodstorm) };

			config.Illustrator = "黒夢";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtearmedoDef))]
	public sealed class cardtearmedo : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<setearmedo>(Battle.Player, 1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


