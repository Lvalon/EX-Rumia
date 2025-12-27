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
using LBoL.Core.StatusEffects;
using lvalonexrumia.StatusEffects;

namespace lvalonexrumia.lvalonexrumiaUlt
{
	public sealed class exultbDef : lvalonexrumiaUltTemplate
	{
		public override UltimateSkillConfig MakeConfig()
		{
			UltimateSkillConfig config = GetDefaulUltConfig();
			config.Damage = 10;
			config.Value1 = 1;
			config.Value2 = 2;

			// Add the relative status effects in the description box.   
			config.RelativeEffects = new List<string>() { nameof(sebleed) };
			return config;
		}
	}

	[EntityLogic(typeof(exultbDef))]
	public sealed class exultb : UltimateSkill
	{
		public exultb()
		{
			base.TargetType = TargetType.AllEnemies;
			base.GunName = GunNameID.GetGunFromId(7071); //鬼气狂澜B
		}

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
		{
			yield return PerformAction.Spell(Battle.Player, "exultb");
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				yield return new ApplyStatusEffectAction<sebleed>(unit, 1, 0, 0, 0, 0.2f);
			}
			if (Battle.BattleShouldEnd) { yield break; }
			for (int i = 0; i < base.Value2; i++)
			{
				yield return new DamageAction(base.Owner, Battle.AllAliveEnemies, this.Damage, base.GunName, GunType.Single);
			}
		}
	}
}
