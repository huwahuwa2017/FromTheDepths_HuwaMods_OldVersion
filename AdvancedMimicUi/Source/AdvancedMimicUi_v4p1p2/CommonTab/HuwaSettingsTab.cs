using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaSettingsTab : SuperScreen<int>
    {
        public static float AddPosition { get; private set; } = 1f;

        public static float AddScale { get; private set; } = 1f;

        public static float AddAngle { get; private set; } = 45f;

        public static bool SearchName { get; private set; } = true;

        public static bool SearchRuntimeId { get; private set; } = false;

        public static bool SearchGuid { get; private set; } = false;

        public static bool ViewHistory { get; private set; } = true;

        public override Content Name
        {
            get
            {
                return new Content("Settings", new ToolTip("You can change the settings related to AdvancedMimicUi."), "SettingsTab");
            }
        }

        public HuwaSettingsTab(ConsoleWindow window) : base(window, default)
        {
        }

        public override void Build()
        {
            CreateHeader("Amount of increase in the add button", new ToolTip("You can change the amount of increase of the add button"));

            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment();
            screenSegment_0.AddInterpretter(SimpleFloatInput<int>.Quick(default, M.m<int>(d => AddPosition), new ToolTip("Amount of increase in the add button"), (d, I) => AddPosition = Mathf.Max(0f, I), M.m<int>(d => "Amount of increase (Position)")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<int>.Quick(default, M.m<int>(d => AddScale), new ToolTip("Amount of increase in the add button"), (d, I) => AddScale = Mathf.Max(0f, I), M.m<int>(d => "Amount of increase (Scale)")));
            screenSegment_0.AddInterpretter(SimpleFloatInput<int>.Quick(default, M.m<int>(d => AddAngle), new ToolTip("Amount of increase in the add button"), (d, I) => AddAngle = Mathf.Max(0f, I), M.m<int>(d => "Amount of increase (Angle)")));



            CreateHeader("Pick a mesh tab settings", new ToolTip("Settings related to 'Pick a mesh tab'"));

            ScreenSegmentStandard screenSegment_4 = CreateStandardSegment();
            screenSegment_4.AddInterpretter(SubjectiveToggle<int>.Quick(default, "ViewHistory", new ToolTip("Shows or hides the history of Mesh selection"), (d, I) => ViewHistory = I, d => ViewHistory));



            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.AddInterpretter(SubjectiveToggle<int>.Quick(default, "Name", new ToolTip("Search for a Name"), (d, I) => SearchName = I, d => SearchName));
            screenSegment_1.AddInterpretter(SubjectiveToggle<int>.Quick(default, "RuntimeId", new ToolTip("Search for a RuntimeId"), (d, I) => SearchRuntimeId = I, d => SearchRuntimeId));
            screenSegment_1.AddInterpretter(SubjectiveToggle<int>.Quick(default, "Guid", new ToolTip("Search for a Guid"), (d, I) => SearchGuid = I, d => SearchGuid));
        }
    }
}
