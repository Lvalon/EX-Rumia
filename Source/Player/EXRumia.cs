using Cysharp.Threading.Tasks;
//using DG.Tweening;
using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using lvalonexrumia.ImageLoader;
using lvalonexrumia.Localization;
using LBoL.Core.Battle;
using System.Collections.Generic;
using LBoL.Core;
using System;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.Core.Battle.BattleActions;
//using lvalonexrumia.BattleActions;

namespace lvalonexrumia
{
	public sealed class lvalonexrumiaDef : PlayerUnitTemplate
	{
		public UniTask<Sprite>? LoadSpellPortraitAsync { get; private set; }

		public override IdContainer GetId()
		{
			return BepinexPlugin.modUniqueID;
		}

		public override LocalizationOption LoadLocalization()
		{
			return lvalonexrumiaLocalization.PlayerUnitBatchLoc.AddEntity(this);
		}

		public override PlayerImages LoadPlayerImages()
		{
			return lvalonexrumiaImageLoader.LoadPlayerImages(BepinexPlugin.playerName);
		}
		public override EikiSummonInfo AssociateEikiSummon()
		{
			return new EikiSummonInfo(typeof(Enemies.lvalonexrumia));
		}

		public override PlayerUnitConfig MakeConfig()
		{
			return lvalonexrumiaLoadouts.playerUnitConfig;
		}

		[EntityLogic(typeof(lvalonexrumiaDef))]
		public sealed class lvalonexrumia : PlayerUnit
		{
		}
	}
}