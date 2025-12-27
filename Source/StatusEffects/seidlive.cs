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
	public sealed class seidliveDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.Order = int.MaxValue;
			return config;
		}
	}

	[EntityLogic(typeof(seidliveDef))]
	public sealed class seidlive : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, new EventSequencedReactor<ChangeLifeEventArgs>(OnLifeChanged));
			foreach (Unit forunit in Battle.AllAliveUnits)
			{
				ReactOwnerEvent(forunit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
			}
			HandleOwnerEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
			ReactOwnerEvent(Battle.Player.TurnStarted, OnPlayerTurnStarted);
			HandleOwnerEvent(Owner.Dying, OnDying);
			ReactOwnerEvent(Battle.BattleEnding, OnBattleEnding);
			ReactOwnerEvent(Battle.RoundEnded, OnRoundEnded);
		}

		private IEnumerable<BattleAction> OnRoundEnded(GameEventArgs args)
		{
			if (GameRun.Player.Hp <= 0)
			{
				NotifyActivating();
				yield return new ForceKillAction(Battle.Player, Battle.Player);
			}
			yield return new RemoveStatusEffectAction(this);
		}

		private IEnumerable<BattleAction> OnBattleEnding(GameEventArgs args)
		{
			if (GameRun.Player.Hp <= 0)
			{
				NotifyActivating();
				GameRun.SetHpAndMaxHp(1, GameRun.Player.MaxHp, true);
			}
			yield break;
		}

		private void OnDying(DieEventArgs args)
		{
			NotifyActivating();
			args.CancelBy(this);
		}

		private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
		{
			yield break;
		}

		private void OnEnemySpawned(UnitEventArgs args)
		{
			ReactOwnerEvent(args.Unit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
		}

		private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			if (args.DamageInfo.Damage == 0) { yield break; }
			foreach (var action in HandleLifeChanged(args.Target, (int)args.DamageInfo.Damage * -1, args.Source, args.Cause, args.ActionSource))
			{
				yield return action;
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (args.Amount == 0) { yield break; }
			foreach (var action in HandleLifeChanged(args.argsunit, args.Amount, Battle.Player, args.Cause, args.ActionSource))
			{
				yield return action;
			}
			yield break;
		}
		private IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			// if (Battle.BattleShouldEnd) { yield break; }
			// if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player) && !Battle.BattleShouldEnd && -amount >= toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 1, true)) //player decreases life / takes dmg >= 1%
			// {
			// 	int pct = -amount * 100 / GameMaster.Instance.CurrentGameRun.Player.MaxHp;
			// 	NotifyActivating();
			// 	foreach (Unit unit in Battle.AllAliveEnemies)
			// 	{
			// 		if (!unit.IsAlive || Battle.BattleShouldEnd) { continue; }
			// 		yield return new ChangeLifeAction(-toolbox.hpfrompercent(unit, pct, true), unit);
			// 	}
			// }
			yield break;
		}
	}
}