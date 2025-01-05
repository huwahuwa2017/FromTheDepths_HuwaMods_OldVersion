using BrilliantSkies.Core.Help;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using ES2_PolygonControl;
using System;

namespace AdvancedMimicUi
{
    public static class MimicAndDecoration_CommonUi
    {
        private static string ResetText = "Reset";

        private static string MinusText = "-";

        private static string PlusText = "+";

        private static string AddMinusText = "Add -";

        private static string AddPlusText = "Add +";

        private static Func<string>[] Texts = new Func<string>[]
        {
            () => MinusText + SettingsSuperScreen.AddPosition,
            () => PlusText + SettingsSuperScreen.AddPosition,
            () => MinusText + SettingsSuperScreen.AddScale,
            () => PlusText + SettingsSuperScreen.AddScale,
            () => MinusText + SettingsSuperScreen.AddAngle,
            () => PlusText + SettingsSuperScreen.AddAngle
        };

        private static Func<string>[] ToolTips = new Func<string>[]
        {
            () => AddMinusText + SettingsSuperScreen.AddPosition,
            () => AddPlusText + SettingsSuperScreen.AddPosition,
            () => AddMinusText + SettingsSuperScreen.AddScale,
            () => AddPlusText + SettingsSuperScreen.AddScale,
            () => AddMinusText + SettingsSuperScreen.AddAngle,
            () => AddPlusText + SettingsSuperScreen.AddAngle
        };

        public static void BarsUiGeneration(ScreenSegmentTable screenSegmentTable, HuwaUi<MimicAndDecoration_CommonData> hUi_0, bool advPositioning = false)
        {
            SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>[] subjectiveFloats = new SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>[9];

            subjectiveFloats[0] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, -10f, 10f, 0.05f, 0f, M.m<MimicAndDecoration_CommonData>(I => I.Positioning.x), "Left right positioning {0}", (I, f) => I.PositioningX = f, new ToolTip("Adjust the left right positioning of the mesh"));
            subjectiveFloats[1] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, -10f, 10f, 0.05f, 0f, M.m<MimicAndDecoration_CommonData>(I => I.Positioning.y), "Up down positioning {0}", (I, f) => I.PositioningY = f, new ToolTip("Adjust the up down positioning of the mesh"));
            subjectiveFloats[2] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, -10f, 10f, 0.05f, 0f, M.m<MimicAndDecoration_CommonData>(I => I.Positioning.z), "Forward backward positioning {0}", (I, f) => I.PositioningZ = f, new ToolTip("Adjust the forward backward positioning of the mesh"));

            subjectiveFloats[3] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, 0.05f, 10f, 0.05f, 1f, M.m<MimicAndDecoration_CommonData>(I => I.Scaling.x), "Left right scaling {0}", (I, f) => I.ScalingX = f, new ToolTip("Adjust the left right scaling of the mesh"));
            subjectiveFloats[4] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, 0.05f, 10f, 0.05f, 1f, M.m<MimicAndDecoration_CommonData>(I => I.Scaling.y), "Up down scaling {0}", (I, f) => I.ScalingY = f, new ToolTip("Adjust the up down scaling of the mesh"));
            subjectiveFloats[5] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, 0.05f, 10f, 0.05f, 1f, M.m<MimicAndDecoration_CommonData>(I => I.Scaling.z), "Forward backward scaling {0}", (I, f) => I.ScalingZ = f, new ToolTip("Adjust the forward backward scaling of the mesh"));

            subjectiveFloats[6] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, -180f, 180f, 0.1f, 0f, M.m<MimicAndDecoration_CommonData>(I => Rounding.R1(Angles.FixRot180To180(I.Orientation.y))), "Yaw {0:0.##}", (I, f) => I.OrientationY = f, new ToolTip("Adjust the yaw of the mesh"));
            subjectiveFloats[7] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, -90f, 90f, 0.1f, 0f, M.m<MimicAndDecoration_CommonData>(I => -Rounding.R1(Angles.FixRot180To180(I.Orientation.x))), "Pitch {0:0.##}", (I, f) => I.OrientationX = -f, new ToolTip("Adjust the pitch of the mesh"));
            subjectiveFloats[8] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData>.Quick(
                hUi_0.Focus, -180f, 180f, 0.1f, 0f, M.m<MimicAndDecoration_CommonData>(I => Rounding.R1(Angles.FixRot180To180(I.Orientation.z))), "Roll {0:0.##}", (I, f) => I.OrientationZ = f, new ToolTip("Adjust the roll of the mesh"));

            Array.ForEach(subjectiveFloats,
                (SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecoration_CommonData> s) =>
                {
                    s.TextBox.ForceRounding = false;
                    HuwaUi.ABar_AddMember_Access(s).FastWidthOptimization = true;
                });

            screenSegmentTable.AddInterpretter(subjectiveFloats[0]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.PositioningX *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[0], ToolTips[0], I => I.PositioningX -= SettingsSuperScreen.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[1], ToolTips[1], I => I.PositioningX += SettingsSuperScreen.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.PositioningX = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[1]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.PositioningY *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[0], ToolTips[0], I => I.PositioningY -= SettingsSuperScreen.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[1], ToolTips[1], I => I.PositioningY += SettingsSuperScreen.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.PositioningY = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[2]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.PositioningZ *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[0], ToolTips[0], I => I.PositioningZ -= SettingsSuperScreen.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[1], ToolTips[1], I => I.PositioningZ += SettingsSuperScreen.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.PositioningZ = 0f));

            if (!advPositioning)
            {
                screenSegmentTable.AddInterpretter(subjectiveFloats[3]);
                screenSegmentTable.AddInterpretter(new Empty());
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[2], ToolTips[2], I => I.ScalingX -= SettingsSuperScreen.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[3], ToolTips[3], I => I.ScalingX += SettingsSuperScreen.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.ScalingX = 1f));

                screenSegmentTable.AddInterpretter(subjectiveFloats[4]);
                screenSegmentTable.AddInterpretter(new Empty());
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[2], ToolTips[2], I => I.ScalingY -= SettingsSuperScreen.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[3], ToolTips[3], I => I.ScalingY += SettingsSuperScreen.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.ScalingY = 1f));

                screenSegmentTable.AddInterpretter(subjectiveFloats[5]);
                screenSegmentTable.AddInterpretter(new Empty());
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[2], ToolTips[2], I => I.ScalingZ -= SettingsSuperScreen.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[3], ToolTips[3], I => I.ScalingZ += SettingsSuperScreen.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.ScalingZ = 1f));
            }

            screenSegmentTable.AddInterpretter(subjectiveFloats[6]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.OrientationY *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[4], ToolTips[4], I => I.OrientationY -= SettingsSuperScreen.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[5], ToolTips[5], I => I.OrientationY += SettingsSuperScreen.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.OrientationY = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[7]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.OrientationX *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[4], ToolTips[4], I => I.OrientationX += SettingsSuperScreen.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[5], ToolTips[5], I => I.OrientationX -= SettingsSuperScreen.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.OrientationX = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[8]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.OrientationZ *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[4], ToolTips[4], I => I.OrientationZ -= SettingsSuperScreen.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[5], ToolTips[5], I => I.OrientationZ += SettingsSuperScreen.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(ResetText, ResetText, I => I.OrientationZ = 0f));
        }
    }
}
