using System.Linq;
using HarmonyLib;
using UnityModManagerNet;

namespace ShuntingStart
{
	public static class Main
	{
		public static bool Load(UnityModManager.ModEntry modEntry)
		{
			var harmony = new Harmony(modEntry.Info.Id);
			harmony.PatchAll();

			return true;
		}
	}
}
