using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Segments;

namespace AdvancedMimicUi
{
    public class HuwaDecorationSettingsTab : SettingsSuperScreen
    {
        private DecorationsSubject decorationsSubject;

        public HuwaDecorationSettingsTab(ConsoleWindow window, DecorationsSubject decorationsSubject) : base(window)
        {
            this.decorationsSubject = decorationsSubject;
        }

        public override void Build()
        {
            base.Build();

            HuwaUi<DecorationsSubject> hUi = new HuwaUi<DecorationsSubject>(decorationsSubject);

            CreateHeader("Decoration Settings");

            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment();

            screenSegment_0.AddInterpretter(hUi.AddButton(_locFile.Get("MassDeleteUntethered", "Delete all \"untethered\" decorations", true), _locFile.Get("MassDeleteUntethered_tip", "Will permanently remove any decorations that are \"untethered\" (glowing red)", true), I => I.RemoveUntethered()));
            screenSegment_0.AddInterpretter(SubjectiveDisplay<int>.Quick(default, M.m<int>(I => $"Number of decorations in the 'Construct'  :  <<{decorationsSubject.ConstructDecorations.DecorationCount} / 5000>>")));
        }
    }
}
