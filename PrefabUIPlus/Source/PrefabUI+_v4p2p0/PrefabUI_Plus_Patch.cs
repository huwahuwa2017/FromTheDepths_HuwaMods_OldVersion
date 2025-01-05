using Assets.Scripts.Gui;
using Assets.Scripts.Persistence;
using BrilliantSkies.Core.Help;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd;
using BrilliantSkies.Ftd.Avatar.Build;
using BrilliantSkies.Ftd.Avatar.HUD;
using BrilliantSkies.Localisation.Runtime.FileManagers.Files;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Elements;
using BrilliantSkies.Ui.HudSystem;
using BrilliantSkies.Ui.Special.PopUps;
using HarmonyLib;
using System;
using UnityEngine;

namespace PrefabUIPlus
{
    internal static class PrefabUI_Plus_Patch
    {
        [HarmonyPatch(typeof(HudBuildCommands), "PrefabAndSubObjectOptions", new Type[] { typeof(BuildInfo) })]
        private static class HudBuildCommands_PrefabAndSubObjectOptions_Patch
        {
            private static bool Prefix(HudBuildCommands __instance, BuildInfo info)
            {
                void DisplayMessage() => Traverse.Create(__instance).Method("DisplayMessage").GetValue();
                void DisplayPrefabOptions(SavedSubObject prefab, bool displayCapture) => Traverse.Create(__instance).Method("DisplayPrefabOptions", prefab, displayCapture).GetValue();

                UiComposite<int> _widthBar = Traverse.Create(__instance).Field("_widthBar").GetValue<UiComposite<int>>();
                UiComposite<int> _heightBar = Traverse.Create(__instance).Field("_heightBar").GetValue<UiComposite<int>>();
                UiComposite<int> _lengthBar = Traverse.Create(__instance).Field("_lengthBar").GetValue<UiComposite<int>>();



                bool flag = Input.GetKey(KeyCode.Tab);
                if (flag) GUI.FocusControl("");

                int width = Screen.width;
                int height = Screen.height;
                Rect screenRect = new Rect(width * 0.39f, height * 0.7f, width * 0.22f, height * 0.3f);
                cBuild singleton = cBuild.GetSingleton();
                if (info.AddRemove == enumAddRemove.getprefab)
                {
                    SavedSubObject prefab = singleton.BuildingWith.Prefab;
                    if (prefab != null)
                    {
                        Vector3i dimensions = prefab.Dimensions;
                        Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        FtdUiDefs.PrefabWidthRemove.BackgroundTint = ((dimensions.x == 1) ? color : Color.white);
                        FtdUiDefs.PrefabHeightRemove.BackgroundTint = ((dimensions.y == 1) ? color : Color.white);
                        FtdUiDefs.PrefabLengthRemove.BackgroundTint = ((dimensions.z == 1) ? color : Color.white);
                        int num = (int)ConsoleStyles.Instance.HudKeyDimension;
                        FtdUiDefs.PrefabWidthRemove.HeightOverride = num;
                        FtdUiDefs.PrefabWidthRemove.WidthOverride = num;
                        FtdUiDefs.PrefabWidthAdd.HeightOverride = num;
                        FtdUiDefs.PrefabWidthAdd.WidthOverride = num;
                        FtdUiDefs.PrefabHeightRemove.HeightOverride = num;
                        FtdUiDefs.PrefabHeightRemove.WidthOverride = num;
                        FtdUiDefs.PrefabHeightAdd.HeightOverride = num;
                        FtdUiDefs.PrefabHeightAdd.WidthOverride = num;
                        FtdUiDefs.PrefabLengthRemove.HeightOverride = num;
                        FtdUiDefs.PrefabLengthRemove.WidthOverride = num;
                        FtdUiDefs.PrefabLengthAdd.HeightOverride = num;
                        FtdUiDefs.PrefabLengthAdd.WidthOverride = num;
                        GUILayout.BeginArea(screenRect);
                        GUILayout.BeginVertical(ConsoleStyles.Instance.Styles.Windows.ConsoleWindow.Style);
                        DisplayMessage();

                        GUILayout.BeginHorizontal();
                        _widthBar.Draw(prefab.Dimensions.x);
                        InputType inputType = FtdUiDefs.PrefabWidthRemove.DisplayButton();
                        dimensions.x = IntInputCreate(dimensions.x);
                        InputType inputType2 = FtdUiDefs.PrefabWidthAdd.DisplayButton();
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        _heightBar.Draw(prefab.Dimensions.y);
                        InputType inputType3 = FtdUiDefs.PrefabHeightRemove.DisplayButton();
                        dimensions.y = IntInputCreate(dimensions.y);
                        InputType inputType4 = FtdUiDefs.PrefabHeightAdd.DisplayButton();
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        _lengthBar.Draw(prefab.Dimensions.z);
                        InputType inputType5 = FtdUiDefs.PrefabLengthRemove.DisplayButton();
                        dimensions.z = IntInputCreate(dimensions.z);
                        InputType inputType6 = FtdUiDefs.PrefabLengthAdd.DisplayButton();
                        GUILayout.EndHorizontal();

                        DisplayPrefabOptions(prefab, displayCapture: true);
                        GUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndArea();
                        if (inputType != 0)
                        {
                            dimensions.x -= ((inputType == InputType.LeftClick) ? 1 : 5);
                        }
                        if (inputType2 != 0)
                        {
                            dimensions.x += ((inputType2 == InputType.LeftClick) ? 1 : 5);
                        }
                        if (inputType3 != 0)
                        {
                            dimensions.y -= ((inputType3 == InputType.LeftClick) ? 1 : 5);
                        }
                        if (inputType4 != 0)
                        {
                            dimensions.y += ((inputType4 == InputType.LeftClick) ? 1 : 5);
                        }
                        if (inputType5 != 0)
                        {
                            dimensions.z -= ((inputType5 == InputType.LeftClick) ? 1 : 5);
                        }
                        if (inputType6 != 0)
                        {
                            dimensions.z += ((inputType6 == InputType.LeftClick) ? 1 : 5);
                        }
                        dimensions.x = Clamping.Clamp(dimensions.x, 1, 999);
                        dimensions.y = Clamping.Clamp(dimensions.y, 1, 999);
                        dimensions.z = Clamping.Clamp(dimensions.z, 1, 999);
                        prefab.Dimensions = dimensions;
                    }
                }
                else if (info.AddRemove == enumAddRemove.placeprefab)
                {
                    SavedSubObject prefab2 = singleton.BuildingWith.Prefab;
                    if (prefab2 != null && prefab2.IsValid)
                    {
                        GUILayout.BeginArea(screenRect);
                        GUILayout.BeginVertical(ConsoleStyles.Instance.Styles.Windows.ConsoleWindow.Style);
                        DisplayMessage();
                        DisplayPrefabOptions(prefab2, displayCapture: false);
                        GUILayout.EndVertical();
                        GUILayout.FlexibleSpace();
                        GUILayout.EndArea();
                    }
                }

                if (flag) GUI.FocusControl("");



                return false;
            }

            private static int IntInputCreate(int num)
            {
                StylePlus style = ConsoleStyles.Instance.Styles.Sliders.TextInput;
                string temp = style.TextField(num.ToString(), GUILayout.Width(50));
                return int.Parse(temp);
            }
        }

        [HarmonyPatch(typeof(HudBuildCommands), "DisplayPrefabOptions", new Type[] { typeof(SavedSubObject), typeof(bool) })]
        private static class HudBuildCommands_DisplayPrefabOptions_Patch
        {
            private static bool Prefix(HudBuildCommands __instance, SavedSubObject prefab, bool displayCapture)
            {
                ILocFile _locFile = HudBuildCommands._locFile;
                BlueprintFileModel GetPrefabFileModel(SavedSubObject prefab2, string name) => Traverse.Create(__instance).Method("GetPrefabFileModel", prefab2, name).GetValue<BlueprintFileModel>();



                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (FtdUiDefs.EraseArea.DisplayButton() != 0)
                {
                    cBuild.GetSingleton().EraseArea();
                }
                if (prefab.IsValid)
                {

                    InputType inputType2 = FtdUiDefs.ClearPrefab.DisplayButton();
                    InputType inputType = FtdUiDefs.SavePrefab.DisplayButton();
                    if (inputType != 0)
                    {
                        BlueprintFolder prefabFolder = GameFolders.GetPrefabFolder();
                        GuiPopUp.Instance.Add(new PopupTreeViewSave<BlueprintFileModel>(_locFile.Get("Popup_SavePrefab", "Save prefab"), FtdGuiUtils.GetFileBrowserFor(prefabFolder), delegate (string s, bool b)
                        {
                            if (b)
                            {
                                prefab.Name = s;
                            }
                        }, (string s) => GetPrefabFileModel(prefab, s), string.IsNullOrEmpty(prefab.Name) ? _locFile.Get("Popup_SavePrefab_Desc", "New prefab") : prefab.Name));
                    }
                    if (inputType2 != 0)
                    {
                        cBuild.GetSingleton().SetLoadPrefab(new SavedSubObject(null)
                        {
                            Dimensions = prefab.Dimensions
                        });
                    }
                }
                if (displayCapture && !prefab.IsValid && FtdUiDefs.CapturePrefab.DisplayButton() != 0)
                {
                    cBuild.GetSingleton().FillPrefab();
                }
                if (FtdUiDefs.LoadPrefab.DisplayButton() != 0)
                {
                    BlueprintFolder folder = GameFolders.GetPrefabFolder();
                    GuiPopUp.Instance.Add(new PopupTreeView(_locFile.Get("Popup_LoadPrefab", "Load prefab"), FtdGuiUtils.GetFileBrowserFor(folder), delegate (string s, bool b)
                    {
                        if (b)
                        {
                            cBuild singleton = cBuild.GetSingleton();
                            Blueprint blueprint = folder.GetFile(s, appendFileExtension: true).Load();
                            singleton.SetLoadPrefab(new SavedSubObject(blueprint)
                            {
                                Name = s
                            });
                        }
                    }));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();



                return false;
            }
        }
    }
}
