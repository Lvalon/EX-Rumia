using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using lvalonexrumia.ImageLoader;
using lvalonexrumia.Localization;
using lvalonexrumia.Config;

namespace lvalonexrumia.lvalonexrumiaUlt
{
    public class lvalonexrumiaUltTemplate : UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return lvalonexrumiaDefaultConfig.DefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return lvalonexrumiaLocalization.UltimateSkillsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return lvalonexrumiaImageLoader.LoadUltLoader(ult: this);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            throw new System.NotImplementedException();
        }

        public UltimateSkillConfig GetDefaulUltConfig()
        {
            return lvalonexrumiaDefaultConfig.DefaultUltConfig();
        }
    }
}