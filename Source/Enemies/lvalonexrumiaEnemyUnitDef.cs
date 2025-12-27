using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using lvalonexrumia.Enemies.Template;
using lvalonexrumia.GunName;


namespace lvalonexrumia.Enemies
{
	public sealed class lvalonexrumiaEnemyUnitDef : lvalonexrumiaEnemyUnitTemplate
	{
		public override IdContainer GetId() => nameof(lvalonexrumia);

		public override EnemyUnitConfig MakeConfig()
		{
			EnemyUnitConfig config = GetEnemyUnitDefaultConfig();
			//Whether the boss should be enabled.
			config.IsPreludeOpponent = BepinexPlugin.enableAct1Boss.Value;
			//config.IsPreludeOpponent = false;

			//Color(s) of the exhibits the boss can drop (right-most exhibit).
			config.BaseManaColor = new List<ManaColor>() { ManaColor.Black, ManaColor.Red };

			config.Type = EnemyType.Boss;

			//Boss properties
			config.MaxHp = 280;
			config.MaxHpHard = 290;
			config.MaxHpLunatic = 300;

			config.Damage1 = 6;
			config.Damage1Hard = 7;
			config.Damage1Lunatic = 8;

			config.Damage2 = 9;
			config.Damage2Hard = 10;
			config.Damage2Lunatic = 11;

			config.Damage3 = 1;
			config.Damage3Hard = 1;
			config.Damage3Lunatic = 1;

			config.Damage4 = 2;
			config.Damage4Hard = 2;
			config.Damage4Lunatic = 3;

			config.Defend = 5;
			config.DefendHard = 6;
			config.DefendLunatic = 7;

			config.Count1 = 2;
			config.Count1Hard = 2;
			config.Count1Lunatic = 3;

			config.Count2 = 1;
			config.Count2Hard = 1;
			config.Count2Lunatic = 2;

			config.PowerLoot = new MinMax(100, 100);
			config.BluePointLoot = new MinMax(100, 100);

			config.Gun1 = new List<string> { GunNameID.GetGunFromId(7021) }; //ultb
			config.Gun2 = new List<string> { GunNameID.GetGunFromId(7071) }; //ulta
			config.Gun3 = new List<string> { GunNameID.GetGunFromId(520) }; //tenshade
			config.Gun4 = new List<string> { GunNameID.GetGunFromId(7161) }; //shadowcut

			return config;
		}
	}
}
