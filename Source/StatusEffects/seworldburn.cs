using System;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using lvalonexrumia.Patches;
using lvalonmeme.StatusEffects;

namespace lvalonexrumia.StatusEffects
{
	public sealed class seworldburnDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = new List<string>() { nameof(sebleed), nameof(sebloodmark) };
			return config;
		}
	}

	[EntityLogic(typeof(seworldburnDef))]
	public sealed class seworldburn : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			foreach (EnemyUnit allAliveEnemy in base.Battle.AllAliveEnemies)
			{
				ReactOwnerEvent(allAliveEnemy.StatusEffectAdded, OnEnemyStatusEffectAdded);
			}

			HandleOwnerEvent(base.Battle.EnemySpawned, OnEnemySpawned);
		}

		private IEnumerable<BattleAction> OnEnemyStatusEffectAdded(StatusEffectApplyEventArgs args)
		{
			if (args.ActionSource != this)
			{
				if (args.Effect is sebloodmark)
				{
					NotifyActivating();
					yield return new ApplyStatusEffectAction<sebleed>(args.Unit, Level, 0, 0, 0, 0.2f);
				}
				if (args.Effect is sebleed)
				{
					NotifyActivating();
					yield return new ApplyStatusEffectAction<sebloodmark>(args.Unit, Level, 0, 0, 0, 0.2f);
				}
			}
			yield break;
		}

		private void OnEnemySpawned(UnitEventArgs args)
		{
			ReactOwnerEvent(args.Unit.StatusEffectAdded, OnEnemyStatusEffectAdded);
		}
	}
}