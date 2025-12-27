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
	public sealed class setornDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(seatkincrease), nameof(seincrease) };
			config.HasCount = true;
			return config;
		}
	}

	[EntityLogic(typeof(setornDef))]
	public sealed class setorn : StatusEffect
	{
		public int lifeneed
		{
			get
			{
				if (Owner == null)
				{
					return 0;
				}
				else
				{
					return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true);
				}
			}
		}
		public int increase
		{
			get
			{
				if (Owner == null)
				{
					return 0;
				}
				return Level * 10;
			}
		}
		public int increaselife
		{
			get
			{
				if (Owner == null)
				{
					return 0;
				}
				return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Level * 10, true);
			}
		}
		protected override void OnAdded(Unit unit)
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
			ReactOwnerEvent(Battle.Player.DamageReceived, OnPlayerDamageReceived);
			ReactOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			ReactOwnerEvent(Battle.BattleEnded, OnBattleEnded, 0);
			HandleOwnerEvent(Battle.Player.HealingReceived, OnHealingReceived);
		}
		private IEnumerable<BattleAction> OnBattleEnded(GameEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
		}

		private void OnHealingReceived(HealEventArgs args)
		{
			if (Battle.BattleShouldEnd) { return; }
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
		}

		private IEnumerable<BattleAction> OnLifeChanged(ChangeLifeEventArgs args)
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
			yield break;
		}

		private IEnumerable<BattleAction> OnPlayerDamageReceived(DamageEventArgs args)
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
			if (args.DamageInfo.IsGrazed && !Battle.BattleShouldEnd && Battle.Player.Hp < toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 50, true))
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, Level * 10, 0, 0, 0, 0.2f);
				yield return new ChangeLifeAction(toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, Level * 10, true));
			}
		}
	}
}