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
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Cards;
using lvalonexrumia.GunName;

namespace lvalonexrumia.StatusEffects
{
	public sealed class seshadowcutDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.IsStackable = false;
			config.RelativeEffects = new List<string>() { nameof(ExtraTurn) };
			return config;
		}
	}

	[EntityLogic(typeof(seshadowcutDef))]
	public sealed class seshadowcut : ExtraTurnPartner
	{
		public DamageInfo Damage => DamageInfo.Attack(Level, isAccuracy: true);
		protected override void OnAdded(Unit unit)
		{
			if (!(unit is PlayerUnit))
			{
				BepinexPlugin.log.LogWarning(DebugName + " should not apply to non-player unit.");
				React(new RemoveStatusEffectAction(this));
				return;
			}
			base.ThisTurnActivating = false;
			base.ShowCount = false;
			HandleOwnerEvent(base.Battle.Player.TurnStarting, delegate
			{
				if (base.Battle.Player.IsExtraTurn && !base.Battle.Player.IsSuperExtraTurn && base.Battle.Player.GetStatusEffectExtend<ExtraTurnPartner>() == this)
				{
					base.ThisTurnActivating = true;
					base.ShowCount = true;
					base.Highlight = true;
				}
			});
			ReactOwnerEvent(Battle.Player.TurnStarted, OnPlayerTurnStarted);
			ReactOwnerEvent(base.Battle.Player.TurnEnded, OnPlayerTurnEnded);
		}

		private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
		{
			if (base.ThisTurnActivating)
			{
				for (int i = 0; i < Level; i++)
				{
					if (Battle.BattleShouldEnd) { yield break; }
					if (i == 0)
					{
						yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, GunNameID.GetGunFromId(7161), GunType.Single);
					}
					else
					{
						yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, GunNameID.GetGunFromId(520), GunType.Single);
					}
				}
				yield return new RemoveStatusEffectAction(this);
			}
		}

		private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
		{
			if (base.ThisTurnActivating)
			{
				yield return new RemoveStatusEffectAction(this);
			}
		}
	}
}