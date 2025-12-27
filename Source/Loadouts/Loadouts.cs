using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using lvalonexrumia.Cards;
using lvalonexrumia.Exhibits;
using lvalonexrumia.lvalonexrumiaUlt;
namespace lvalonexrumia
{
	public class lvalonexrumiaLoadouts
	{
		public static string UltimateSkillA = nameof(exulta);
		public static string UltimateSkillB = nameof(exultb);

		public static string ExhibitA = nameof(exexa);
		public static string ExhibitB = nameof(exexb);
		public static List<string> DeckA = new List<string>{
			nameof(Shoot),
			nameof(Shoot),
			nameof(Boundary),
			nameof(Boundary),
			nameof(cardexaa),
			nameof(cardexaa),
			nameof(cardexab),
			nameof(cardexab),
			nameof(cardbite),
			nameof(cardbloodwork),
		};

		public static List<string> DeckB = new List<string>{
			nameof(Shoot),
			nameof(Shoot),
			nameof(Boundary),
			nameof(Boundary),
			nameof(cardexba),
			nameof(cardexba),
			nameof(cardexbb),
			nameof(cardexbb),
			nameof(cardexbb),
			nameof(cardblooduse),
		};

		public static PlayerUnitConfig playerUnitConfig = new PlayerUnitConfig(
			Id: BepinexPlugin.modUniqueID,
			HasHomeName: false,
			ShowOrder: 1000,
			Order: 0,
			UnlockLevel: 0,
			ModleName: "",
			NarrativeColor: "#FF0808",
			IsSelectable: true,
			MaxHp: 100,
			InitialMana: new ManaGroup() { Black = 2, Red = 2 },
			InitialMoney: 50,
			InitialPower: 0,
			BasicRingOrder: null,
			LeftColor: ManaColor.Black,
			RightColor: ManaColor.Red,
			UltimateSkillA: UltimateSkillA,
			UltimateSkillB: UltimateSkillB,
			ExhibitA: ExhibitA,
			ExhibitB: ExhibitB,
			DeckA: DeckA,
			DeckB: DeckB,
			DifficultyA: 3,
			DifficultyB: 3
		);
	}
}
