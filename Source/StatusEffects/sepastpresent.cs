using System;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sepastpresentDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.IsStackable = false;
			return config;
		}
	}

	[EntityLogic(typeof(sepastpresentDef))]
	public sealed class sepastpresent : StatusEffect
	{
		bool go = false;
		public override bool ForceNotShowDownText => true;
		private readonly List<(Card, CardUsingEventArgs)> AttackEchoArgs = new List<(Card, CardUsingEventArgs)>();
		protected override void OnAdded(Unit unit)
		{
			go = false;
			HandleOwnerEvent(Battle.CardUsed, OnCardUsed);
			ReactOwnerEvent(Battle.Player.TurnStarted, OnTurnStarted);
			HandleOwnerEvent(Battle.Player.TurnEnded, OnTurnEnded);
		}

		private void OnTurnEnded(UnitEventArgs args)
		{
			go = true;
		}

		private void OnCardUsed(CardUsingEventArgs args)
		{
			Card token = args.Card.CloneTwiceToken();
			token.IsPlayTwiceToken = true;
			token.PlayTwiceSourceCard = args.Card;
			AttackEchoArgs.Add((token, args.Clone()));
		}

		private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
		{
			if (!go) { yield break; }
			Highlight = true;
			foreach ((Card card, CardUsingEventArgs aargs) in AttackEchoArgs)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				NotifyActivating();
				yield return new PlayTwiceAction(card, aargs);
			}
			AttackEchoArgs.Clear();
			yield return new RemoveStatusEffectAction(this);
			yield break;
		}
	}
}