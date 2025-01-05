using BrilliantSkies.Ftd.Avatar.Movement;
using BrilliantSkies.Ftd.Avatar.Skills;
using BrilliantSkies.Ui.Consoles;
using HarmonyLib;
using System;

namespace VRM_Loader
{
    class VRM_Loader_Patchs
    {
        [HarmonyPatch(typeof(AvatarPartVisibility), "SetState", new Type[] { typeof(enumCameraState) })]
        private class AvatarPartVisibility_SetState_Patch
        {
            private static bool Prefix(AvatarPartVisibility __instance, enumCameraState cameraState)
            {
                if (cameraState == enumCameraState.firstPerson)
                {
                    CameraManager.GetSingleton().CameraInternalised();
                }
                else
                {
                    CameraManager.GetSingleton().CameraExternalised();
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(CharacterSheetUi), "BuildInterface", new Type[] { typeof(string) })]
        class ConstructableInfoUiPatch
        {
            static void Postfix(CharacterSheetUi __instance, ref ConsoleWindow __result)
            {
                CharacterSheet _focus = Traverse.Create(__instance).Field("_focus").GetValue<CharacterSheet>();
                __result.AllScreens.Add(new VRM_SelectTab(__result, _focus));
            }
        }
    }
}
