using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Segments;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaDecorationEditorTab : DecorationTab
    {
        private ResizeWindowHeight _rwh;

        public HuwaDecorationEditorTab(ConsoleWindow window, Decoration focus, int index = -1) : base(window, focus, index)
        {
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            _rwh = _rwh ?? new ResizeWindowHeight(Window);
            _rwh.Update();
        }

        public override void Build()
        {
            MimicAndDecorationCommonData MAD_CD = new MimicAndDecorationCommonData(_focus);
            HuwaUi<MimicAndDecorationCommonData> hUi_0 = new HuwaUi<MimicAndDecorationCommonData>(MAD_CD);
            HuwaUi<Decoration> hUi_1 = new HuwaUi<Decoration>(_focus);

            ScreenSegmentTable screenSegment_0 = MimicAndDecoration_CommonUi.BarsUiGeneration(this, hUi_0);
            screenSegment_0.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_1 = MimicAndDecoration_CommonUi.MaterialReplacementDropDown(this, new MimicAndDecorationCommonData(_focus));
            screenSegment_1.SpaceAbove = 10f;

            ScreenSegmentTable screenSegment_2 = MimicHelp.BuildPaintColorPicker(this, _focus.ConstructableColors, () => _focus.Color.Us, I => _focus.Color.Us = I);
            screenSegment_2.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_3 = MimicAndDecoration_CommonUi.AxisFlip(this, hUi_0, HuwaHelpFunctions.XAxisFlip, HuwaHelpFunctions.YAxisFlip, HuwaHelpFunctions.ZAxisFlip);
            screenSegment_3.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_4 = CreateStandardHorizontalSegment();
            screenSegment_4.SpaceAbove = 10f;

            screenSegment_4.AddInterpretter(hUi_1.AddButton("Delete", "Permanently delete this decoration", DecorationDelete));
            screenSegment_4.AddInterpretter(hUi_1.AddButton("Copy", "Copy the settings of this decoration", I => I.Copy()));
            screenSegment_4.AddInterpretter(hUi_1.AddButton("Paste", "Paste the settings of this decoration", I => I.Paste()));
        }

        public Decoration GetDecoration()
        {
            return _focus;
        }

        private void DecorationDelete(Decoration deco)
        {
            deco.Delete();
            (Window._governingUi as IDecorationUi).DecorationDeleted(this);
        }
    }
}
