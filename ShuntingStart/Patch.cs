using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace ShuntingStart
{
    [HarmonyPatch(typeof(LicenseManager))]
    [HarmonyPatch("LoadData")]
    public static class LicenseManagerLoadDataPatch
    {
        // Change original call from FreightHaul to Shunting
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            /*
            // Replace IL-code
            //   IL_0112: ldc.i4    512
            // with
            //   IL_0112: ldc.i4    1024

				JobLicenseType_v1 are enum values
			*/

            var codes = new List<CodeInstruction>(instructions);

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_I4 && (int)codes[i].operand == 512)
                {
                    codes[i].operand = 1024;
                    break;
                }
            }

            return instructions;
        }

    }
}
