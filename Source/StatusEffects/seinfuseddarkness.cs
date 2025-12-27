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
	public sealed class seinfuseddarknessDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			return config;
		}
	}

	[EntityLogic(typeof(seinfuseddarknessDef))]
	public sealed class seinfuseddarkness : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
		public ManaGroup Mana
		{
			get
			{
				return new ManaGroup() { Philosophy = 1 };
			}
		}
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
				NotifyActivating();
				yield return new GainManaAction(Mana);
			}
		}
	}
}