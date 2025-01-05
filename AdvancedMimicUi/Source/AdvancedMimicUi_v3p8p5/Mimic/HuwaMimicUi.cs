using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;

namespace AdvancedMimicUi
{
    public class HuwaMimicUi : MimicUi
    {
        private ConsoleWindow _consoleWindow_0;

        private ConsoleWindow _consoleWindow_1;

        public HuwaMimicUi(Mimic focus) : base(focus)
        {
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            _consoleWindow_0 = NewWindow(0, "Adjust position, scale and orientation", new ScaledRectangle(10f, 10f, 600f, 0f));
            _consoleWindow_0.DisplayTextPrompt = false;
            _consoleWindow_0.SetMultipleTabs(new HuwaMimicEditor(_consoleWindow_0, _focus, this));

            _consoleWindow_1 = NewWindow(2, "Other", new ScaledRectangle(670f, 10f, 600f, 400f));
            _consoleWindow_1.DisplayTextPrompt = false;
            _consoleWindow_1.MinimumWindowHeight = new PixelSizing(35f, Dimension.Height);
            _consoleWindow_1.SetMultipleTabs
            (
                new HuwaMimicMeshSelect(_consoleWindow_1, _focus),
                new HuwaMimicOffsetTab(_consoleWindow_1, _focus),
                new HuwaCalculatorTab(_consoleWindow_1),
                new HuwaMimicTriangleGeneratorTab(_consoleWindow_1, _focus),
                new HuwaMimicSettingsTab(_consoleWindow_1, this, _focus)
            );

            return _consoleWindow_0;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            HuwaTriangleGeneratorTab.CanvasSetActive(_consoleWindow_1.Screen is HuwaTriangleGeneratorTab);
        }
    }
}
