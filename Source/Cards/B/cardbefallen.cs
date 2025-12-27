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
using LBoL.EntityLib.Cards.Neutral.Black;

namespace lvalonexrumia.Cards
{
	public sealed class cardbefallenDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Black = 1 };
			config.UpgradedCost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //decrease percentage
			config.Value2 = 3; //dark blood token, shadow

			config.RelativeEffects = new List<string>() { nameof(sedecrease), nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(sedecrease), nameof(sedarkblood) };

			config.RelativeCards = new List<string>() { nameof(carddarkblood), nameof(Shadow) };
			config.UpgradedRelativeCards = new List<string>() { nameof(carddarkblood), nameof(Shadow) };

			config.Illustrator = "蜂鳥 あかり";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbefallenDef))]
	public sealed class cardbefallen : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, false);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield return new ChangeLifeAction(-heal);
			yield return new AddCardsToHandAction(Library.CreateCards<Shadow>(1, false));
			yield break;
		}
	}
}


