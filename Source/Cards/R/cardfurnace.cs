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
using System.Linq;
using LBoL.Core.Cards;

namespace lvalonexrumia.Cards
{
	public sealed class cardfurnaceDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Red };
			config.Cost = new ManaGroup() { Red = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //heal
			config.Value2 = 1; //red blood
			config.UpgradedValue2 = 2;

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(seatkincrease) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(seatkincrease) };

			config.RelativeCards = new List<string>() { nameof(cardredblood) };
			config.UpgradedRelativeCards = new List<string>() { nameof(cardredblood) };

			config.Illustrator = "ラシャ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardfurnaceDef))]
	public sealed class cardfurnace : lvalonexrumiaCard
	{
		public override bool Triggered => Battle != null && Battle.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(-heal);
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, 20, 0, 0, 0, 0.2f);
			yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(1, false));
			if (TriggeredAnyhow)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Value2, false));
			}
			yield break;
		}
	}
}


