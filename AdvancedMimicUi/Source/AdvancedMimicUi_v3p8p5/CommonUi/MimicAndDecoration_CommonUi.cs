using BrilliantSkies.Core.Help;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Layouts.DropDowns;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedMimicUi
{
    public static class MimicAndDecoration_CommonUi
    {
        private static string _resetText = "Reset";

        private static string _minusText = "-";

        private static string _plusText = "+";

        private static string _addMinusText = "Add -";

        private static string _addPlusText = "Add +";

        private static Func<string>[] Texts = new Func<string>[]
        {
            () => _minusText + HuwaSettingsTab.AddPosition,
            () => _plusText + HuwaSettingsTab.AddPosition,
            () => _minusText + HuwaSettingsTab.AddScale,
            () => _plusText + HuwaSettingsTab.AddScale,
            () => _minusText + HuwaSettingsTab.AddAngle,
            () => _plusText + HuwaSettingsTab.AddAngle
        };

        private static Func<string>[] ToolTips = new Func<string>[]
        {
            () => _addMinusText + HuwaSettingsTab.AddPosition,
            () => _addPlusText + HuwaSettingsTab.AddPosition,
            () => _addMinusText + HuwaSettingsTab.AddScale,
            () => _addPlusText + HuwaSettingsTab.AddScale,
            () => _addMinusText + HuwaSettingsTab.AddAngle,
            () => _addPlusText + HuwaSettingsTab.AddAngle
        };

        public static ScreenSegmentTable BarsUiGeneration(ConsoleUiScreen screen, HuwaUi<MimicAndDecorationCommonData> hUi_0, bool advPositioning = false)
        {
            ScreenSegmentTable screenSegmentTable = screen.CreateTableSegment(5, advPositioning ? 6 : 9);
            screenSegmentTable.SqueezeTable = true;

            SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>[] subjectiveFloats = new SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>[9];

            subjectiveFloats[0] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, -20f, 20f, 0.05f, 0f, M.m<MimicAndDecorationCommonData>(I => I.Positioning.x), "Left right positioning {0}", (I, f) => I.PositioningX = f, new ToolTip("Adjust the left right positioning of the mesh"));
            subjectiveFloats[1] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, -20f, 20f, 0.05f, 0f, M.m<MimicAndDecorationCommonData>(I => I.Positioning.y), "Up down positioning {0}", (I, f) => I.PositioningY = f, new ToolTip("Adjust the up down positioning of the mesh"));
            subjectiveFloats[2] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, -20f, 20f, 0.05f, 0f, M.m<MimicAndDecorationCommonData>(I => I.Positioning.z), "Forward backward positioning {0}", (I, f) => I.PositioningZ = f, new ToolTip("Adjust the forward backward positioning of the mesh"));

            subjectiveFloats[3] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, 0.05f, 10f, 0.05f, 1f, M.m<MimicAndDecorationCommonData>(I => I.Scaling.x), "Left right scaling {0}", (I, f) => I.ScalingX = f, new ToolTip("Adjust the left right scaling of the mesh"));
            subjectiveFloats[4] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, 0.05f, 10f, 0.05f, 1f, M.m<MimicAndDecorationCommonData>(I => I.Scaling.y), "Up down scaling {0}", (I, f) => I.ScalingY = f, new ToolTip("Adjust the up down scaling of the mesh"));
            subjectiveFloats[5] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, 0.05f, 10f, 0.05f, 1f, M.m<MimicAndDecorationCommonData>(I => I.Scaling.z), "Forward backward scaling {0}", (I, f) => I.ScalingZ = f, new ToolTip("Adjust the forward backward scaling of the mesh"));

            subjectiveFloats[6] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, -180f, 180f, 0.1f, 0f, M.m<MimicAndDecorationCommonData>(I => Rounding.R1(Angles.FixRot180To180(I.Orientation.y))), "Yaw {0:0.##}", (I, f) => I.OrientationY = f, new ToolTip("Adjust the yaw of the mesh"));
            subjectiveFloats[7] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, -90f, 90f, 0.1f, 0f, M.m<MimicAndDecorationCommonData>(I => -Rounding.R1(Angles.FixRot180To180(I.Orientation.x))), "Pitch {0:0.##}", (I, f) => I.OrientationX = -f, new ToolTip("Adjust the pitch of the mesh"));
            subjectiveFloats[8] = SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData>.Quick(
                hUi_0.Focus, -180f, 180f, 0.1f, 0f, M.m<MimicAndDecorationCommonData>(I => Rounding.R1(Angles.FixRot180To180(I.Orientation.z))), "Roll {0:0.##}", (I, f) => I.OrientationZ = f, new ToolTip("Adjust the roll of the mesh"));

            Array.ForEach(subjectiveFloats,
                (SubjectiveFloatClampedWithBarFromMiddle<MimicAndDecorationCommonData> s) =>
                {
                    s.TextBox.ForceRounding = false;
                    HuwaUi.ABar_AddMember_Access(s).FastWidthOptimization = true;
                });

            screenSegmentTable.AddInterpretter(subjectiveFloats[0]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.PositioningX *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[0], ToolTips[0], I => I.PositioningX -= HuwaSettingsTab.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[1], ToolTips[1], I => I.PositioningX += HuwaSettingsTab.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.PositioningX = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[1]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.PositioningY *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[0], ToolTips[0], I => I.PositioningY -= HuwaSettingsTab.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[1], ToolTips[1], I => I.PositioningY += HuwaSettingsTab.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.PositioningY = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[2]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.PositioningZ *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[0], ToolTips[0], I => I.PositioningZ -= HuwaSettingsTab.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[1], ToolTips[1], I => I.PositioningZ += HuwaSettingsTab.AddPosition));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.PositioningZ = 0f));

            if (!advPositioning)
            {
                screenSegmentTable.AddInterpretter(subjectiveFloats[3]);
                screenSegmentTable.AddInterpretter(new Empty());
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[2], ToolTips[2], I => I.ScalingX -= HuwaSettingsTab.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[3], ToolTips[3], I => I.ScalingX += HuwaSettingsTab.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.ScalingX = 1f));

                screenSegmentTable.AddInterpretter(subjectiveFloats[4]);
                screenSegmentTable.AddInterpretter(new Empty());
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[2], ToolTips[2], I => I.ScalingY -= HuwaSettingsTab.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[3], ToolTips[3], I => I.ScalingY += HuwaSettingsTab.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.ScalingY = 1f));

                screenSegmentTable.AddInterpretter(subjectiveFloats[5]);
                screenSegmentTable.AddInterpretter(new Empty());
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[2], ToolTips[2], I => I.ScalingZ -= HuwaSettingsTab.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[3], ToolTips[3], I => I.ScalingZ += HuwaSettingsTab.AddScale));
                screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.ScalingZ = 1f));
            }

            screenSegmentTable.AddInterpretter(subjectiveFloats[6]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.OrientationY *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[4], ToolTips[4], I => I.OrientationY -= HuwaSettingsTab.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[5], ToolTips[5], I => I.OrientationY += HuwaSettingsTab.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.OrientationY = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[7]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.OrientationX *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[4], ToolTips[4], I => I.OrientationX += HuwaSettingsTab.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[5], ToolTips[5], I => I.OrientationX -= HuwaSettingsTab.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.OrientationX = 0f));

            screenSegmentTable.AddInterpretter(subjectiveFloats[8]);
            screenSegmentTable.AddInterpretter(hUi_0.FlipButton(I => I.OrientationZ *= -1f));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[4], ToolTips[4], I => I.OrientationZ -= HuwaSettingsTab.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(Texts[5], ToolTips[5], I => I.OrientationZ += HuwaSettingsTab.AddAngle));
            screenSegmentTable.AddInterpretter(hUi_0.AddButton(_resetText, _resetText, I => I.OrientationZ = 0f));

            return screenSegmentTable;
        }

        public static ScreenSegmentStandardHorizontal AxisFlip<T>(ConsoleUiScreen screen, HuwaUi<T> hUi_0, Action<T> actionX, Action<T> actionY, Action<T> actionZ)
        {
            ScreenSegmentStandardHorizontal screenSegment = screen.CreateStandardHorizontalSegment();

            screenSegment.AddInterpretter(hUi_0.AddButton("X axis flip", "Reverses the position and angle based on the X axis.", actionX));
            screenSegment.AddInterpretter(hUi_0.AddButton("Y axis flip", "Reverses the position and angle based on the Y axis.", actionY));
            screenSegment.AddInterpretter(hUi_0.AddButton("Z axis flip", "Reverses the position and angle based on the Z axis.", actionZ));

            return screenSegment;
        }

        public static ScreenSegmentStandardHorizontal MaterialReplacementDropDown(ConsoleUiScreen screen, MimicAndDecorationCommonData madcd)
        {
            ScreenSegmentStandardHorizontal screenSegment = screen.CreateStandardHorizontalSegment();
            DropDownMenuAlt<Guid> dropDownMenuAlt = new DropDownMenuAlt<Guid>(TextAnchor.MiddleCenter);
            List<DropDownMenuAltItem<Guid>> list = new List<DropDownMenuAltItem<Guid>>();

            list.Add(new DropDownMenuAltItem<Guid>
            {
                ObjectForAction = Guid.Empty,
                Name = "No material override",
                ToolTip = "Don't override the material"
            });

            foreach (MaterialDefinition materialDefinition in Configured.i.Materials.Components)
            {
                list.Add(new DropDownMenuAltItem<Guid>
                {
                    ObjectForAction = materialDefinition.ComponentId.Guid,
                    ToolTip = "Don't expect all materials to work. Alloy, metal, rubber, stone and heavy armour are tri-planar mapped and can fit to any mesh. Other materials have different texture atlas layouts and will not readily swap between meshes. The emissivity-only texture will work on anything.",
                    Name = materialDefinition.ComponentId.Name.ToString()
                });
            }

            dropDownMenuAlt.SetItems(list.ToArray());
            screenSegment.AddInterpretter(new DropDown<MimicAndDecorationCommonData, Guid>(madcd, dropDownMenuAlt, (I, e) => I.MaterialReplacement == e, (I, e) => I.MaterialReplacement = e));

            return screenSegment;
        }
    }
}
