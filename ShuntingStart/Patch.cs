using System.Collections.Generic;
using System.Reflection.Emit;
using DV;
using System.Text;
using DV.Booklets;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using DV.Utils;
using HarmonyLib;
using UnityEngine;
using static DV.UI.ATutorialsMenuProvider;
using System;

namespace ShuntingStart
{
    [HarmonyPatch(typeof(LicenseManager), "LoadData")]
    public static class LicenseManagerLoadDataPatch
    {
        private static Boolean isFreightHaulAlreadyAcquired = true;

        [HarmonyPrefix]
        static bool Prefix()
        {

            JobLicenseType_v2 jobLicenseType_v = JobLicenses.FreightHaul.ToV2();
            if (LicenseManager.Instance.IsJobLicenseAcquired(jobLicenseType_v) == false)
            {
                LicenseManager.Instance.AcquireJobLicense(jobLicenseType_v);
                isFreightHaulAlreadyAcquired = false;
                Debug.Log("ShuntingStart: FreightHaul license not purchased, temporarily adding for bypassing vanilla-check");
            }

            return true; // Run original method as intended
        }

        [HarmonyPostfix]
        static void Postfix()
        {
            if (isFreightHaulAlreadyAcquired)
            {
                Debug.Log("ShuntingStart: Skipping Shunting license purchase");
                return;
            }

            JobLicenseType_v2 jobLicenseType_v = JobLicenses.Shunting.ToV2();
            if(!LicenseManager.Instance.IsJobLicenseAcquired(jobLicenseType_v))
            {
                LicenseManager.Instance.AcquireJobLicense(jobLicenseType_v);
                Debug.Log("ShuntingStart: Shunting license acquired");
            }

            jobLicenseType_v = JobLicenses.FreightHaul.ToV2();
            if (LicenseManager.Instance.IsJobLicenseAcquired(jobLicenseType_v))
            {
                LicenseManager.Instance.RemoveJobLicense(new List<JobLicenseType_v2> { jobLicenseType_v });
                Debug.Log("ShuntingStart: FreightHaul license cleared after vanilla-check");
            }
            else
            {
                Debug.LogError("What happened?! FreightHaul license not cleared after vanilla-check");
            }
        }
    }


    [HarmonyPatch(typeof(StartGameData_NewCareer))]
    public static class StartGameDataNewCareerPatch
    {
        [HarmonyPatch("Initialize")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            /*
            //          JobLicenseType_v1 are enum values
            //          Replace IL-code
            // IL_0211: ldc.i4    512
            //          with
            // IL_0112: ldc.i4    1024
            */

            var codes = new List<CodeInstruction>(instructions);
            int replacements = 0;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_I4 && (int)codes[i].operand == 512)
                {
                    codes[i].operand = 1024;
                    replacements++;
                    Debug.Log($"ShuntingStart: Replaced FreightHaul(512) with Shunting(1024) at index {i}");
                }
            }

            return codes;
            Debug.Log($"ShuntingStart: Made {replacements} replacements in StartGameDataNewCareerPatch");
        }
    }
}
