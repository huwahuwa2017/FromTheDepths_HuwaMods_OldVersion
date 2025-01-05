using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedMimicUi
{
    public class HuwaMultiDecorationUi : MultiDecorationEditor
    {
        private ConsoleWindow _consoleWindow_1;

        private ConsoleWindow ConsoleWindow_0
        {
            get
            {
                return Traverse.Create(this).Field("_window").GetValue<ConsoleWindow>();
            }
            set
            {
                Traverse.Create(this).Field("_window").SetValue(value);
            }
        }

        public HuwaMultiDecorationUi(DecorationsSubject subject) : base(subject)
        {
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            ConsoleWindow_0 = NewWindow(0, _locFile.Get("MultiDecWindow", "Decorations editor", true), new ScaledRectangle(10f, 10f, 600f, 0f));
            ConsoleWindow_0.DisplayTextPrompt = false;
            List<SuperScreen> superScreenList = new List<SuperScreen>();
            superScreenList.Add(new HuwaMultiDecorationEditorTab(ConsoleWindow_0, _focus, this));
            superScreenList.AddRange(_focus.Decorations.Select((d, i) => new HuwaDecorationEditorTab(ConsoleWindow_0, d, i)));
            ConsoleWindow_0.SetMultipleTabs(superScreenList.ToArray());

            _consoleWindow_1 = NewWindow(2, "Other", new ScaledRectangle(670f, 10f, 600f, 400f));
            _consoleWindow_1.DisplayTextPrompt = false;
            _consoleWindow_1.MinimumWindowHeight = new PixelSizing(35f, Dimension.Height);
            _consoleWindow_1.SetMultipleTabs
            (
                new HuwaDecorationMeshSelectTab(_consoleWindow_1, this),
                new HuwaDecorationPositioningTab(_consoleWindow_1, this),
                new HuwaCalculatorTab(_consoleWindow_1),
                new HuwaDecorationTriangleGeneratorTab(_consoleWindow_1, this),
                new HuwaDecorationSettingsTab(_consoleWindow_1, _focus)
            );

            return ConsoleWindow_0;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            HuwaTriangleGeneratorTab.CanvasSetActive(_consoleWindow_1.Screen is HuwaTriangleGeneratorTab);
        }

        public DecorationsSubject GetDecorationsSubject()
        {
            return _focus;
        }

        public bool GetDecoration(out Decoration deco)
        {
            if (ConsoleWindow_0 != null)
            {
                ConsoleUiScreen screen = ConsoleWindow_0.Screen;

                if (screen is HuwaDecorationEditorTab huwaDecorationTab)
                {
                    deco = huwaDecorationTab.GetDecoration();
                    return true;
                }
            }

            deco = null;
            return false;
        }
    }
}
