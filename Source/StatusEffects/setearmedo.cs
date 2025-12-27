using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;

namespace lvalonexrumia.StatusEffects
{
	public sealed class setearmedoDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sebloodstorm), nameof(seatkincrease) };
			return config;
		}
	}

	[EntityLogic(typeof(setearmedoDef))]
	public sealed class setearmedo : sereact
	{
		public int atkincrease
		{
			get
			{
				if (Owner == null)
				{
					return 0;
				}
				return Level * 5;
			}
		}
		protected override IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			if (Battle.BattleShouldEnd) { yield break; }
			if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player) && !Battle.BattleShouldEnd) //player decreases life / takes dmg
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<sebloodstorm>(Battle.Player, Level, 0, 0, 0, 0.2f);
				yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, Level * 5, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}