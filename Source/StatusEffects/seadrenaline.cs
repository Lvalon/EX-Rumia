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
	public sealed class seadrenalineDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(seatkincrease), nameof(sebloodclot) };
			config.HasCount = true;
			return config;
		}
	}

	[EntityLogic(typeof(seadrenalineDef))]
	public sealed class seadrenaline : sereact
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
		protected override void dosmth()
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
		}
		protected override void OnHealingReceived(HealEventArgs args)
		{
			dosmth();
		}
		protected override IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
			if (Battle.BattleShouldEnd) { yield break; }
			if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player) && !Battle.BattleShouldEnd && Battle.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true)) //player decreases life / takes dmg
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, Level * 5, 0, 0, 0, 0.2f);
				yield return new ApplyStatusEffectAction<sebloodclot>(Battle.Player, Level, 0, 0, 0, 0.2f);
			}
			yield break;
		}
	}
}