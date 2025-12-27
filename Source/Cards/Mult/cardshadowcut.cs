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
using LBoL.EntityLib.Cards.Neutral.Black;
using lvalonexrumia.GunName;

namespace lvalonexrumia.Cards
{
	public sealed class cardshadowcutDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();

			config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };
			config.Cost = new ManaGroup() { Any = 3, Black = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.AllEnemies;

			config.Damage = 5;

			config.Value1 = 5; //shadows to add
			config.Value2 = 5; //times to attack

			config.GunName = GunNameID.GetGunFromId(7161);
			config.GunNameBurst = GunNameID.GetGunFromId(7161);

			config.Keywords = Keyword.Exile | Keyword.Accuracy | Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Accuracy | Keyword.Retain;

			config.RelativeEffects = new List<string>() { nameof(ExtraTurn) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(ExtraTurn) };

			config.Illustrator = "初屋";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardshadowcutDef))]
	public sealed class cardshadowcut : lvalonexrumiaCard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return PerformAction.Spell(Battle.Player, "exshadowcut");
			yield return new AddCardsToHandAction(Library.CreateCards<Shadow>(Value1, false));
			yield return PerformAction.Effect(Battle.Player, "ExtraTime");
			yield return PerformAction.Sfx("ExtraTurnLaunch");
			yield return PerformAction.Animation(Battle.Player, "spell", 1.6f);
			yield return BuffAction<ExtraTurn>(1);
			yield return new ApplyStatusEffectAction<seshadowcut>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			yield return new RequestEndPlayerTurnAction();
			yield break;
		}
	}
}


