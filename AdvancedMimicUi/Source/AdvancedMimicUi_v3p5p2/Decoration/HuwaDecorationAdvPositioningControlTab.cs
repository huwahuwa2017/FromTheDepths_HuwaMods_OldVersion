using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Layouts.DropDowns;
using BrilliantSkies.Ui.Tips;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaDecorationAdvPositioningControlTab : SuperScreen<DecorationsSubject>
    {
        private HuwaDecorationAdvPositioningUi mainUi;

        public override Content Name
        {
            get
            {
                return new Content("Advanced positioning control", new ToolTip(""), "HuwaDecorationAdvPositioningControlTab");
            }
        }

        public HuwaDecorationAdvPositioningControlTab(ConsoleWindow window, DecorationsSubject focus, HuwaDecorationAdvPositioningUi mainUi) : base(window, focus)
        {
            this.mainUi = mainUi;
        }

        public override void Build()
        {
            ScreenSegmentStandardHorizontal screenSegment_0 = CreateStandardHorizontalSegment();
            screenSegment_0.SpaceAbove = 10f;

            screenSegment_0.AddInterpretter(SubjectiveDisplay<int>.Quick(default, M.m<int>("Rotation origin  :"))).Justify = TextAnchor.UpperRight;

            DropDownMenuAlt<Decoration> dropDownMenuAlt = new DropDownMenuAlt<Decoration>();
            List<DropDownMenuAltItem<Decoration>> dropDownItemList = new List<DropDownMenuAltItem<Decoration>> { new DropDownMenuAltItem<Decoration>() { Name = "Default", ObjectForAction = null } };
            dropDownItemList.AddRange(_focus.Decorations.Select((deco, index) => new DropDownMenuAltItem<Decoration>() { Name = "Decoration No." + index, ObjectForAction = deco }));
            dropDownMenuAlt.SetItems(dropDownItemList.ToArray());

            screenSegment_0.AddInterpretter(new DropDown<int, Decoration>(default, dropDownMenuAlt, (int _, Decoration I) => mainUi.SacrificeSelectDecoration == I, (int _, Decoration I) => mainUi.SacrificeSelectDecoration = I));



            ScreenSegmentStandard screenSegment_1 = CreateStandardSegment();
            screenSegment_1.SpaceAbove = 10f;
            screenSegment_1.SpaceBelow = 20f;

            screenSegment_1.AddInterpretter(SubjectiveButton<int>.Quick(default, "Cancel the position", new ToolTip("Cancels the position based on the selected decoration"), _ => mainUi.PositionReset()))
                .FadeOut = M.m<int>(_ => mainUi.SacrificeSelectDecoration == null);

            screenSegment_1.AddInterpretter(SubjectiveButton<int>.Quick(default, "Cancel the rotation", new ToolTip("Cancels the rotation based on the selected decoration"), _ => mainUi.RotationReset()))
                .FadeOut = M.m<int>(_ => mainUi.SacrificeSelectDecoration == null);
        }
    }
}
