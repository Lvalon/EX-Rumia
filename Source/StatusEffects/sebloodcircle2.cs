// using System;
// using System.Collections.Generic;
// using LBoL.Base;
// using LBoL.ConfigData;
// using LBoL.Core;
// using LBoL.Core.Battle;
// using LBoL.Core.Battle.BattleActions;
// using LBoL.Core.StatusEffects;
// using LBoL.Core.Units;
// using LBoL.Presentation;
// using LBoLEntitySideloader.Attributes;
// using lvalonexrumia.Patches;

// namespace lvalonexrumia.StatusEffects
// {
// 	public sealed class sebloodcircle2Def : lvalonexrumiaStatusEffectTemplate
// 	{
// 		public override StatusEffectConfig MakeConfig()
// 		{
// 			//fp=4, burst=20, lockon=7, camo=9
// 			StatusEffectConfig config = GetDefaultStatusEffectConfig();
// 			config.Type = StatusEffectType.Positive;
// 			return config;
// 		}
// 	}

// 	[EntityLogic(typeof(sebloodcircle2Def))]
// 	public sealed class sebloodcircle2 : StatusEffect
// 	{
// 		public int heal
// 		{
// 			get
// 			{
// 				if (Owner == null)
// 				{
// 					return 0;
// 				}
// 				if (Level == 0)
// 				{
// 					return 0;
// 				}
// 				return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Level, true);
// 			}
// 		}
// 		protected override void OnAdded(Unit unit)
// 		{
// 			Highlight = true;
// 			HandleOwnerEvent(Owner.TurnEnded, OnTurnEnded);
// 			ReactOwnerEvent(Owner.DamageDealt, OnDamageDealt);
// 		}

// 		private void OnTurnEnded(UnitEventArgs args)
// 		{
// 			Highlight = true;
// 		}

// 		private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
// 		{
// 			if (!Battle.BattleShouldEnd && args.Target.StatusEffects != null && Highlight)
// 			{
// 				DamageInfo damageInfo = args.DamageInfo;
// 				if (damageInfo.DamageType == DamageType.Attack && args.Target.HasStatusEffect<sebloodmark>())
// 				{
// 					NotifyActivating();
// 					yield return new ChangeLifeAction(heal);
// 					Highlight = false;
// 				}
// 			}
// 		}
// 	}
// }