using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sequickrestoreDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Special;
			config.HasCount = true;
			return config;
		}
	}

	[EntityLogic(typeof(sequickrestoreDef))]
	public sealed class sequickrestore : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
		int truecount = 0;
		public int truecounter
		{
			get
			{
				return truecount;
			}
		}
		protected override void OnAdded(Unit unit)
		{
			if (Count != 0)
			{
				truecount = Count;
			}
			else
			{
				truecount = Owner.Hp;
				Count = truecount;
			}
			ReactOwnerEvent(Battle.RoundEnded, OnTurnEnding);
			ReactOwnerEvent(Battle.BattleEnding, OnBattleEnding);
		}

		private IEnumerable<BattleAction> OnBattleEnding(GameEventArgs args)
		{
			if (Battle.Player.TryGetStatusEffect(out seidlive _))
			{
				NotifyActivating();
				GameRun.SetHpAndMaxHp(truecount, GameRun.Player.MaxHp, true);
			}
			yield break;
		}

		private IEnumerable<BattleAction> OnTurnEnding(GameEventArgs args)
		{
			if (Battle.BattleShouldEnd) { yield break; }
			if (truecount > 0)
			{
				if (truecount > GameRun.Player.MaxHp)
				{
					truecount = GameRun.Player.MaxHp;
				}
				NotifyActivating();
				GameRun.SetHpAndMaxHp(truecount, GameRun.Player.MaxHp, true);
			}
			yield return new RemoveStatusEffectAction(this);
			yield break;
		}
	}
}