using System;
using System.Collections;
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
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.Patches;
using lvalonexrumia.StatusEffects;
using lvalonmeme.StatusEffects;

namespace lvalonexrumia.Exhibits
{
	public sealed class exrumiafumoDef : lvalonexrumiaExhibitTemplate
	{
		public override ExhibitConfig MakeConfig()
		{
			ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
			exhibitConfig.Owner = null;
			exhibitConfig.IsPooled = true;
			exhibitConfig.BaseManaColor = null;
			exhibitConfig.Revealable = false;

			exhibitConfig.LosableType = ExhibitLosableType.Losable;
			exhibitConfig.Rarity = Rarity.Uncommon;
			exhibitConfig.Appearance = AppearanceType.Anywhere;
			exhibitConfig.RelativeCards = new List<string>() { nameof(cardredblood), nameof(carddarkblood) };
			exhibitConfig.Value1 = 1; // tmp Firepower
			return exhibitConfig;
		}
	}

	[EntityLogic(typeof(exrumiafumoDef))]
	[ExhibitInfo(WeighterType = typeof(exrumiafumoWeighter))]
	public sealed class exrumiafumo : Exhibit
	{
		public class exrumiafumoWeighter : IExhibitWeighter
		{
			public float WeightFor(Type type, GameRunController gameRun)
			{
				return gameRun.BaseDeck.Any((Card card) => card.Config.RelativeCards.Contains(nameof(cardredblood)) || card.Config.RelativeCards.Contains(nameof(carddarkblood)) || card.Config.UpgradedRelativeCards.Contains(nameof(cardredblood)) || card.Config.UpgradedRelativeCards.Contains(nameof(carddarkblood)) || card.Config.RelativeEffects.Contains(nameof(sedarkblood)) || card.Config.UpgradedRelativeEffects.Contains(nameof(sedarkblood))) ? 1 : 0;
			}
		}
		protected override void OnEnterBattle()
		{
			ReactBattleEvent(Battle.CardUsed, OnCardUsed);
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			if (args.Card.Id == nameof(cardredblood) || args.Card.Id == nameof(carddarkblood))
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<TempFirepower>(Battle.Player, Value1, 0, 0, 0, 0.2f);
			}
		}
	}
}