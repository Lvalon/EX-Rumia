using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebewaterDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			return config;
		}
	}

	[EntityLogic(typeof(sebewaterDef))]
	public sealed class sebewater : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			if (Battle.BattleShouldEnd || (!(args.Card is cardredblood) && !(args.Card is carddarkblood)))
			{
				yield break;
			}
			NotifyActivating();
			if (Battle.DiscardZone.Count > 0)
			{
				SelectCardInteraction interaction = new SelectCardInteraction(0, Level, Battle.DiscardZone)
				{
					Source = this
				};
				yield return new InteractionAction(interaction);
				IReadOnlyList<Card> cards = interaction.SelectedCards;
				if (cards.Count > 0)
				{
					foreach (Card card in cards)
					{
						yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Random);
						//card.Zone = CardZone.Draw;
						//yield return new MoveCardAction(card, CardZone.Draw);
					}
				}
			}
			yield return new DrawManyCardAction(Level);
			yield break;
		}
	}
}