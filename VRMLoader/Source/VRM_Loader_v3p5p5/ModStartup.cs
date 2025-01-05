using BrilliantSkies.Core.Timing;
using HarmonyLib;
using System.Reflection;
using VRM_Loader;

namespace ModManagement
{
    public static class ModStartup
    {
        /*
        public static void OnLoad()
        {
        }
        */

        public static void OnStart()
        {
            ModInformation.VersionConfirmation();

            CharacterReplacement.Start();
            GameEvents.UpdateEvent.RegWithEvent(CharacterReplacement.Update);

            Harmony harmony = new Harmony("VRM_Loader_Patchs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        /*
        public static void OnSave()
        {
        }
        */
    }
}
