using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Examples.OptionsMenu;
using BrilliantSkies.Ui.Tips;

namespace AdvancedMimicUi
{
    public class HuwaMultiDecorationEditorTab : MultiDecorationEditorTab
    {
        private ResizeWindowHeight _rwh;

        private HuwaMultiDecorationUi _mainUi;

        public HuwaMultiDecorationEditorTab(ConsoleWindow window, DecorationsSubject focus, HuwaMultiDecorationUi mainUi) : base(window, focus)
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
            HuwaUi<DecorationsSubject> hUi = new HuwaUi<DecorationsSubject>(_focus);



            SubjectiveButton<DecorationsSubject> subjectiveButton_0 = hUi.AddButton("Add new decoration", "Add a new decoration here", I => I.AddNewDecoration());
            subjectiveButton_0.FadeOut = M.m<DecorationsSubject>(I => !I.CanAddNew());

            ScreenSegmentStandard screenSegment_4 = CreateStandardSegment();
            screenSegment_4.SpaceAbove = 20f;
            screenSegment_4.AddInterpretter(subjectiveButton_0);



            SubjectiveButton<DecorationsSubject> subjectiveButton_1 = hUi.AddButton("Delete all", "Delete all the decorations at this point", I => I.DeleteAll());
            subjectiveButton_1.FadeOut = M.m<DecorationsSubject>(I => I.Decorations.Count == 0);

            SubjectiveButton<DecorationsSubject> subjectiveButton_2 = hUi.AddButton("Copy all", "Copy the decorations listed here to clipboard", I => I.CopyAll());
            subjectiveButton_2.FadeOut = M.m<DecorationsSubject>(I => I.Decorations.Count == 0);

            SubjectiveButton<DecorationsSubject> subjectiveButton_3 = hUi.AddButton("Paste all", "Paste in new decorations from theclipboard", I => I.PasteIn());
            subjectiveButton_3.FadeOut = M.m<DecorationsSubject>(I => !I.ReadyToPaste() || !I.CanAddNew());

            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.SpaceAbove = 20f;
            screenSegment_1.AddInterpretter(subjectiveButton_1);
            screenSegment_1.AddInterpretter(subjectiveButton_2);
            screenSegment_1.AddInterpretter(subjectiveButton_3);



            SubjectiveButton<DecorationsSubject> subjectiveButton_4 = hUi.AddButton("Apply with mirror", "Copy and flip these decorations onto the other side of your mirror", I => I.MirrorIt());
            subjectiveButton_4.FadeOut = M.m<DecorationsSubject>(I => !I.MirroringEnabled() || I.Decorations.Count == 0);

            SubjectiveButton<DecorationsSubject> subjectiveButton_5 = hUi.AddButton("Advanced positioning", "Operate the position, size, and angle of all decorations currently being adjusted at once",
                (DecorationsSubject I) =>
                {
                    if (I.Decorations.Count != 0)
                    {
                        new HuwaDecorationAdvPositioningUi(_focus).ActivateGui(GuiActivateType.Stack); ;
                        _mainUi.DeactivateGui(GuiDeactivateType.Standard);
                    }
                });
            subjectiveButton_5.FadeOut = M.m<DecorationsSubject>(I => I.Decorations.Count == 0);

            ScreenSegmentStandard screenSegment_2 = CreateStandardSegment();
            screenSegment_2.SpaceAbove = 20f;
            screenSegment_2.AddInterpretter(subjectiveButton_4);
            screenSegment_2.AddInterpretter(new Blank(20f));
            screenSegment_2.AddInterpretter(subjectiveButton_5);



            ToolTip toolTip_0 = new ToolTip(DecorationTab._locFile.Get("Toggle_HideOriginalMesh_Tip", "If true, the mesh of the block we are tethering to will be hidden. Only works for static blocks."));
            SubjectiveToggle<DecorationsSubject> subjectiveToggle_0 = SubjectiveToggle<DecorationsSubject>.Quick(_focus, DecorationTab._locFile.Get("Toggle_HideOriginalMesh", "Hide original mesh"), toolTip_0,
                (DecorationsSubject I, bool b) =>
                {
                    if (I.Decorations.Count == 0) I.AddNewDecoration();
                    I.Decorations[0].SetHideOriginalMesh(b);
                },
                (DecorationsSubject I) =>
                {
                    if (I.Decorations.Count == 0) return false;
                    return I.Decorations[0].GetHideOriginalMesh();
                });

            ScreenSegmentStandardHorizontal screenSegment_0 = CreateStandardHorizontalSegment();
            screenSegment_0.SpaceAbove = 20f;
            screenSegment_0.SpaceBelow = 20f;
            screenSegment_0.AddInterpretter(subjectiveToggle_0);
            screenSegment_0.AddInterpretter(FtdBuildPanel.GetWireframeToggle());



            ScreenSegmentHeader screenSegment_5 = CreateHeader(_locFile.Get("Header_MassPaint", "Mass paint"), new ToolTip(_locFile.Get("Header_MassPaint_Tip", "Select a color to paint all attached decorations")));
            screenSegment_5.SetConditionalDisplay(() => _focus.Decorations.Count > 0);

            ScreenSegmentTable screenSegment_6 = MimicHelp.BuildPaintColorPicker(this, _focus.ConstructColors, GetMassPaintColor, SetMassPaintColor);
            screenSegment_6.SetConditionalDisplay(() => _focus.Decorations.Count > 0);



            /*
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
            */
        }

        private int GetMassPaintColor()
        {
            int colorIndex = _focus.Decorations[0].Color.Us;

            foreach (Decoration deco in _focus.Decorations)
            {
                if (deco.Color.Us != colorIndex)
                {
                    return -1;
                }
            }

            return colorIndex;
        }

        private void SetMassPaintColor(int colorIndex)
        {
            foreach (Decoration deco in _focus.Decorations)
            {
                deco.Color.Us = colorIndex;
            }
        }
    }
}
