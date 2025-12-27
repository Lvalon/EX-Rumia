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
	public sealed class extenshadedDef : lvalonexrumiaUltTemplate
	{
		public override UltimateSkillConfig MakeConfig()
		{
			UltimateSkillConfig config = GetDefaulUltConfig();
			return config;
		}
	}

	[EntityLogic(typeof(extenshadedDef))]
	public sealed class extenshaded : UltimateSkill
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
		{
			yield break;
		}
	}
	public sealed class exshadowcutDef : lvalonexrumiaUltTemplate
	{
		public override UltimateSkillConfig MakeConfig()
		{
			UltimateSkillConfig config = GetDefaulUltConfig();
			return config;
		}
	}

	[EntityLogic(typeof(exshadowcutDef))]
	public sealed class exshadowcut : UltimateSkill
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
		{
			yield break;
		}
	}
	public sealed class exgojoDef : lvalonexrumiaUltTemplate
	{
		public override UltimateSkillConfig MakeConfig()
		{
			UltimateSkillConfig config = GetDefaulUltConfig();
			return config;
		}
	}
	[EntityLogic(typeof(exgojoDef))]
	public sealed class exgojo : UltimateSkill
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
		{
			yield break;
		}
	}
}
