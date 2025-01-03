using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using DV.Booklets;
using DV.Localization.Debug;
using HarmonyLib;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using UnityModManagerNet;

namespace ShuntingStart
{
	[HarmonyPatch(typeof(LicenseManager))]
	[HarmonyPatch("LoadData")]
	public static class LicenseManagerLoadDataPatch
	{
		// Change super method before running
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// Todo: Try to replace base FreightHaul BookletCreator.CreateLicense()
			return instructions;
		}

		// Run patch after super method
		static void Postfix(LicenseManager __instance)
		{
			// Price of FreightHaul is 0 by default
			JobLicenses.FreightHaul.ToV2().price = 1500f;

			// Check if it is a fresh savegame (until first JobLicense purchased)
			var acquiredJobLicenses = __instance.GetAcquiredJobLicenses();
			if (acquiredJobLicenses.Count == 1
			&& acquiredJobLicenses.Contains(JobLicenses.FreightHaul.ToV2()))
			{
				// Change Game Logic tracking
				__instance.RemoveJobLicense(new List<JobLicenseType_v2> { JobLicenses.FreightHaul.ToV2() });
				__instance.AcquireJobLicense(JobLicenses.Shunting.ToV2());

				// Create Booklet page (availalbe in Lost & Found shed)
				BookletCreator.CreateLicense(JobLicenses.Shunting.ToV2(), UnityEngine.Vector3.zero,
					UnityEngine.Quaternion.identity, WorldMover.OriginShiftParent);
			}
		}
	}
}
