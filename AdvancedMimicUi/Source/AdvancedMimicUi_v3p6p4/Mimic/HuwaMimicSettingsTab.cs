using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedMimicUi
{
    public class HuwaMimicSettingsTab : SettingsSuperScreen
    {
        private Mimic mimic;

        private HuwaMimicUi huwaMimicUi;

        public HuwaMimicSettingsTab(ConsoleWindow window, HuwaMimicUi huwaMimicUi, Mimic mimic) : base(window)
        {
            this.mimic = mimic;
            this.huwaMimicUi = huwaMimicUi;
        }

        public override void Build()
        {
            base.Build();

            CreateHeader("Mimic Settings");

            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment();

            screenSegment_0.AddInterpretter(Button.Quick("All mimic to decoration", new ToolTip("Replace all Mimic with Decoration."), AllMimicToDecoration));
        }

        private void AllMimicToDecoration()
        {
            List<AllConstruct> allConstructList = new List<AllConstruct>();
            List<Mimic> mimicList = new List<Mimic>();

            MainConstruct mainConstruct = mimic.MainConstruct as MainConstruct;

            allConstructList.Add(mainConstruct);
            allConstructList.AddRange(mainConstruct.AllSubConstructsPerIndex.OfType<AllConstruct>());

            foreach (AllConstruct allConstruct in allConstructList)
            {
                if (allConstruct != null)
                {
                    mimicList.AddRange(allConstruct.AllBasics.AliveAndDead.Blocks.OfType<Mimic>());
                }
            }

            foreach (Mimic mimic in mimicList)
            {
                Decoration decoration = mimic.GetConstructableOrSubConstructable().DecorationsRestricted.NewDecoration(mimic.LocalPosition, false);

                if (decoration != null)
                {
                    bool flag2 = mimic.Data.MeshGuid.Us != Guid.Empty;
                    if (flag2)
                    {
                        decoration.MeshGuid.Us = mimic.Data.MeshGuid.Us;
                    }
                    decoration.Positioning.Us = mimic.LocalRotation * mimic.Data.Positioning.Us;
                    decoration.Scaling.Us = mimic.Data.Scaling.Us;
                    decoration.Orientation.Us = (mimic.LocalRotation * mimic.Data.OrientationQuaternion).eulerAngles;
                    decoration.Color.Us = mimic.color;
                    mimic.MainConstruct.ConnectionRulesRestricted.RequestRuleDeactivation();
                    mimic.SeparateBlock(null, false, false);
                }
            }

            huwaMimicUi.DeactivateGui(GuiDeactivateType.Standard);
        }
    }
}
