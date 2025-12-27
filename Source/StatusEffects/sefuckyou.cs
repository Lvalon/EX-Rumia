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
using lvalonexrumia.Patches;
using lvalonmeme.StatusEffects;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sefuckyouDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sedecrease) };
			config.HasCount = true;
			config.Order = 1;
			return config;
		}
	}

	[EntityLogic(typeof(sefuckyouDef))]
	public sealed class sefuckyou : sereact
	{
		public override bool ForceNotShowDownText => true;
		protected override IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			if (Battle.BattleShouldEnd) { yield break; }
			if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player) && !Battle.BattleShouldEnd && -amount >= toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 1, true)) //player decreases life / takes dmg >= 1%
			{
				int pct = -amount * 100 / GameMaster.Instance.CurrentGameRun.Player.MaxHp;
				Count = Math.Max(0, Count - pct);
				if (Count == 0)
				{
					NotifyActivating();
					Battle.RequestDebugAction(new InstantWinAction(), "lvalonexrumia: Mutual Depletion+ instawin effect");
				}
			}
			yield break;
		}
	}
}