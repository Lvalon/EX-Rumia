using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using lvalonexrumia.Config;
using lvalonexrumia.ImageLoader;
using lvalonexrumia.Localization;

namespace lvalonexrumia.Exhibits
{
    public class lvalonexrumiaExhibitTemplate : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return lvalonexrumiaDefaultConfig.DefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return lvalonexrumiaLocalization.ExhibitsBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return lvalonexrumiaImageLoader.LoadExhibitSprite(exhibit: this);
        }

        public override ExhibitConfig MakeConfig()
        {
            return GetDefaultExhibitConfig();
        }

        public ExhibitConfig GetDefaultExhibitConfig()
        {
            return lvalonexrumiaDefaultConfig.DefaultExhibitConfig();
        }

    }
}