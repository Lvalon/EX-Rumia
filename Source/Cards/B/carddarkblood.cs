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

namespace lvalonexrumia.Cards
{
	public sealed class carddarkbloodDef : lvalonexrumiaCardTemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;

			config.Colors = new List<ManaColor>() { ManaColor.Black };
			config.Cost = new ManaGroup() { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;
			config.TargetType = TargetType.Self;

			config.Value1 = 5; //heal percentage
			config.Value2 = 1; //draw and accelerate

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile;

			config.RelativeEffects = new List<string>() { nameof(seincrease), nameof(seaccel), nameof(sedarkblood) };
			config.UpgradedRelativeEffects = new List<string>() { nameof(seincrease), nameof(Graze), nameof(sedarkblood) };

			config.Illustrator = "トト";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(carddarkbloodDef))]
	public sealed class carddarkblood : lvalonexrumiaCard
	{
		protected override int heal => toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Value1, true);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ChangeLifeAction(heal);
			yield return new DrawManyCardAction(Value2);
			if (!IsUpgraded && !Battle.Player.TryGetStatusEffect(out seaccel _))
			{
				yield return new ApplyStatusEffectAction<seaccel>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			}
			if (IsUpgraded)
			{
				yield return new ApplyStatusEffectAction<Graze>(Battle.Player, Value2, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}


