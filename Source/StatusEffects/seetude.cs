using System.Collections.Generic;
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
using lvalonexrumia.GunName;
using lvalonexrumia.Patches;

namespace lvalonexrumia.StatusEffects
{
	public sealed class seetudeDef : lvalonexrumiaStatusEffectTemplate
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

	[EntityLogic(typeof(seetudeDef))]
	public sealed class seetude : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
		public int heal
		{
			get
			{
				if (Owner == null)
				{
					return 0;
				}
				if (Level == 0)
				{
					return 0;
				}
				return toolbox.hpfrompercent(GameMaster.Instance.CurrentGameRun.Player, 5, false);
			}
		}
		public DamageInfo Damage => DamageInfo.Attack(Level, isAccuracy: true);

		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Owner.TurnStarted, OnOwnerTurnStarted);
		}

		private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
		{
			if (Battle.BattleShouldEnd)
			{
				yield break;
			}
			NotifyActivating();
			yield return new ChangeLifeAction(-heal);
			yield return new ApplyStatusEffectAction<seatkincrease>(Battle.Player, 30, 0, 0, 0, 0.2f);
			for (int i = 0; i < 3; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new DamageAction(Battle.Player, Battle.RandomAliveEnemy, Damage, GunNameID.GetGunFromId(7041), GunType.Single);
			}
			if (Battle.BattleShouldEnd) { yield break; }
			yield return new RemoveStatusEffectAction(this);
		}
	}
}