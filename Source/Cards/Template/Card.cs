using System;
using System.Collections.Generic;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Presentation;
using lvalonexrumia.Patches;

namespace lvalonexrumia.Cards.Template
{
	public class lvalonexrumiaCard : Card
	{
		protected virtual int heal { get; set; } = 0;
		public int healnum
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					// if (GameMaster.Instance.CurrentGameRun.Battle != null)
					// {
					return heal;
					// }
				}
				return 0;
			}
		}
		public int healnum3
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					int lifeafter = GameMaster.Instance.CurrentGameRun.Player.Hp - heal;
					int lifeafter2 = Convert.ToInt32(Math.Round((double)lifeafter * Value1 / 100, MidpointRounding.AwayFromZero));
					return lifeafter2;
				}
				return 0;
			}
		}
		public string healstring //SPACE ON THE RIGHT
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					// if (GameMaster.Instance.CurrentGameRun.Battle != null)
					// {
					return "(" + heal.ToString() + ") ";
					// }
				}
				return "";
			}
		}
		public string healstringnos
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					// if (GameMaster.Instance.CurrentGameRun.Battle != null)
					// {
					return " (" + heal.ToString() + ")";
					// }
				}
				return "";
			}
		}
		public string healstrings
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					// if (GameMaster.Instance.CurrentGameRun.Battle != null)
					// {
					return " (" + heal.ToString() + ") ";
					// }
				}
				return "";
			}
		}
		public string healstring2
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					// if (GameMaster.Instance.CurrentGameRun.Battle != null)
					// {
					return " (" + heal.ToString() + "," + healnum3.ToString() + ")";
					// }
				}
				return "";
			}
		}
		public string healstring2s //SPACE ON BOTH SIDES
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun != null)
				{
					// if (GameMaster.Instance.CurrentGameRun.Battle != null)
					// {
					return " (" + heal.ToString() + "," + healnum3.ToString() + ") ";
					// }
				}
				return "";
			}
		}
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
		public string needstring
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun == null)
				{
					return "";
				}
				else
				{
					return "(" + lifeneed.ToString() + ") ";
				}
			}
		}
		public string needstring25
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun == null)
				{
					return "";
				}
				else
				{
					return "(" + toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 25, true).ToString() + ") ";
				}
			}
		}
		public string needstring15
		{
			get
			{
				if (GameMaster.Instance.CurrentGameRun == null)
				{
					return "";
				}
				else
				{
					return "(" + toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 15, true).ToString() + ") ";
				}
			}
		}
		//lvalonexrumiaCard can be used to give additional properties to all the cards.
		//For instance, this can be used to give every card a new custom parameter called Value3. 
		//Custom value for display purposes.
		protected virtual int BaseValue3 { get; set; } = 0;
		protected virtual int BaseUpgradedValue3 { get; set; } = 0;
		public int Value3
		{
			get
			{
				if (this.IsUpgraded)
				{
					return BaseUpgradedValue3;
				}
				return BaseValue3;
			}
		}
	}
	public abstract class reactcard : lvalonexrumiaCard
	{
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.BattleEnded, OnBattleEnded, 0);
			HandleBattleEvent(Battle.Player.HealingReceived, OnHealingReceived);
			HandleBattleEvent(Battle.BattleStarting, new GameEventHandler<GameEventArgs>(OnBattleStarting));
		}

		private IEnumerable<BattleAction> OnBattleEnded(GameEventArgs args)
		{
			yield return new RemoveCardAction(this);
		}

		protected virtual void OnBattleStarting(GameEventArgs args)
		{
			ReactBattleEvent(CustomGameEventManager.PostChangeLifeEvent, new EventSequencedReactor<ChangeLifeEventArgs>(OnLifeChanged));
			foreach (Unit unit in Battle.AllAliveUnits)
			{
				ReactBattleEvent(unit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
			}
			HandleBattleEvent(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(OnEnemySpawned));
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

		protected virtual void OnEnemySpawned(UnitEventArgs args)
		{
			ReactBattleEvent(args.Unit.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
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



		// protected override IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		// {
		// 	if (actionSource != this)
		// 	{
		// 		if (receive == null || receive == Battle.Player) //player
		// 		{
		// 			if (amount > 0) //player gain life
		// 			{
		// 			}
		// 			if (amount < 0) //player lose life
		// 			{
		// 			}
		// 		}
		// 		else if (receive as EnemyUnit != null)
		// 		{
		// 			if (amount < 0) //enemy gain life
		// 			{
		// 			}
		// 		}
		// 	}
		// 	yield break;
		// }
	}
}