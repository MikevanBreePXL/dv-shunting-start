using System.Linq;
using HarmonyLib;
using UnityModManagerNet;

namespace ShuntingStart
{
	public static class Main
	{
		public static bool Load(UnityModManager.ModEntry modEntry)
		{
			modEntry.OnToggle = OnToggle;

			return true;
		}
		
		private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
		{
			var harmony = new Harmony(modEntry.Info.Id);
			if (value)
			{
				harmony.PatchAll();
			}
			else
			{
				harmony.UnpatchAll();
			}

			return true;
		}
	}
}
