using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;
using ES2_PolygonControl;

namespace AdvancedMimicUi
{
    public class HuwaDecorationAdvPositioningTab : SuperScreen<DecorationsSubject>
    {
        private ResizeWindowHeight RWH;

        private HuwaDecorationAdvPositioningUi mainUi;

        public override Content Name
        {
            get
            {
                return new Content("HuwaDecorationAdvancedPositioning", new ToolTip(""), "HuwaDecorationAdvancedPositioning");
            }
        }

        public HuwaDecorationAdvPositioningTab(ConsoleWindow window, DecorationsSubject focus, HuwaDecorationAdvPositioningUi mainUi) : base(window, focus)
        {
            this.mainUi = mainUi;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            RWH = RWH ?? new ResizeWindowHeight(Window);
            RWH.Update();
        }

        public override void Build()
        {
            HuwaUi<MimicAndDecoration_CommonData> hUi_0 = new HuwaUi<MimicAndDecoration_CommonData>(mainUi.SacrificeMAD_Data);

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(5, 6);
            screenSegment_0.SpaceAbove = 10f;

            MimicAndDecoration_CommonUi.BarsUiGeneration(screenSegment_0, hUi_0, true);



            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.SpaceAbove = 40f;
            screenSegment_1.SpaceBelow = 20f;

            screenSegment_1.AddInterpretter(hUi_0.AddButton("Return", "Exit without saving",
                (MimicAndDecoration_CommonData I) =>
                {
                    new HuwaMultiDecorationUi(_focus).ActivateGui(GuiActivateType.Stack);
                    mainUi.DeactivateGui(GuiDeactivateType.Standard);
                }));

            screenSegment_1.AddInterpretter(new Blank(10f));

            screenSegment_1.AddInterpretter(hUi_0.AddButton("Apply", "Save and exit",
                (MimicAndDecoration_CommonData I) =>
                {
                    mainUi.UndoReservation = false;
                    new HuwaMultiDecorationUi(_focus).ActivateGui(GuiActivateType.Stack);
                    mainUi.DeactivateGui(GuiDeactivateType.Standard);
                }));
        }
    }
}
