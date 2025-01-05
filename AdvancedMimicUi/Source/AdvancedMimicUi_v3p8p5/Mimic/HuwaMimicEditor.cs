using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Core.Serialisation.Parameters.Prototypes;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd.Avatar.Build;
using BrilliantSkies.Ftd.Avatar.Build.Marker;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Examples.OptionsMenu;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;
using System;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaMimicEditor : SuperScreen<Mimic>
    {
        private HuwaMimicUi _huwaMimicUi;

        private ResizeWindowHeight _rwh;

        public override Content Name
        {
            get
            {
                return new Content("Adjust transfrom", new ToolTip("Adjusts the position, scale and orientatation of the mesh that will be rendered."), "HuwaMimicUiTab_1");
            }
        }

        public HuwaMimicEditor(ConsoleWindow window, Mimic focus, HuwaMimicUi huwaMimicUi) : base(window, focus)
        {
            _huwaMimicUi = huwaMimicUi;
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
            HuwaUi<Mimic> hUi_1 = new HuwaUi<Mimic>(_focus);

            ScreenSegmentTable screenSegment_0 = MimicAndDecoration_CommonUi.BarsUiGeneration(this, hUi_0);
            screenSegment_0.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_1 = MimicAndDecoration_CommonUi.MaterialReplacementDropDown(this, new MimicAndDecorationCommonData(_focus));
            screenSegment_1.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_2 = MimicAndDecoration_CommonUi.AxisFlip(this, hUi_0, HuwaHelpFunctions.XAxisFlip, HuwaHelpFunctions.YAxisFlip, HuwaHelpFunctions.ZAxisFlip);
            screenSegment_2.SpaceAbove = 10f;

            ScreenSegmentStandardHorizontal screenSegment_3 = CreateStandardHorizontalSegment();
            screenSegment_3.SpaceAbove = 10f;

            SubjectiveButton<Mimic> subjectiveButton_0 = hUi_1.AddButton("Change to decoration", "Delete this mimic and make it a Decoration instead, you can then move the decoration so that it is correctly tethered to a block", SwapToDecoration);

            SubjectiveButton<Mimic> subjectiveButton_1 = hUi_1.AddButton("Copy to clipboard", "Copy all settings to clipboard", I => RootCopyPaster.Copy(I));

            SubjectiveButton<Mimic> subjectiveButton_2 = hUi_1.AddButton("Paste from clipboard", "Paste all settings from clipboard", I => RootCopyPaster.Paste(I));
            subjectiveButton_2.FadeOut = M.m<Mimic>(I => !RootCopyPaster.ReadyToPaste(I));

            SubjectiveButton<Mimic> subjectiveButton_3 = hUi_1.AddButton("Mirror data transfer", "Transfer the data to the mimic block on the other side of the mirror mode.",
                (Mimic I) =>
                {
                    Mimic targetMimic = GetTargetMimic();

                    if (targetMimic != null)
                    {
                        MimicAndDecorationCommonData Mdata = new MimicAndDecorationCommonData(_focus);
                        MimicAndDecorationCommonData Tdata = new MimicAndDecorationCommonData(targetMimic);
                        MimicAndDecorationCommonData.Copy(Mdata, Tdata);
                        HuwaHelpFunctions.XAxisFlip(Tdata);

                        //Tdata.MeshGuid.Us = HuwaMimicHelp.GetMirrorMesh(Tdata.MeshGuid.Us);
                    }
                });
            subjectiveButton_3.FadeOut = M.m<Mimic>(I => GetTargetMimic() == null);

            screenSegment_3.AddInterpretter(subjectiveButton_0);
            screenSegment_3.AddInterpretter(subjectiveButton_1);
            screenSegment_3.AddInterpretter(subjectiveButton_2);
            screenSegment_3.AddInterpretter(subjectiveButton_3);

            ScreenSegmentStandardHorizontal screenSegment_4 = CreateStandardHorizontalSegment();
            screenSegment_4.SpaceAbove = 10f;

            screenSegment_4.AddInterpretter(FtdBuildPanel.GetWireframeToggle());
            screenSegment_4.AddInterpretter(Quick.Toggle(_focus.Data, (MimicData I) => "HideTooltip"));
        }

        private Mimic GetTargetMimic()
        {
            cBuild cBuildMe = cBuild.GetSingleton();

            if (cBuildMe != null && cBuildMe.buildMarker != null)
            {
                if (HuwaHelpFunctions.GetMirrorStuff(out MirrorStuff mirrorStuff) && mirrorStuff.MirrorMode != MirrorMode.off)
                {
                    return cBuildMe.buildMarker.C.AllBasicsRestricted.GetTypeViaLocalPosition<Mimic>(GetMirroredLocalPos(mirrorStuff, _focus.LocalPosition));
                }
            }

            return null;
        }

        private Vector3i GetMirroredLocalPos(MirrorStuff mirrorStuff, Vector3 lp)
        {
            Vector3 vector3 = Vector3.zero;

            if (mirrorStuff != null)
            {
                Vector3 mirrorLocalPos = mirrorStuff.GetMirrorLocalPosition();

                switch (mirrorStuff.MirrorPlane)
                {
                    case MirrorPlane.forwardup:
                        vector3 = new Vector3(2f * (lp.x - mirrorLocalPos.x), 0f, 0f);
                        break;
                    case MirrorPlane.rightup:
                        vector3 = new Vector3(0f, 0f, 2f * (lp.z - mirrorLocalPos.z));
                        break;
                    case MirrorPlane.forwardright:
                        vector3 = new Vector3(0f, 2f * (lp.y - mirrorLocalPos.y), 0f);
                        break;
                }
            }

            return (Vector3i)(lp - vector3);
        }

        public void SwapToDecoration(Mimic mimic)
        {
            Decoration decoration = mimic.GetConstructableOrSubConstructable().DecorationsRestricted.NewDecoration(mimic.LocalPosition, false);

            if (decoration != null)
            {
                if (mimic.Data.MeshGuid.Us != Guid.Empty)
                {
                    decoration.MeshGuid.Us = mimic.Data.MeshGuid.Us;
                }

                decoration.Positioning.Us = mimic.LocalRotation * mimic.Data.Positioning.Us;
                decoration.Scaling.Us = mimic.Data.Scaling.Us;
                decoration.Orientation.Us = (mimic.LocalRotation * mimic.Data.OrientationQuaternion).eulerAngles;
                decoration.Color.Us = mimic.color;

                mimic.MainConstruct.ConnectionRulesRestricted.RequestRuleDeactivation();
                mimic.SeparateBlock(null, false, false);

                _huwaMimicUi.DeactivateGui(GuiDeactivateType.Standard);
            }
        }
    }
}
