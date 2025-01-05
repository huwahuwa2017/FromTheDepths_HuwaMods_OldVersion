using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaDecorationMassPositioningTab : SuperScreen<DecorationsSubject>
    {
        private ResizeWindowHeight _rwh;

        private HuwaDecorationMassPositioningUi _mainUi;

        public override Content Name
        {
            get
            {
                return new Content("HuwaDecorationMassPositioning", new ToolTip(""), "HuwaDecorationMassPositioning");
            }
        }

        public HuwaDecorationMassPositioningTab(ConsoleWindow window, DecorationsSubject focus, HuwaDecorationMassPositioningUi mainUi) : base(window, focus)
        {
            _mainUi = mainUi;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            _rwh = _rwh ?? new ResizeWindowHeight(Window);
            _rwh.Update();
        }

        public override void Build()
        {
            HuwaUi<MimicAndDecorationCommonData> hUi_0 = new HuwaUi<MimicAndDecorationCommonData>(_mainUi.SacrificeMAD_Data);

            ScreenSegmentTable screenSegment_0 = MimicAndDecoration_CommonUi.BarsUiGeneration(this, hUi_0, true);
            screenSegment_0.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.SpaceAbove = 40f;

            screenSegment_1.AddInterpretter(hUi_0.AddButton("Return", "Exit without saving",
                (MimicAndDecorationCommonData I) =>
                {
                    new HuwaMultiDecorationUi(_focus).ActivateGui(GuiActivateType.Stack);
                    _mainUi.DeactivateGui(GuiDeactivateType.Standard);
                }));

            screenSegment_1.AddInterpretter(new Blank(10f));

            screenSegment_1.AddInterpretter(hUi_0.AddButton("Apply", "Save and exit",
                (MimicAndDecorationCommonData I) =>
                {
                    _mainUi.UndoReservation = false;
                    new HuwaMultiDecorationUi(_focus).ActivateGui(GuiActivateType.Stack);
                    _mainUi.DeactivateGui(GuiDeactivateType.Standard);
                }));
        }
    }
}
