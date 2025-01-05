using BrilliantSkies.Ftd.Constructs.UI;
using BrilliantSkies.Ui.Consoles;
using HarmonyLib;
using System;

namespace CountBlocks
{
    class CountBlocksPatch
    {
        [HarmonyPatch(typeof(ConstructableInfoUi), "BuildInterface", new Type[] { typeof(string) })]
        class ConstructableInfoUiPatch
        {
            static void Postfix(ConstructableInfoUi __instance, ref ConsoleWindow __result)
            {
                ConstructInfo _focus = Traverse.Create(__instance).Field("_focus").GetValue<ConstructInfo>();
                __result.AllScreens.Add(new CountBlocksTab(__result, _focus));
            }
        }
    }
}
