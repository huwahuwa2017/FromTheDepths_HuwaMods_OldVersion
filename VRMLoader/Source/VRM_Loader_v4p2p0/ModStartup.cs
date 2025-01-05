using BrilliantSkies.Core.Timing;
using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using VRM_Loader;

namespace ModManagement
{
    public static class ModStartup
    {
        private static bool _start = false;

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /*
        public static void OnLoad()
        {
        }
        */

        public static void OnStart()
        {
            if (_start)
                return;

            _start = true;

            ModInformation.VersionConfirmation();

            Harmony harmony = new Harmony("VRM_Loader_Patchs");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            AllocConsole();
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

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
