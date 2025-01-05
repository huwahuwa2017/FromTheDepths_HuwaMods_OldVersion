using BrilliantSkies.Ftd.Avatar;
using BrilliantSkies.Ftd.Avatar.Movement;
using BrilliantSkies.Ftd.Avatar.Skills;
using BrilliantSkies.Ui.Consoles;
using HarmonyLib;
using System;

namespace VRM_Loader
{
    internal class VRM_Loader_Patchs
    {
        [HarmonyPatch(typeof(cCameraControl), "OnAvatarSpawned", new Type[] { typeof(ModelSpawnData) })]
        private class cCameraControl_OnAvatarSpawned_Patch
        {
            private static void Postfix(cCameraControl __instance, ModelSpawnData data)
            {
                CharacterReplacement.SetModelSpawnData(data);
                CharacterReplacement.DefaultAvatarSetActive();
                CharacterReplacement.ModelUpdate();
            }
        }

        [HarmonyPatch(typeof(CharacterSheetUi), "BuildInterface", new Type[] { typeof(string) })]
        private class ConstructableInfoUiPatch
        {
            private static void Postfix(CharacterSheetUi __instance, ref ConsoleWindow __result)
            {
                CharacterSheet _focus = Traverse.Create(__instance).Field("_focus").GetValue<CharacterSheet>();
                __result.AllScreens.Add(new VRM_SelectTab(__result, _focus));
            }
        }
    }
}
