using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;

namespace AdvancedMimicUi
{
    public class HuwaMultiDecorationEditorTab : MultiDecorationEditorTab
    {
        private ResizeWindowHeight RWH;

        private HuwaMultiDecorationUi mainUi;

        public HuwaMultiDecorationEditorTab(ConsoleWindow window, DecorationsSubject focus, HuwaMultiDecorationUi mainUi) : base(window, focus)
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
            HuwaUi<DecorationsSubject> hUi = new HuwaUi<DecorationsSubject>(_focus);

            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment();
            screenSegment_0.SpaceAbove = 10f;

            screenSegment_0.AddInterpretter(hUi.AddButton("Add new decoration", "Add a new decoration here", I => I.AddNewDecoration()))
                .FadeOut = M.m<DecorationsSubject>(I => !I.CanAddNew());

            screenSegment_0.AddInterpretter(SubjectiveToggle<DecorationsSubject>.Quick(_focus, "Hide original mesh", new ToolTip("If true, the mesh of the block we are tethering to will be hidden. Only works for static blocks.", 200f),
                (DecorationsSubject I, bool b) =>
                {
                    if (I.Decorations.Count == 0) I.AddNewDecoration();
                    I.Decorations[0].SetHideOriginalMesh(b);
                },
                (DecorationsSubject I) =>
                {
                    if (I.Decorations.Count == 0) return false;
                    return I.Decorations[0].GetHideOriginalMesh();
                }));

            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.SpaceAbove = 20f;

            screenSegment_1.AddInterpretter(hUi.AddButton("Delete all", "Delete all the decorations at this point", I => I.DeleteAll()))
                .FadeOut = M.m<DecorationsSubject>(I => I.Decorations.Count == 0);

            screenSegment_1.AddInterpretter(hUi.AddButton("Copy all", "Copy the decorations listed here to clipboard", I => I.CopyAll()))
                .FadeOut = M.m<DecorationsSubject>(I => I.Decorations.Count == 0);

            screenSegment_1.AddInterpretter(hUi.AddButton("Paste all", "Paste in new decorations from theclipboard", I => I.PasteIn()))
                .FadeOut = M.m<DecorationsSubject>(I => !I.ReadyToPaste() || !I.CanAddNew());



            ScreenSegmentStandard screenSegment_2 = CreateStandardSegment();
            screenSegment_2.SpaceAbove = 20f;

            screenSegment_2.AddInterpretter(hUi.AddButton("Apply with mirror", "Copy and flip these decorations onto the other side of your mirror", I => I.MirrorIt()))
                .FadeOut = M.m<DecorationsSubject>(I => !I.MirroringEnabled() || I.Decorations.Count == 0);

            screenSegment_2.AddInterpretter(new Blank(20f));

            screenSegment_2.AddInterpretter(hUi.AddButton("Advanced positioning", "Operate the position, size, and angle of all decorations currently being adjusted at once",
                (DecorationsSubject I) =>
                {
                    if (I.Decorations.Count != 0)
                    {
                        new HuwaDecorationAdvPositioningUi(_focus).ActivateGui(GuiActivateType.Stack); ;
                        mainUi.DeactivateGui(GuiDeactivateType.Standard);
                    }
                }))
                .FadeOut = M.m<DecorationsSubject>(I => I.Decorations.Count == 0);

            screenSegment_2.AddInterpretter(new Blank(40f));

            SubjectiveFloatClampedWithNub<DecorationsSubject> subjectiveFloatClampedWithNub = screenSegment_2.AddInterpretter(new SubjectiveFloatClampedWithNub<DecorationsSubject>(M.m<DecorationsSubject>(-1f), M.m<DecorationsSubject>(31f), M.m<DecorationsSubject>(I => I.MassPaintIndex), M.m<DecorationsSubject>(1f), _focus, M.m(
                (DecorationsSubject I) =>
                {
                    if (I.MassPaintIndex == -1)
                    {
                        return _locFile.Get("MassPaintInactive", "Paint all (caution)", true);
                    }

                    return _locFile.Format("MassPaint", "All decorations painted index {0}", new object[] { I.MassPaintIndex });
                }),
                (I, i) => I.MassPaint((int)i), null, M.m<DecorationsSubject>(new ToolTip(_locFile.Get("MassPaintTip", "Paint all decorations in the current selection this color. <<Changes will take place once you move the slider. Resetting the slider to -1 will put back the colors that were assigned before the slider was moved>>.", true)))));
            HuwaUi.ABar_AddMember_Access(subjectiveFloatClampedWithNub).FastWidthOptimization = true;
        }
    }
}
