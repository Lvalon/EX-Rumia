using HarmonyLib;

namespace lvalonexrumia
{
	public static class PInfo
	{
		// each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
		public const string GUID = "llbol.char.exrumia";
		public const string Name = "EX Rumia";
		public const string version = "1.0.6";
		public static readonly Harmony harmony = new Harmony(GUID);

	}
}
