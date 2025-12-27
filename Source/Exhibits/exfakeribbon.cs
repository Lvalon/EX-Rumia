using System;
using System.Collections;
using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;

namespace lvalonexrumia.Exhibits
{
	public sealed class exfakeribbonDef : lvalonexrumiaExhibitTemplate
	{
		public override ExhibitConfig MakeConfig()
		{
			ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
			exhibitConfig.Owner = null;
			exhibitConfig.IsPooled = true;
			exhibitConfig.BaseManaColor = null;
			exhibitConfig.Revealable = false;

			// exhibitConfig.Order = 1;
			// exhibitConfig.Value1 = 2;
			// exhibitConfig.Value2 = 10;

			exhibitConfig.LosableType = ExhibitLosableType.Losable;
			exhibitConfig.Rarity = Rarity.Rare;
			exhibitConfig.Appearance = AppearanceType.ShopOnly;
			// exhibitConfig.Mana = new ManaGroup() { Philosophy = 1 };
			exhibitConfig.Mana = new ManaGroup() { Red = 1 };
			return exhibitConfig;
		}
	}

	[EntityLogic(typeof(exfakeribbonDef))]
	[ExhibitInfo(WeighterType = typeof(exfakeribbonWeighter))]
	public sealed class exfakeribbon : Exhibit
	{
		public class exfakeribbonWeighter : IExhibitWeighter
		{
			public float WeightFor(Type type, GameRunController gameRun)
			{
				return gameRun.Player.Id == nameof(lvalonexrumia) ? 1 : 0;
			}
		}
		public ManaGroup ManaFake = new ManaGroup() { Red = 1 };
		protected override IEnumerator SpecialGain(PlayerUnit player)
		{
			this.OnGain(player);
			yield return GameMaster.Instance.CurrentGameRun.GainExhibitRunner(Library.CreateExhibit(nameof(extrueribbon)), true);
			yield break;
		}
		protected override void OnGain(PlayerUnit player)
		{
			GameMaster.Instance.CurrentGameRun.LoseExhibit(this, false, true);
		}
	}
}