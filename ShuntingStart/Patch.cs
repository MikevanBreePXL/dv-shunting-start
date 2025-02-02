﻿using System.Collections.Generic;
using System.Reflection.Emit;
using DV.ThingTypes;
using DV.ThingTypes.TransitionHelpers;
using HarmonyLib;
using UnityEngine;

namespace ShuntingStart
{
    [HarmonyPatch(typeof(LicenseManager), "LoadData")]
    public static class LicenseManagerLoadDataPatch
    {
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

            Debug.Log($"ShuntingStart: Made {replacements} replacements in LicenseManagerLoadDataPatch");
            return codes;
        }

        [HarmonyPostfix]
        static void Postfix(LicenseManager __instance)
        {
            JobLicenses.FreightHaul.ToV2().price = 10000f; // 10k
        }
    }


    [HarmonyPatch(typeof(StartGameData_NewCareer))]
    public static class StartGameDataNewCareerPatch
    {
        [HarmonyPatch("PrepareNewSaveData")]
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
