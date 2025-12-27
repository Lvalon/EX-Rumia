using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonexrumia.GunName;
using lvalonexrumia.StatusEffects;
//using lvalonexrumia.BattleActions;

namespace lvalonexrumia.lvalonexrumiaUlt
{
	public sealed class exultaDef : lvalonexrumiaUltTemplate
	{
		public override UltimateSkillConfig MakeConfig()
		{
			UltimateSkillConfig config = GetDefaulUltConfig();
			config.Damage = 10;
			config.Value1 = 1;
			config.Value2 = 3;
			config.RelativeEffects = new List<string>() { nameof(sebloodmark) };
			return config;
		}
	}

	[EntityLogic(typeof(exultaDef))]
	public sealed class exulta : UltimateSkill
	{
		public exulta()
		{
			base.TargetType = TargetType.SingleEnemy;
			base.GunName = GunNameID.GetGunFromId(7021); //盛宴B
		}

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
		{
			yield return PerformAction.Spell(Battle.Player, "exulta");
			EnemyUnit enemy = selector.GetEnemy(base.Battle);
			if (enemy.IsAlive && !Battle.BattleShouldEnd)
			{
				yield return new ApplyStatusEffectAction<sebloodmark>(enemy, base.Value1, 0, 0, 0, 0.2f);
				for (int i = 0; i < base.Value2; i++)
				{
					if (enemy.IsAlive && !Battle.BattleShouldEnd)
					{
						yield return new DamageAction(base.Owner, enemy, this.Damage, base.GunName, GunType.Single);
					}
				}
			}
			yield break;
		}
	}
}
