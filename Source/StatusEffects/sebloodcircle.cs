using System;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebloodcircleDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			return config;
		}
	}

	[EntityLogic(typeof(sebloodcircleDef))]
	public sealed class sebloodcircle : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Owner.TurnStarted, OnOwnerTurnStarted);
			// ReactOwnerEvent(Owner.DamageDealt, OnDamageDealt);
		}

		private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
		{
			List<Card> list = Battle.ExileZone.Where(delegate (Card card)
			{
				return card is carddarkblood || card is cardredblood;
			}).ToList();
			if (Battle.BattleShouldEnd || list.Count == 0)
			{
				yield break;
			}
			NotifyActivating();
			SelectCardInteraction interaction = new SelectCardInteraction(0, list.Count, list)
			{
				Source = this
			};
			yield return new InteractionAction(interaction);
			IReadOnlyList<Card> cards = interaction.SelectedCards;
			if (cards.Count > 0)
			{
				foreach (Card card in cards)
				{
					if (!Battle.HandIsFull)
					{
						yield return new MoveCardAction(card, CardZone.Hand);
					}
				}
			}
			yield break;
		}

		// private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
		// {
		// 	if (!Battle.BattleShouldEnd && args.Target.IsAlive)
		// 	{
		// 		DamageInfo damageInfo = args.DamageInfo;
		// 		if (damageInfo.DamageType == DamageType.Attack)
		// 		{
		// 			if (!args.Target.HasStatusEffect<sebloodmark>())
		// 			{
		// 				NotifyActivating();
		// 				yield return new ApplyStatusEffectAction<sebleed>(args.Target, Level, 0, 0, 0, 0.2f);
		// 			}
		// 			if (args.Target.HasStatusEffect<sebleed>() && !args.Target.HasStatusEffect<sebloodmark>())
		// 			{
		// 				NotifyActivating();
		// 				yield return new ApplyStatusEffectAction<sebloodmark>(args.Target, Level, 0, 0, 0, 0.2f);
		// 			}
		// 		}
		// 	}
		// }
	}
}