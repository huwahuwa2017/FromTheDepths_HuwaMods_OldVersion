using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;

namespace AdvancedMimicUi
{
    public class HuwaMimicUi : MimicUi
    {
        private ConsoleWindow consoleWindow_0;

        private ConsoleWindow consoleWindow_1;

        public HuwaMimicUi(Mimic focus) : base(focus)
        {
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            consoleWindow_0 = NewWindow(0, "Adjust position, scale and orientation", new ScaledRectangle(10f, 10f, 600f, 0f));
            consoleWindow_0.DisplayTextPrompt = false;
            consoleWindow_0.SetMultipleTabs(new HuwaMimicEditor(consoleWindow_0, _focus, this));

            consoleWindow_1 = NewWindow(2, "Other", new ScaledRectangle(670f, 10f, 600f, 400f));
            consoleWindow_1.DisplayTextPrompt = false;
            consoleWindow_1.MinimumWindowHeight = new PixelSizing(35f, Dimension.Height);
            consoleWindow_1.SetMultipleTabs(new HuwaMimicMeshSelect(consoleWindow_1, _focus), new HuwaMimicOffsetTab(consoleWindow_1, _focus), new CalculationSuperScreen(consoleWindow_1), new HuwaMimicTriangleGeneratorTab(consoleWindow_1, _focus), new HuwaMimicSettingsTab(consoleWindow_1, this, _focus));

            return consoleWindow_0;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            TriangleGeneratorSuperScreen.CanvasSetActive(consoleWindow_1.Screen is TriangleGeneratorSuperScreen);
        }
    }
}
