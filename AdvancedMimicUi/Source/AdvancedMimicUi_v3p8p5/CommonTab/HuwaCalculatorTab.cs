using BrilliantSkies.Core.Help;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Texts;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaCalculatorTab : SuperScreen<int>
    {
        private static Vector2 _inputData = new Vector2(2f, 1f);

        private static string _angle;

        private static string _hypotenuse;



        public override Content Name
        {
            get
            {
                return new Content("Calculation", new ToolTip("Calculate the angle and hypotenuse length."), "CalculationTab");
            }
        }

        public HuwaCalculatorTab(ConsoleWindow window) : base(window, default)
        {
        }

        public override void Build()
        {
            HuwaUi<int> hUi = new HuwaUi<int>(0);
            DataUpdate(_inputData.x, _inputData.y);

            ScreenSegmentStandardHorizontal screenSegment_0 = CreateStandardHorizontalSegment();
            screenSegment_0.SpaceAbove = 10f;

            screenSegment_0.AddInterpretter(SimpleFloatInput<int>.Quick(_focus, M.m<int>(I => _inputData.x), new ToolTip("Input Width (X)"), (I, f) => DataUpdate(f, _inputData.y), M.m<int>(I => "Width : ")));
            screenSegment_0.AddInterpretter(hUi.AddButton("<- Swap ->", "Swap Height and Width.", I => DataUpdate(_inputData.y, _inputData.x)));
            screenSegment_0.AddInterpretter(SimpleFloatInput<int>.Quick(_focus, M.m<int>(I => _inputData.y), new ToolTip("Input Height (Y)"), (I, f) => DataUpdate(_inputData.x, f), M.m<int>(I => "Height : ")));



            ScreenSegmentTable screenSegment_1 = CreateTableSegment(2, 2);
            screenSegment_1.SqueezeTable = false;
            screenSegment_1.SpaceAbove = 10f;
            screenSegment_1.SpaceBelow = 20f;

            screenSegment_1.AddInterpretter(TextInput<int>.Quick(_focus, M.m<int>(I => _angle), "atan(Y / X) * rad2deg  = ", new ToolTip("Calculation result of angle."), (int I, string s) => { }));
            screenSegment_1.AddInterpretter(hUi.AddButton("Copy", "Copy this value to the clipboard for pasting elsewhere.", I => GUIUtility.systemCopyBuffer = _angle));

            screenSegment_1.AddInterpretter(TextInput<int>.Quick(_focus, M.m<int>(I => _hypotenuse), "(X ^ 2 + Y ^ 2) ^ 0.5  = ", new ToolTip("Calculation result of hypotenuse."), (int I, string s) => { }));
            screenSegment_1.AddInterpretter(hUi.AddButton("Copy", "Copy this value to the clipboard for pasting elsewhere.", I => GUIUtility.systemCopyBuffer = _hypotenuse));
        }

        private static void DataUpdate(float x, float y)
        {
            _inputData.x = x;
            _inputData.y = y;

            _angle = Rounding.R4(Mathf.Atan(_inputData.y / _inputData.x) * Mathf.Rad2Deg).ToString();
            _hypotenuse = Rounding.R4(_inputData.magnitude).ToString();
        }
    }
}
