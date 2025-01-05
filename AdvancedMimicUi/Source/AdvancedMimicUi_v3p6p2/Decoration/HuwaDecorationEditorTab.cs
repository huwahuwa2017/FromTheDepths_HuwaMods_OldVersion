﻿using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Segments;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaDecorationEditorTab : DecorationTab
    {
        private ResizeWindowHeight RWH;

        public HuwaDecorationEditorTab(ConsoleWindow window, Decoration focus, int index = -1) : base(window, focus, index)
        {
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            RWH = RWH ?? new ResizeWindowHeight(Window);
            RWH.Update();
        }

        public override void Build()
        {
            MimicAndDecorationCommonData MAD_CD = new MimicAndDecorationCommonData(_focus);
            HuwaUi<MimicAndDecorationCommonData> hUi_0 = new HuwaUi<MimicAndDecorationCommonData>(MAD_CD);
            HuwaUi<Decoration> hUi_1 = new HuwaUi<Decoration>(_focus);

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(5, 9);
            screenSegment_0.SpaceAbove = 10f;

            MimicAndDecoration_CommonUi.BarsUiGeneration(screenSegment_0, hUi_0);

            /*
            SubjectiveFloatClampedWithBar<Var<int>> subjectiveFloatClampedWithBar = Quick.SliderInt(_focus, (I) => "Color");
            HuwaUi.ABar_AddMember_Access(subjectiveFloatClampedWithBar).FastWidthOptimization = true;

            screenSegment_0.AddInterpretter(subjectiveFloatClampedWithBar);
            screenSegment_0.AddInterpretter(new Empty());
            screenSegment_0.AddInterpretter(new Empty());
            screenSegment_0.AddInterpretter(new Empty());
            screenSegment_0.AddInterpretter(new Empty());
            */

            MimicHelp.BuildPaintColorPicker(this, _focus.ConstructableColors, () => _focus.Color.Us,
                (int I) =>
                {
                    _focus.Color.Us = I;
                });



            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.SpaceAbove = 10f;

            screenSegment_1.AddInterpretter(hUi_0.AddButton("X axis flip", "Reverses the position and angle based on the X axis.", HuwaMimicHelp.XAxisFlip));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Y axis flip", "Reverses the position and angle based on the Y axis.", HuwaMimicHelp.YAxisFlip));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Z axis flip", "Reverses the position and angle based on the Z axis.", HuwaMimicHelp.ZAxisFlip));



            ScreenSegmentStandardHorizontal screenSegment_2 = CreateStandardHorizontalSegment();
            screenSegment_2.SpaceAbove = 10f;

            screenSegment_2.AddInterpretter(hUi_1.AddButton("Delete decoration", "Permanently delete this decoration", DecorationDelete));
            screenSegment_2.AddInterpretter(hUi_1.AddButton("Copy", "Copy the settings of this decoration", I => I.Copy()));
            screenSegment_2.AddInterpretter(hUi_1.AddButton("Paste", "Paste the settings of this decoration", I => I.Paste()));
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
