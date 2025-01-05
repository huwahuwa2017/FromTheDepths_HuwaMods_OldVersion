using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedMimicUi
{
    public class HuwaMultiDecorationUi : MultiDecorationEditor
    {
        private ConsoleWindow consoleWindow_1;

        private ConsoleWindow consoleWindow_0
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
            consoleWindow_0 = NewWindow(0, _locFile.Get("MultiDecWindow", "Decorations editor", true), new ScaledRectangle(10f, 10f, 600f, 0f));
            consoleWindow_0.DisplayTextPrompt = false;
            List<SuperScreen> superScreenList = new List<SuperScreen>();
            superScreenList.Add(new HuwaMultiDecorationEditorTab(consoleWindow_0, _focus, this));
            superScreenList.AddRange(_focus.Decorations.Select((d, i) => new HuwaDecorationEditorTab(consoleWindow_0, d, i)));
            consoleWindow_0.SetMultipleTabs(superScreenList.ToArray());

            consoleWindow_1 = NewWindow(2, "Other", new ScaledRectangle(670f, 10f, 600f, 400f));
            consoleWindow_1.DisplayTextPrompt = false;
            consoleWindow_1.MinimumWindowHeight = new PixelSizing(35f, Dimension.Height);
            consoleWindow_1.SetMultipleTabs(new HuwaDecorationMeshSelectTab(consoleWindow_1, this), new HuwaDecorationPositioningTab(consoleWindow_1, this), new CalculationSuperScreen(consoleWindow_1), new HuwaDecorationTriangleGeneratorTab(consoleWindow_1, this), new HuwaDecorationSettingsTab(consoleWindow_1, _focus));

            return consoleWindow_0;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            TriangleGeneratorSuperScreen.CanvasSetActive(consoleWindow_1.Screen is TriangleGeneratorSuperScreen);
        }

        public DecorationsSubject GetDecorationsSubject()
        {
            return _focus;
        }

        public bool GetDecoration(out Decoration deco)
        {
            if (consoleWindow_0 != null)
            {
                ConsoleUiScreen screen = consoleWindow_0.Screen;

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
