using System;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Patches;
using lvalonexrumia.StatusEffects;

namespace lvalonexrumia.Exhibits
{
	public sealed class exexbDef : lvalonexrumiaExhibitTemplate
	{
		public override ExhibitConfig MakeConfig()
		{

			ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
			exhibitConfig.Value1 = 1;
			exhibitConfig.Mana = new ManaGroup() { Red = 1 };
			exhibitConfig.BaseManaColor = ManaColor.Red;

			exhibitConfig.RelativeEffects = new List<string>() { nameof(sebleed) };

			return exhibitConfig;
		}
	}

	[EntityLogic(typeof(exexbDef))]
	public sealed class exexb : ShiningExhibit
	{
		protected override void OnEnterBattle()
		{
			ReactBattleEvent(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnStarted));
			ReactBattleEvent(Battle.Player.StatisticalTotalDamageDealt, new EventSequencedReactor<StatisticalDamageEventArgs>(OnStatisticalDamageDealt));
			ReactBattleEvent(Battle.Player.DamageReceived, OnDamageReceived);
			ReactBattleEvent(Battle.Player.DamageDealt, OnDamageDealt);
			ReactBattleEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			Active = true;
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
			foreach (var action in HandleLifeChanged(args.argsunit, args.Amount, Battle.Player, args.Cause, args.ActionSource))
			{
				yield return action;
			}
			yield break;
		}

		private IEnumerable<BattleAction> HandleLifeChanged(Unit receive, int amount, Unit source, ActionCause cause, GameEntity actionSource)
		{
			if (Battle.BattleShouldEnd) { yield break; }
			if (actionSource != this && amount < 0 && (receive == null || receive == Battle.Player)) //player
			{
				foreach (Unit unit in Battle.AllAliveEnemies)
				{
					if (unit.IsAlive && !Battle.BattleShouldEnd)
					{
						NotifyActivating();
						yield return new ChangeLifeAction(amount, unit);
					}
				}
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
		{
			Active = true;
			yield break;
		}
		private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
		{
			if (Battle.BattleShouldEnd || !Active)
			{
				yield break;
			}
			if (Active)
			{
				NotifyActivating();
				Active = false;
				foreach (Unit unit in Battle.AllAliveEnemies)
				{
					yield return new ApplyStatusEffectAction<sebleed>(unit, Value1, 0, 0, 0, 0.2f);
				}
			}
			yield break;
		}
		private IEnumerable<BattleAction> OnStatisticalDamageDealt(StatisticalDamageEventArgs args)
		{
			// if (Battle.BattleShouldEnd || !Active)
			// {
			// 	yield break;
			// }
			// foreach (KeyValuePair<Unit, IReadOnlyList<DamageEventArgs>> keyValuePair in args.ArgsTable)
			// {
			// 	Unit unit;
			// 	IReadOnlyList<DamageEventArgs> readOnlyList;
			// 	keyValuePair.Deconstruct(out unit, out readOnlyList);
			// 	Unit unit2 = unit;
			// 	IReadOnlyList<DamageEventArgs> source = readOnlyList;
			// 	if (unit2.IsAlive)
			// 	{
			// 		if (source.Count(delegate (DamageEventArgs damageAgs)
			// 		{
			// 			DamageInfo damageInfo = damageAgs.DamageInfo;
			// 			return damageInfo.DamageType == DamageType.Attack && damageInfo.Amount > 0f;
			// 		}) > 0)
			// 		{
			// 			if (Active)
			// 			{
			// 				NotifyActivating();
			// 			}
			// 			Unit target = unit2;
			// 			int? duration = new int?(Value1);
			// 			yield return new ApplyStatusEffectAction<sebleed>(target, duration, 0, 0, 0, 0.2f);
			// 		}
			// 	}
			// }
			// if (Active)
			// {
			// 	Active = false;
			// }
			yield break;
		}
		protected override void OnLeaveBattle()
		{
			Active = false;
		}
	}
}