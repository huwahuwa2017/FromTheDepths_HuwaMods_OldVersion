using BrilliantSkies.Core.Help;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Core.UiSounds;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaDecorationPositioningTab : SuperScreen<HuwaMultiDecorationUi>
    {
        public override Content Name
        {
            get
            {
                return new Content("Positioning", new ToolTip("Settings for adjusting the position."), "PositioningTab");
            }
        }

        public HuwaDecorationPositioningTab(ConsoleWindow window, HuwaMultiDecorationUi focus) : base(window, focus)
        {
        }

        public override void Build()
        {
            HuwaUi<HuwaMultiDecorationUi> hUi_0 = new HuwaUi<HuwaMultiDecorationUi>(_focus);

            CreateHeader("Shift tether point");

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(3, 2);
            screenSegment_0.SqueezeTable = false;

            screenSegment_0.AddInterpretter(hUi_0.AddButton("right", "Move the decoration's tether point right", () => STP_Action(I => I.Positioning.Us.x >= -9f, Vector3i.right)));
            screenSegment_0.AddInterpretter(hUi_0.AddButton("up", "Move the decoration's tether point up", () => STP_Action(I => I.Positioning.Us.y >= -9f, Vector3i.up)));
            screenSegment_0.AddInterpretter(hUi_0.AddButton("forward", "Move the decoration's tether point forward", () => STP_Action(I => I.Positioning.Us.z >= -9f, Vector3i.forward)));
            screenSegment_0.AddInterpretter(hUi_0.AddButton("left", "Move the decoration's tether point left", () => STP_Action(I => I.Positioning.Us.x <= 9f, Vector3i.left)));
            screenSegment_0.AddInterpretter(hUi_0.AddButton("down", "Move the decoration's tether point down", () => STP_Action(I => I.Positioning.Us.y <= 9f, Vector3i.down)));
            screenSegment_0.AddInterpretter(hUi_0.AddButton("back", "Move the decoration's tether point back", () => STP_Action(I => I.Positioning.Us.z <= 9f, Vector3i.back)));



            CreateHeader("Decorations rotation");

            ScreenSegmentTable screenSegment_1 = CreateTableSegment(3, 2);
            screenSegment_1.SqueezeTable = false;

            screenSegment_1.AddInterpretter(hUi_0.AddButton("X axis 90°rotation", "Rotate the decorations 90°around the X axis", () => DR_Action(new Vector3(90f, 0f, 0f))));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Y axis 90°rotation", "Rotate the decorations 90°around the Y axis", () => DR_Action(new Vector3(0f, 90f, 0f))));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Z axis 90°rotation", "Rotate the decorations 90°around the Z axis", () => DR_Action(new Vector3(0f, 0f, 90f))));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("X axis -90°rotation", "Rotate the decorations -90°around the X axis", () => DR_Action(new Vector3(-90f, 0f, 0f))));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Y axis -90°rotation", "Rotate the decorations -90°around the Y axis", () => DR_Action(new Vector3(0f, -90f, 0f))));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Z axis -90°rotation", "Rotate the decorations -90°around the Z axis", () => DR_Action(new Vector3(0f, 0f, -90f))));



            CreateHeader("Decorations flip");

            ScreenSegmentTable screenSegment_2 = CreateTableSegment(3, 1);
            screenSegment_2.SqueezeTable = false;
            screenSegment_2.SpaceBelow = 20f;

            screenSegment_2.AddInterpretter(hUi_0.AddButton("X axis flip", "Reverses the position and angle based on the X axis.", () => DF_Action("x")));
            screenSegment_2.AddInterpretter(hUi_0.AddButton("Y axis flip", "Reverses the position and angle based on the Y axis.", () => DF_Action("y")));
            screenSegment_2.AddInterpretter(hUi_0.AddButton("Z axis flip", "Reverses the position and angle based on the Z axis.", () => DF_Action("z")));
        }

        private void STP_Action(Func<Decoration, bool> check, Vector3i shift)
        {
            GetDecorations(out List<Decoration> decorations, out AllConstructDecorations decorationManager);

            foreach (Decoration decoration in decorations)
            {
                if (check(decoration))
                {
                    decorationManager.ShiftDecoration(decoration, shift);
                    decoration.Positioning.Us -= shift;

                    GUISoundManager.GetSingleton().PlayBeep();
                }
                else
                {
                    GUISoundManager.GetSingleton().PlayFailure();
                }
            }
        }

        private void DR_Action(Vector3 EA)
        {
            Quaternion addRotation = Quaternion.Euler(EA);

            GetDecorations(out List<Decoration> decorations, out _);

            foreach (Decoration decoration in decorations)
            {
                decoration.Positioning.Us = Vector3Rounding(addRotation * decoration.Positioning.Us);
                decoration.Orientation.Us = Vector3Rounding((addRotation * decoration.OrientationQuaternion).eulerAngles);

                GUISoundManager.GetSingleton().PlayBeep();
            }
        }

        private void DF_Action(string type)
        {
            GetDecorations(out List<Decoration> decorations, out _);
            List<MimicAndDecorationCommonData> MAD_CD_List = decorations.Select(I => new MimicAndDecorationCommonData(I)).ToList();

            switch (type)
            {
                case "x":
                    MAD_CD_List.ForEach(HuwaMimicHelp.XAxisFlip);
                    break;
                case "y":
                    MAD_CD_List.ForEach(HuwaMimicHelp.YAxisFlip);
                    break;
                case "z":
                    MAD_CD_List.ForEach(HuwaMimicHelp.ZAxisFlip);
                    break;
            }
        }

        private void GetDecorations(out List<Decoration> decorations, out AllConstructDecorations decorationManager)
        {
            decorations = new List<Decoration>();

            if (_focus.GetDecoration(out Decoration deco))
            {
                decorations.Add(deco);
                decorationManager = deco.OurManager;
            }
            else
            {
                decorations = _focus.GetDecorationsSubject().Decorations;
                decorationManager = decorations?[0].OurManager;
            }
        }

        private Vector3 Vector3Rounding(Vector3 vector)
        {
            return new Vector3(Rounding.R4(vector.x), Rounding.R4(vector.y), Rounding.R4(vector.z));
        }
    }
}
