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
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebleedslashDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sebloodsword), nameof(sebloodmark) };
			return config;
		}
	}

	[EntityLogic(typeof(sebleedslashDef))]
	public sealed class sebleedslash : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			//Highlight = true;
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
			HandleOwnerEvent(Battle.Player.TurnEnded, OnTurnEnded);
		}

		private void OnTurnEnded(UnitEventArgs args)
		{
			//Highlight = true;
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			//if (!Highlight) { yield break; }
			if (Battle.BattleShouldEnd) { yield break; }
			if (args.Card.Config.Colors.Contains(ManaColor.Black))// && args.Card.Config.Type == CardType.Attack)
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<sebloodsword>(Battle.Player, Level, 0, 0, 0, 0.2f);
				//yield return new AddCardsToHandAction(Library.CreateCards<cardbloodsword>(Level, false));
				//Highlight = false;
			}
			// if (args.Card.Id == nameof(cardbloodsword) && args.Selector.SelectedEnemy != null && args.Selector.SelectedEnemy.IsAlive)
			// {
			// 	NotifyActivating();
			// 	yield return new ApplyStatusEffectAction<sebloodmark>(args.Selector.SelectedEnemy, Level, 0, 0, 0, 0.2f);
			// }
			yield break;
		}
	}
}