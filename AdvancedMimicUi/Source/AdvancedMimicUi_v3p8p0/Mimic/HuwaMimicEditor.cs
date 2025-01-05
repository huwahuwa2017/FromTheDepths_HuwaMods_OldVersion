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
        private HuwaMimicUi huwaMimicUi;

        private ResizeWindowHeight RWH;

        public override Content Name
        {
            get
            {
                return new Content("Adjust transfrom", new ToolTip("Adjusts the position, scale and orientatation of the mesh that will be rendered."), "HuwaMimicUiTab_1");
            }
        }

        public HuwaMimicEditor(ConsoleWindow window, Mimic focus, HuwaMimicUi huwaMimicUi) : base(window, focus)
        {
            this.huwaMimicUi = huwaMimicUi;
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
            HuwaUi<Mimic> hUi_1 = new HuwaUi<Mimic>(_focus);

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(5, 9);
            screenSegment_0.SpaceAbove = 10f;
            screenSegment_0.SpaceBelow = 10f;
            screenSegment_0.SqueezeTable = true;

            MimicAndDecoration_CommonUi.BarsUiGeneration(screenSegment_0, hUi_0);



            ScreenSegmentTable screenSegment_1 = CreateTableSegment(3, 1);
            screenSegment_1.SpaceBelow = 10f;
            screenSegment_1.SqueezeTable = false;

            screenSegment_1.AddInterpretter(hUi_0.AddButton("X axis flip", "Reverses the position and angle based on the X axis.", HuwaMimicHelp.XAxisFlip));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Y axis flip", "Reverses the position and angle based on the Y axis.", HuwaMimicHelp.YAxisFlip));
            screenSegment_1.AddInterpretter(hUi_0.AddButton("Z axis flip", "Reverses the position and angle based on the Z axis.", HuwaMimicHelp.ZAxisFlip));



            ScreenSegmentTable screenSegment_2 = CreateTableSegment(4, 1);
            screenSegment_2.SqueezeTable = false;

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
                        HuwaMimicHelp.XAxisFlip(Tdata);

                        //Tdata.MeshGuid.Us = HuwaMimicHelp.GetMirrorMesh(Tdata.MeshGuid.Us);
                    }
                });
            subjectiveButton_3.FadeOut = M.m<Mimic>(I => GetTargetMimic() == null);

            screenSegment_2.AddInterpretter(subjectiveButton_0);
            screenSegment_2.AddInterpretter(subjectiveButton_1);
            screenSegment_2.AddInterpretter(subjectiveButton_2);
            screenSegment_2.AddInterpretter(subjectiveButton_3);



            ScreenSegmentStandardHorizontal screenSegment_3 = CreateStandardHorizontalSegment();

            screenSegment_3.AddInterpretter(FtdBuildPanel.GetWireframeToggle());
            screenSegment_3.AddInterpretter(Quick.Toggle(_focus.Data, (MimicData I) => "HideTooltip"));
        }

        private Mimic GetTargetMimic()
        {
            cBuild cBuildMe = cBuild.GetSingleton();

            if (cBuildMe != null && cBuildMe.buildMarker != null)
            {
                if (HuwaMimicHelp.GetMirrorStuff(out MirrorStuff mirrorStuff) && mirrorStuff.MirrorMode != MirrorMode.off)
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

                huwaMimicUi.DeactivateGui(GuiDeactivateType.Standard);
            }
        }
    }
}
