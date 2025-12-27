using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using lvalonexrumia.Patches;
using LBoL.Core.Battle;
using LBoL.Core;
using System.Collections.Generic;
using LBoL.Presentation;
using System;
using LBoL.Core.Battle.BattleActions;

namespace lvalonexrumia.StatusEffects
{
	public abstract class sereact : StatusEffect
	{
		public int lifeneed
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun == null)
				{
					return 0;
				}
				else
				{
					return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
				}
			}
		}
		protected override void OnAdded(Unit unit)
		{
			dosmth();
			ReactOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, new EventSequencedReactor<ChangeLifeEventArgs>(OnLifeChanged));
			foreach (Unit forunit in Battle.AllAliveUnits)
			{
				ReactOwnerEvent(forunit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
			}
			HandleOwnerEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
			ReactOwnerEvent(Battle.BattleEnded, OnBattleEnded, 0);
			HandleOwnerEvent(Battle.Player.HealingReceived, OnHealingReceived);
			ReactOwnerEvent(Battle.Player.TurnStarted, OnPlayerTurnStarted);
		}

		private IEnumerable<BattleAction> OnBattleEnded(GameEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
		}

		protected virtual void dosmth()
		{
		}

		protected virtual void OnHealingReceived(HealEventArgs args)
		{
			if (Battle.BattleShouldEnd) { return; }
		}
		// private IEnumerable<BattleAction> OnHealingReceived(HealEventArgs args)
		// {
		// 	if (args.Amount == 0 || Battle.BattleShouldEnd) { yield break; }
		// 	foreach (var action in HandleLifeChanged(args.Target, (int)args.Amount, args.Source, args.Cause, args.ActionSource))
		// 	{
		// 		yield return action;
		// 	}
		// 	yield break;
		// }

		protected virtual IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
		{
			yield break;
		}

		protected virtual void OnEnemySpawned(UnitEventArgs args)
		{
			ReactOwnerEvent(args.Unit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
		}

		protected virtual IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
		{
			if (args.DamageInfo.Damage == 0) { yield break; }
			foreach (var action in HandleLifeChanged(args.Target, (int)args.DamageInfo.Damage * -1, args.Source, args.Cause, args.ActionSource))
			{
				yield return action;
			}
			yield break;
		}

		protected virtual IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (args.Amount == 0) { yield break; }
			foreach (var action in HandleLifeChanged(args.argsunit, args.Amount, Battle.Player, args.Cause, args.ActionSource))
			{
				yield return action;
			}
			yield break;
		}
		protected abstract IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource);
	}
}