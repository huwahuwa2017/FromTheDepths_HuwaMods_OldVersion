using BrilliantSkies.Core.Timing;
using HarmonyLib;
using System.Reflection;
using VRM_Loader;

namespace ModManagement
{
    public static class ModStartup
    {
        //[DllImport("kernel32.dll")]
        //private static extern bool AllocConsole();

        /*
        public static void OnLoad()
        {
        }
        */

        public static void OnStart()
        {
            ModInformation.VersionConfirmation();

            Harmony harmony = new Harmony("VRM_Loader_Patchs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //AllocConsole();
            //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            CharacterReplacement.Start();
            GameEvents.UpdateEvent.RegWithEvent(CharacterReplacement.Update);
        }

        /*
        public static void OnSave()
        {
        }
        */
    }
}
