using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using lvalonexrumia.Enemies.Template;


namespace lvalonexrumia.Enemies
{
	public sealed class lvalonexrumiaEnemyGroupDef : lvalonexrumiaEnemyGroupTemplate
	{
		public override IdContainer GetId() => nameof(lvalonexrumia);

		public override EnemyGroupConfig MakeConfig()
		{
			EnemyGroupConfig config = GetEnemyGroupDefaultConfig();
			config.Name = nameof(lvalonexrumia);
			config.FormationName = VanillaFormations.Single;
			config.Enemies = new List<string>() { nameof(lvalonexrumia) };
			config.EnemyType = EnemyType.Boss;
			config.RollBossExhibit = true;

			return config;
		}
	}
}