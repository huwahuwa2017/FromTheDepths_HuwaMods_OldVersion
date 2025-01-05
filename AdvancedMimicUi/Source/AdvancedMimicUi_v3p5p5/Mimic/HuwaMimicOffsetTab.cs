using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Core.Help;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaMimicOffsetTab : SuperScreen<Mimic>
    {
        private static float[] parameters = new float[9];

        public override Content Name
        {
            get
            {
                return new Content("Add offset", new ToolTip("Add a value for each parameter."), "HuwaMimicUiTab_4");
            }
        }

        public HuwaMimicOffsetTab(ConsoleWindow window, Mimic focus) : base(window, focus)
        {
        }

        public override void Build()
        {
            ScreenSegmentTable screenSegment_0 = CreateTableSegment(5, 5);
            screenSegment_0.SpaceAbove = 10f;
            screenSegment_0.SpaceBelow = 10f;
            screenSegment_0.SqueezeTable = false;

            screenSegment_0.AddInterpretter(StringDisplay.Quick("Add position", "Add a value for each parameter")).Justify = TextAnchor.UpperRight;
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[0]), new ToolTip("Add a value to 'Left right positioning'"), (Mimic m, float f) => parameters[0] = f, M.m((Mimic m) => "X ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[1]), new ToolTip("Add a value to 'Up down positioning'"), (Mimic m, float f) => parameters[1] = f, M.m((Mimic m) => "Y ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[2]), new ToolTip("Add a value to 'Forward backward positioning'"), (Mimic m, float f) => parameters[2] = f, M.m((Mimic m) => "Z ")));
            screenSegment_0.AddInterpretter(new Empty());

            screenSegment_0.AddInterpretter(new Blank(6f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));

            screenSegment_0.AddInterpretter(StringDisplay.Quick("Add scale", "Add a value for each parameter")).Justify = TextAnchor.UpperRight;
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[3]), new ToolTip("Add a value to 'Left right scaling'"), (Mimic m, float f) => parameters[3] = f, M.m((Mimic m) => "X ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[4]), new ToolTip("Add a value to 'Up down scaling'"), (Mimic m, float f) => parameters[4] = f, M.m((Mimic m) => "Y ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[5]), new ToolTip("Add a value to 'Forward backward scaling'"), (Mimic m, float f) => parameters[5] = f, M.m((Mimic m) => "Z ")));
            screenSegment_0.AddInterpretter(new Empty());

            screenSegment_0.AddInterpretter(new Blank(6f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));
            screenSegment_0.AddInterpretter(new Blank(9f));

            screenSegment_0.AddInterpretter(StringDisplay.Quick("Add angle", "Add a value for each parameter")).Justify = TextAnchor.UpperRight;
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[6]), new ToolTip("Add a value to 'Yaw'"), (Mimic m, float f) => parameters[6] = f, M.m((Mimic m) => "Y ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[7]), new ToolTip("Add a value to 'Pitch'"), (Mimic m, float f) => parameters[7] = f, M.m((Mimic m) => "X ")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<Mimic>.Quick(_focus, M.m((Mimic m) => parameters[8]), new ToolTip("Add a value to 'Roll'"), (Mimic m, float f) => parameters[8] = f, M.m((Mimic m) => "Z ")));
            screenSegment_0.AddInterpretter(new Empty());



            ScreenSegmentStandard screenSegment_1 = CreateStandardSegment(InsertPosition.OnCursor);

            screenSegment_1.AddInterpretter(SubjectiveButton<Mimic>.Quick(_focus, "Add offset", new ToolTip("Add a value to each setting value"),
                (Mimic m) =>
                {
                    MimicData d = m.Data;

                    d.Positioning.x = Rounding.R4(d.Positioning.x + parameters[0]);
                    d.Positioning.y = Rounding.R4(d.Positioning.y + parameters[1]);
                    d.Positioning.z = Rounding.R4(d.Positioning.z + parameters[2]);

                    d.Scaling.x = Rounding.R4(d.Scaling.x + parameters[3]);
                    d.Scaling.y = Rounding.R4(d.Scaling.y + parameters[4]);
                    d.Scaling.z = Rounding.R4(d.Scaling.z + parameters[5]);

                    d.Orientation.y = Rounding.R4(d.Orientation.y + parameters[6]);
                    d.Orientation.x = Rounding.R4(d.Orientation.x - parameters[7]);
                    d.Orientation.z = Rounding.R4(d.Orientation.z + parameters[8]);
                }));
        }
    }
}
