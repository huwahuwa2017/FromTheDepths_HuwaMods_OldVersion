using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Core.Collections.Arrays;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd.Avatar.Build;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;
using HarmonyLib;
using HuwaTech;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class ABar_AddMember : AddMemberBase<ABar>
    {
        public bool FastWidthOptimization { get; set; }
    }

    internal class AdvancedMimicUiPatch
    {
        [HarmonyPatch(typeof(Mimic), "Secondary", new Type[] { typeof(Transform) })]
        private class Mimic_Secondary_Patch
        {
            private static bool Prefix(Mimic __instance)
            {
                new HuwaMimicUi(__instance).ActivateGui(GuiActivateType.Standard);

                return false;
            }
        }

        [HarmonyPatch(typeof(Mimic), "AppendToolTip", new Type[] { typeof(ProTip) })]
        private class Mimic_AppendToolTip_Patch
        {
            private static bool Prefix(Mimic __instance, ProTip tip)
            {
                string name = HuwaMimicHelp.GetItemName(__instance.Data.MeshGuid);

                //ProTipSegment_LinesOfText proTipSegment_LinesOfText = tip.Add(new ProTipSegment_LinesOfText(500), Position.Last);
                //proTipSegment_LinesOfText.AddExtraLine($"Selected mesh : \n<<{name}>>");



                //__instance.AppendToolTip(tip);

                bool bm = cBuild.GetSingleton().buildMode != 0;

                if (!__instance.Data.HideTooltip || bm)
                {
                    tip.Add(new ProTipSegment_TitleSubTitle(Mimic._locFile.Get("SpecialName", "Mimic"), Mimic._locFile.Get("SpecialDescription", "Mimics the appearance of any other block")));
                    tip.Add(new ProTipSegment_Text(300, $"Mimicked block : <<{name}>>"));
                    tip.InfoOnly = false;

                    if (bm) __instance.LocateMeshContour();
                }

                return false;
            }
        }



        [HarmonyPatch(typeof(AllConstructDecorations), "AddOrEditDecorations", new Type[] { typeof(Vector3i) })]
        private class AllConstructDecorations_AddOrEditDecorations_Patch
        {
            private static bool Prefix(AllConstructDecorations __instance, Vector3i position)
            {
                List<Decoration> decorationsAtPosition;
                ThreeDDictionary<List<Decoration>> _decorationArray = Traverse.Create(__instance).Field("_decorationArray").GetValue<ThreeDDictionary<List<Decoration>>>();
                AllConstruct construct = Traverse.Create(__instance).Property("_construct").GetValue<AllConstruct>();

                if (_decorationArray.TryRead(position.x, position.y, position.z, out decorationsAtPosition))
                {
                    new HuwaMultiDecorationUi(new DecorationsSubject(__instance, position, decorationsAtPosition, construct.Main.ColorsRestricted)).ActivateGui(GuiActivateType.Stack);
                }
                else
                {
                    if (__instance.CanAddHere(position))
                    {
                        new HuwaMultiDecorationUi(new DecorationsSubject(__instance, position, null, construct.Main.ColorsRestricted)).ActivateGui(GuiActivateType.Stack);
                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(MultiDecorationEditor), "DisplayAScreenForThis", new Type[] { typeof(Decoration) })]
        private class MultiDecorationEditor_DisplayAScreenForThis_Patch
        {
            private static MultiDecorationEditor instance;

            private static ConsoleWindow _window
            {
                get
                {
                    return Traverse.Create(instance).Field("_window").GetValue<ConsoleWindow>();
                }
                set
                {
                    Traverse.Create(instance).Field("_window").SetValue(value);
                }
            }

            private static bool Prefix(MultiDecorationEditor __instance, Decoration decoration)
            {
                instance = __instance;
                DecorationsSubject _focus = Traverse.Create(__instance).Field("_focus").GetValue<DecorationsSubject>();

                DecorationTab decorationTab = new HuwaDecorationEditorTab(_window, decoration, _focus.Decorations.Count - 1);
                decorationTab.Build();
                _window.AllScreens.Add(decorationTab);
                _window.Screen = decorationTab;

                return false;
            }
        }



        [HarmonyPatch(typeof(ABar), "PrimaryLayout", new Type[] { typeof(SO_BuiltUi), typeof(StylePlus), typeof(string), typeof(string) })]
        private class ABar_PrimaryLayout_Patch
        {
            private static bool Prefix(ABar __instance, ref Rect __result, ref SO_BuiltUi styles, ref StylePlus styleForBar, ref string displayString, ref string tipCode, ref bool ____simpleLayout, ref bool ____anyHeldDown, ref float ____lastWidthUsed)
            {
                ABar_AddMember AM = ClassExpansion<ABar, ABar_AddMember>.Access(__instance);

                bool simpleLayout = ____simpleLayout;
                Rect result;
                if (simpleLayout)
                {
                    styleForBar.Layout(displayString, tipCode, Array.Empty<GUILayoutOption>());
                    result = GUILayoutUtility.GetLastRect();
                }
                else
                {
                    bool flag = ____anyHeldDown && ____lastWidthUsed != -1f;
                    if (flag)
                    {
                        styleForBar.Layout(displayString, tipCode, new GUILayoutOption[]
                        {
                        GUILayout.MinWidth(____lastWidthUsed),
                        GUILayout.MaxWidth(____lastWidthUsed)
                        });
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        bool flag2 = Event.current.type == EventType.Repaint;
                        if (flag2)
                        {
                            ____lastWidthUsed = lastRect.width;
                        }
                        result = lastRect;
                    }
                    else
                    {
                        bool flag3 = ____lastWidthUsed == -1f;
                        if (flag3)
                        {
                            styleForBar.Layout(displayString, tipCode, Array.Empty<GUILayoutOption>());
                        }
                        else
                        {
                            if (AM.FastWidthOptimization)
                            {
                                styleForBar.Layout(displayString, tipCode, new GUILayoutOption[]
                                {
                                    GUILayout.MinWidth(0f),
                                    GUILayout.MaxWidth(1280f)
                                });
                            }
                            else
                            {
                                styleForBar.Layout(displayString, tipCode, new GUILayoutOption[]
                                {
                                    GUILayout.MinWidth(____lastWidthUsed - 15f),
                                    GUILayout.MaxWidth(____lastWidthUsed + 15f)
                                });
                            }
                        }
                        Rect lastRect2 = GUILayoutUtility.GetLastRect();
                        bool flag4 = Event.current.type == EventType.Repaint;
                        if (flag4)
                        {
                            ____lastWidthUsed = lastRect2.width;
                        }
                        result = lastRect2;
                    }
                }
                __result = result;

                return false;
            }
        }
    }
}
