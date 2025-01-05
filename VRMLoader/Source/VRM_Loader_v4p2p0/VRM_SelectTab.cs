using BrilliantSkies.Ftd.Avatar.Skills;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using System.IO;

namespace VRM_Loader
{
    public class VRM_SelectTab : SuperScreen<CharacterSheet>
    {
        public VRM_SelectTab(ConsoleWindow window, CharacterSheet focus) : base(window, focus)
        {
            this.Name = new Content("VRM_Loader", new ToolTip("Select a character model.", 200f));
        }

        public override void Build()
        {
            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment(InsertPosition.OnCursor);

            string text = @"Please put the VRM format file in the following folder
                C:\Users\UserName\Documents\From The Depths\Mods\VRMLoader\VRM";

            screenSegment_0.AddInterpretter(SubjectiveDisplay<CharacterSheet>.Quick(_focus, M.m((CharacterSheet c) => text)));

            screenSegment_0.AddInterpretter(SubjectiveButton<CharacterSheet>.Quick(_focus, "DefaultAvatar", new ToolTip("Show DefaultAvatar"),
                (CharacterSheet c) =>
                {
                    CharacterReplacement.Replacement("DefaultAvatar");
                }));

            screenSegment_0.AddInterpretter(SubjectiveButton<CharacterSheet>.Quick(_focus, "Transparent", new ToolTip("Show nothing"),
                (CharacterSheet c) =>
                {
                    CharacterReplacement.Replacement("Transparent");
                }));

            screenSegment_0.AddInterpretter(new Empty());

            string[] vrmFiles = CharacterReplacement.VRM_FolderConfirmation();

            foreach (string fileName in vrmFiles)
            {
                screenSegment_0.AddInterpretter(SubjectiveButton<CharacterSheet>.Quick(_focus, Path.GetFileName(fileName), new ToolTip("Read the VRM file"),
                    (CharacterSheet c) =>
                    {
                        CharacterReplacement.Replacement(fileName);
                    }));
            }
        }
    }
}
