using System;
using System.Collections.Generic;
using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoL.Presentation.Effect;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.Units;

namespace lvalonexrumia.Patches
{
	public class ChangeLifeEventArgs : GameEventArgs
	{
		public int Amount { get; set; }
		public Unit argsunit { get; set; }

		// not working

		public override string GetBaseDebugString()
		{
			return "Life Changed: " + this.Amount + " for " + (this.argsunit != null ? this.argsunit.Name : "Player");
		}
	}
	[HarmonyPatch]
	public sealed class ChangeLifeAction : SimpleEventBattleAction<ChangeLifeEventArgs>
	{
		internal ChangeLifeAction(int amount = 1, Unit unit = null)
		{
			this.Args = new ChangeLifeEventArgs
			{
				Amount = amount,
				argsunit = unit
			};
		}

		public override void PreEventPhase()
		{
			this.Trigger(CustomGameEventManager.PreChangeLifeEvent);
		}
		public override void MainPhase()
		{
			BattleAction ba = null;
			var gamerun = GameMaster.Instance.CurrentGameRun;
			var enemy = Args.argsunit as EnemyUnit;

			//number display
			UnitView target = GameDirector.GetUnit(Battle.Player);
			if (enemy != null && enemy.IsAlive)
			{
				target = GameDirector.GetUnit(enemy);
			}
			bool positive = Args.Amount >= 0;
			PopupHud.Instance.PopupFromScene(Math.Abs(Args.Amount), positive ? PopupHud.HealColor : PopupHud.PlayerHitColor, target.transform.position);
			EffectManager.CreateEffect(positive ? "UnitHealLarge" : "BloodHit", target.EffectRoot, local: true);
			AudioManager.PlayUi(positive ? "HealLarge" : "Bamuman");

			if (Args.argsunit == null || Args.argsunit == Battle.Player) //player is target
			{
				if (gamerun.Player.Hp + Args.Amount > gamerun.Player.MaxHp)
				{
					gamerun.SetHpAndMaxHp(gamerun.Player.MaxHp, gamerun.Player.MaxHp, true);
					return;
				}
				if (gamerun.Player.Hp + Args.Amount <= 0)
				{
					base.React(new ForceKillAction(base.Battle.Player, base.Battle.Player));
				}
				else
				{
					gamerun.SetHpAndMaxHp(gamerun.Player.Hp + Args.Amount, gamerun.Player.MaxHp, true);
				}
			}
			else if (enemy != null && enemy.IsAlive) //valid enemy target
			{
				if (enemy.Hp + Args.Amount > enemy.MaxHp)
				{
					gamerun.SetEnemyHpAndMaxHp(enemy.MaxHp, enemy.MaxHp, enemy, true);
					return;
				}
				if (enemy.Hp + Args.Amount <= 0)
				{
					base.React(new ForceKillAction(base.Battle.Player, enemy));
				}
				else
				{
					gamerun.SetEnemyHpAndMaxHp(enemy.Hp + Args.Amount, enemy.MaxHp, enemy, true);
				}
			}
			if (null != ba)
			{
				this.React(new Reactor(ba));
			}
		}
		public override void PostEventPhase()
		{
			this.Trigger(CustomGameEventManager.PostChangeLifeEvent);
		}
	}
	// public sealed class ChangeLifeAction : SimpleAction
	// {        
	//     private readonly ChangeLifeEventArgs Args;

	//     internal ChangeLifeAction(int amount = 1, Unit unit = null)
	// 	{
	// 		this.Args = new ChangeLifeEventArgs
	//         {
	//             Amount = amount,
	//             argsunit = unit
	// 		};
	// 	}

	// 	public override IEnumerable<Phase> GetPhases()
	// 	{
	//         yield return base.CreateEventPhase<ChangeLifeEventArgs>("Pre Life Changed", this.Args, CustomGameEventManager.PreChangeLifeEvent);

	//         yield return base.CreatePhase("Main", delegate
	//         {
	//             if (Args.Amount == 0)
	//             {
	//                 return;
	//             }
	//             var gamerun = GameMaster.Instance.CurrentGameRun;
	//             var enemy = Args.argsunit as EnemyUnit;
	//             if (Args.argsunit == null)
	//             {
	//                 if (gamerun.Player.Hp + Args.Amount <= 0)
	//                 {
	//                     base.React(new ForceKillAction(base.Battle.Player, base.Battle.Player));
	//                 }
	//                 else
	//                 {
	//                     gamerun.SetHpAndMaxHp(gamerun.Player.Hp + Args.Amount, gamerun.Player.MaxHp, true);
	//                 }
	//             }
	//             else if (enemy != null)
	//             {
	//                 if (enemy.Hp + Args.Amount <= 0)
	//                 {
	//                     base.React(new ForceKillAction(base.Battle.Player, enemy));
	//                 }
	//                 else
	//                 {
	//                     gamerun.SetEnemyHpAndMaxHp(enemy.Hp + Args.Amount, enemy.MaxHp, enemy, true);
	//                 }
	//             }
	//         }, false);

	//         yield return base.CreateEventPhase<ChangeLifeEventArgs>("Life Changed", this.Args, CustomGameEventManager.PostChangeLifeEvent);
	//         yield break;
	// 	}
	// }
}
