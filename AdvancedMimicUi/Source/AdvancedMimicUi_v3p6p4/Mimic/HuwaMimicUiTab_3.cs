using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Core.Help;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Texts;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using System;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaMimicUiTab_3 : SuperScreen<Mimic>
    {
        private static float Height = 1f;

        private static float Width = 2f;

        public override Content Name
        {
            get
            {
                return new Content("Calculation", new ToolTip("Calculate the angle and hypotenuse length."), "HuwaMimicUiTab_3");
            }
        }

        public HuwaMimicUiTab_3(ConsoleWindow window, Mimic focus) : base(window, focus)
        {
        }

        public override void Build()
        {
            ScreenSegmentTable screenSegment_0 = CreateTableSegment(3, 1);
            screenSegment_0.SpaceAbove = 10f;
            screenSegment_0.SpaceBelow = 10f;
            screenSegment_0.SqueezeTable = false;

            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => Height), new ToolTip("Input Height"), (Mimic m, float f) => Height = f, M.m((Mimic m) => "Height : ")));

            screenSegment_0.AddInterpretter(SubjectiveButton<Mimic>.Quick(_focus, "<- Swap ->", new ToolTip("Swap Height and Width."),
                (Mimic m) =>
                {
                    float num = Height;
                    Height = Width;
                    Width = num;
                }));

            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => Width), new ToolTip("Input Width"), (Mimic m, float f) => Width = f, M.m((Mimic m) => "Width : ")));



            ScreenSegmentTable screenSegment_1 = CreateTableSegment(2, 2);
            screenSegment_1.SqueezeTable = false;

            string AngleCal() => Rounding.R4((Mathf.Atan(Height / Width) * Mathf.Rad2Deg)).ToString();
            string HypotenuseCal() => Rounding.R4(Math.Sqrt(Math.Pow(Height, 2f) + Math.Pow(Width, 2f))).ToString();

            screenSegment_1.AddInterpretter(TextInput<Mimic>.Quick(_focus, M.m((Mimic I) => AngleCal()), "arcsin(Height / Width)  = ", new ToolTip("Calculation result of angle."), (Mimic I, string s) => { }));
            screenSegment_1.AddInterpretter(SubjectiveButton<Mimic>.Quick(_focus, "Copy", new ToolTip("Copy this value to the clipboard for pasting elsewhere."), (Mimic I) => GUIUtility.systemCopyBuffer = AngleCal()));

            screenSegment_1.AddInterpretter(TextInput<Mimic>.Quick(_focus, M.m((Mimic I) => HypotenuseCal()), "(Height ^ 2 + Width ^ 2) ^ 0.5  = ", new ToolTip("Calculation result of hypotenuse."), (Mimic I, string s) => { }));
            screenSegment_1.AddInterpretter(SubjectiveButton<Mimic>.Quick(_focus, "Copy", new ToolTip("Copy this value to the clipboard for pasting elsewhere."), (Mimic I) => GUIUtility.systemCopyBuffer = HypotenuseCal()));
        }
    }
}
