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
	public sealed class seenduranceDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.HasCount = true;
			return config;
		}
	}

	[EntityLogic(typeof(seenduranceDef))]
	public sealed class seendurance : sereact
	{
		protected override void dosmth()
		{
			Highlight = Owner.Hp < lifeneed;
		}
		protected override void OnHealingReceived(HealEventArgs args)
		{
			dosmth();
		}
		protected override IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
		{
			if (Battle.BattleShouldEnd) { yield break; }
			if (Count > 0)
			{
				NotifyActivating();
				yield return new AddCardsToHandAction(Library.CreateCards<cardredblood>(Count, false));
			}
			Count = 0;
			yield break;
		}
		protected override IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			Highlight = Owner.Hp < lifeneed;
			if (Battle.BattleShouldEnd) { yield break; }
			if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player) && !Battle.BattleShouldEnd && Battle.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true)) //player decreases life / takes dmg
			{
				NotifyActivating();
				Count += Level;
			}
			yield break;
		}
	}
}