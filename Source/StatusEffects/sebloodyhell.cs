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
	public sealed class sebloodyhellDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(seatkincrease) };
			return config;
		}
	}

	[EntityLogic(typeof(sebloodyhellDef))]
	public sealed class sebloodyhell : StatusEffect
	{
		public int atkincrease
		{
			get
			{
				if (Owner == null)
				{
					return 0;
				}
				return Level * 5;
			}
		}
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			//if (args.Card.Config.Colors.Contains(ManaColor.Red) && args.Card.Config.Type == CardType.Attack)
			if (args.Card.Config.Colors.Contains(ManaColor.Red))
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, Level * 5, 0, 0, 0, 0.2f);
			}
		}
	}
}