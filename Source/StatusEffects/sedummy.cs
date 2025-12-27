using LBoLEntitySideloader.Attributes;
using LBoL.Core.StatusEffects;
using UnityEngine;
using lvalonexrumia.StatusEffects;
using LBoL.ConfigData;
using System.Collections.Generic;

namespace lvalonmeme.StatusEffects
{
	public sealed class sedecreaseDef : lvalonexrumiaStatusEffectTemplate
	{
		public override Sprite LoadSprite() => null;
	}

	[EntityLogic(typeof(sedecreaseDef))]
	public sealed class sedecrease : StatusEffect
	{
	}
	public sealed class seincreaseDef : lvalonexrumiaStatusEffectTemplate
	{
		public override Sprite LoadSprite() => null;
	}

	[EntityLogic(typeof(seincreaseDef))]
	public sealed class seincrease : StatusEffect
	{
	}
	public sealed class sebosshealDef : lvalonexrumiaStatusEffectTemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			//fp=4, burst=20, lockon=7, camo=9
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.RelativeEffects = new List<string>() { nameof(seincrease) };
			return config;
		}
	}

	[EntityLogic(typeof(sebosshealDef))]
	public sealed class sebossheal : StatusEffect
	{
		public override bool ForceNotShowDownText => true;
	}
}

