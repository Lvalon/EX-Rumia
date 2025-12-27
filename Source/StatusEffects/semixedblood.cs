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
	public sealed class semixedbloodDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sedarkblood) };
			return config;
		}
	}

	[EntityLogic(typeof(semixedbloodDef))]
	public sealed class semixedblood : StatusEffect
	{
		private readonly List<(Card, CardUsingEventArgs)> AttackEchoArgs = new List<(Card, CardUsingEventArgs)>();
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
			ReactOwnerEvent(Battle.Player.TurnEnded, OnTurnEnded);
		}

		private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
			yield break;
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			if (args.Card is carddarkblood)
			{
				for (int i = 0; i < Level; i++)
				{
					Card token = Library.CreateCard<cardredblood>();
					token.IsPlayTwiceToken = true;
					token.PlayTwiceSourceCard = args.Card;
					AttackEchoArgs.Add((token, args.Clone()));
				}
			}
			foreach ((Card card, CardUsingEventArgs aargs) in AttackEchoArgs)
			{
				NotifyActivating();
				yield return new PlayTwiceAction(card, aargs);
			}
			if (args.ActionSource != this && args.Card is cardredblood)
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<sedarkblood>(Battle.Player, Level, 0, 0, 0, 0.2f);
			}
			AttackEchoArgs.Clear();
			yield break;
		}
	}
}