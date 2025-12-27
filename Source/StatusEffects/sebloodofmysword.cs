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
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class sebloodofmyswordDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sebloodsword) };
			return config;
		}
	}

	[EntityLogic(typeof(sebloodofmyswordDef))]
	public sealed class sebloodofmysword : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			//ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
			ReactOwnerEvent(Battle.Player.DamageDealt, OnDamageDealt);
		}

		private IEnumerable<BattleAction> OnDamageDealt(DamageEventArgs args)
		{
			if (args.Target.IsNotAlive || args.Target.Hp == 0 || args.DamageInfo.DamageType != DamageType.Attack) { yield break; }
			yield return new ChangeLifeAction(Level, args.Target);
			//yield return new ApplyStatusEffectAction<TempFirepowerNegative>(args.Target, Level, 0, 0, 0, 0.2f);
			yield return new ApplyStatusEffectAction<TempFirepower>(Owner, Level, 0, 0, 0, 0.2f);
		}

		// private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		// {
		// 	if (Battle.BattleShouldEnd) { yield break; }
		// 	if (args.Card.Id != nameof(cardbloodsword) && args.Card.Config.Type == CardType.Attack)
		// 	{
		// 		NotifyActivating();
		// 		yield return new ApplyStatusEffectAction<sebloodsword>(Battle.Player, Level, 0, 0, 0, 0.2f);
		// 	}
		// }
	}
}