using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class setwilight2Def : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.HasCount = true;
			config.Order = 3;
			return config;
		}
	}

	[EntityLogic(typeof(setwilight2Def))]
	public sealed class setwilight2 : StatusEffect
	{
		bool between = true;
		bool go = true;
		private readonly List<(Card, CardUsingEventArgs)> AttackEchoArgs = new List<(Card, CardUsingEventArgs)>();
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
					return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 25, true);
				}
			}
		}
		void updatecounter()
		{
			Count = lifeneed;
			Highlight = Owner.Hp < lifeneed;
		}
		protected override void OnAdded(Unit unit)
		{
			between = true;
			go = true;
			updatecounter();
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
			HandleOwnerEvent(Battle.Player.DamageReceived, OnDamageReceived);
			HandleOwnerEvent(CustomGameEventManager.PostChangeLifeEvent, OnLifeChanged);
			HandleOwnerEvent(CustomGameEventManager.PreChangeLifeEvent, OnLifeChanging);
			HandleOwnerEvent(Battle.Player.HealingReceived, OnHealingReceived);
			ReactOwnerEvent(Battle.BattleEnded, OnBattleEnded, 0);
		}
		private IEnumerable<BattleAction> OnBattleEnded(GameEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
		}

		private void OnLifeChanging(ChangeLifeEventArgs args)
		{
			if (args.Cause == ActionCause.Card && !between)
			{
				Card card = args.ActionSource as Card;
				if (card is cardredblood)
				{
					args.CancelBy(this);
				}
			}
		}

		private void OnHealingReceived(HealEventArgs args)
		{
			if (!go || Battle.BattleShouldEnd) { return; }
			updatecounter();
		}

		private void OnLifeChanged(ChangeLifeEventArgs args)
		{
			if (!go) { return; }
			updatecounter();
		}

		private void OnDamageReceived(DamageEventArgs args)
		{
			if (!go) { return; }
			updatecounter();
		}
		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			go = false;
			if (Owner.Hp < lifeneed)
			{
				for (int i = 0; i < Level; i++)
				{
					Card token = Library.CreateCard<cardredblood>();
					token.IsPlayTwiceToken = true;
					token.PlayTwiceSourceCard = args.Card;
					AttackEchoArgs.Add((token, args.Clone()));
				}
			}

			between = false;
			foreach ((Card card, CardUsingEventArgs aargs) in AttackEchoArgs)
			{
				NotifyActivating();
				yield return new PlayTwiceAction(card, aargs);
			}
			AttackEchoArgs.Clear();
			go = true;
			between = true;
			updatecounter();
			yield break;
		}
	}
}