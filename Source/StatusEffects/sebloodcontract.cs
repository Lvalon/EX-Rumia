using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebloodcontractDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sebloodsword) };
			return config;
		}
	}

	[EntityLogic(typeof(sebloodcontractDef))]
	public sealed class sebloodcontract : sereact
	{
		protected override IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			if (Battle.BattleShouldEnd) { yield break; }
			if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player) && !Battle.BattleShouldEnd) //player decreases life / takes dmg
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<sebloodsword>(Battle.Player, Level, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}