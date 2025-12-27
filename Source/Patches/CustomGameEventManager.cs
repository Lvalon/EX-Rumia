using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.Presentation;

namespace lvalonexrumia.Patches
{
	[HarmonyPatch]
	class CustomGameEventManager
	{
		[HarmonyPatch(typeof(AudioManager), "PlayBossBgm")]
		internal class AudioManager_PlayBossBgm_Patch_exrumia
		{
			private static bool Prefix(AudioManager __instance)
			{
				GameMaster instance = Singleton<GameMaster>.Instance;
				object obj;
				if (instance == null)
				{
					obj = null;
				}
				else
				{
					GameRunController currentGameRun = instance.CurrentGameRun;
					obj = currentGameRun?.CurrentStation;
				}
				Station val = (Station)obj;
				BossStation val2 = (BossStation)(object)((val is BossStation) ? val : null);
				if (val2 != null)
				{
					EnemyGroup enemyGroup = val2.EnemyGroup;
					if ((enemyGroup?.Id) == BepinexPlugin.modUniqueID)
					{
						BepinexPlugin.log.LogInfo("now playing exrumia bgm");
						AudioManager.PlayInLayer1("lvalonexrumiabgm");
						return false;
					}
				}
				return true;
			}
		}
		public static GameEvent<ChangeLifeEventArgs> PreChangeLifeEvent { get; set; }
		public static GameEvent<ChangeLifeEventArgs> PostChangeLifeEvent { get; set; }

		[HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
		private static bool Prefix(GameRunController __instance)
		{
			PreChangeLifeEvent = new GameEvent<ChangeLifeEventArgs>();
			PostChangeLifeEvent = new GameEvent<ChangeLifeEventArgs>();
			return true;
		}
		[HarmonyPatch(typeof(ExhibitWeightTable), nameof(ExhibitWeightTable.WeightFor), typeof(ExhibitConfig)), HarmonyPostfix]
		public static void OverrideWeightFor(ExhibitWeightTable __instance, ExhibitConfig exhibitConfig, ref float __result)
		{
			if (GameMaster.Instance.CurrentGameRun != null)
			{
				if (GameMaster.Instance.CurrentGameRun.Player.Id == nameof(lvalonexrumia) && exhibitConfig.Id == nameof(Duandai))
				{
					__result = 0f;
				}
			}
		}
	}
}