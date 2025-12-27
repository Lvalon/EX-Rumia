using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using lvalonexrumia.ImageLoader;
using lvalonexrumia.Localization;
using lvalonexrumia.Config;

namespace lvalonexrumia.StatusEffects
{
	public class lvalonexrumiaStatusEffectTemplate : StatusEffectTemplate
	{
		public override IdContainer GetId()
		{
			return lvalonexrumiaDefaultConfig.DefaultID(this);
		}

		public override LocalizationOption LoadLocalization()
		{
			return lvalonexrumiaLocalization.StatusEffectsBatchLoc.AddEntity(this);
		}

		public override Sprite LoadSprite()
		{
			return lvalonexrumiaImageLoader.LoadStatusEffectLoader(status: this);
		}

		public override StatusEffectConfig MakeConfig()
		{
			return GetDefaultStatusEffectConfig();
		}

		public static StatusEffectConfig GetDefaultStatusEffectConfig()
		{
			return lvalonexrumiaDefaultConfig.DefaultStatusEffectConfig();
		}
	}
}