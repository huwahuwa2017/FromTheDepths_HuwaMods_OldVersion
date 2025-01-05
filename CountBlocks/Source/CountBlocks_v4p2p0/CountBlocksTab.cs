using BrilliantSkies.Ftd.Constructs.UI;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Containers;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Texts;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CountBlocks
{
    public class CountBlocksTab : SuperScreen<ConstructInfo>
    {
        static private string searchText = string.Empty;

        static private List<string[]> textList = new List<string[]>();

        public CountBlocksTab(ConsoleWindow window, ConstructInfo focus) : base(window, focus)
        {
            Name = new Content("Count blocks", new ToolTip("Displays the number of blocks used.", 200f));
        }

        public override void Build()
        {
            textList.Clear();

            Dictionary<ItemDefinition, int> blockCount = Configured.i.Get<ModificationComponentContainerItem>().Components.ToDictionary(I => I, I => 0);

            foreach (Block block in _focus.Construct.AllBasics.AliveAndDead.Blocks)
            {
                if (blockCount.ContainsKey(block.item))
                {
                    ++blockCount[block.item];
                }
            }

            foreach (SubConstruct subConstruct in _focus.Construct.AllSubConstructsPerIndex)
            {
                if (subConstruct != null)
                {
                    foreach (Block block in subConstruct.AllBasics.AliveAndDead.Blocks)
                    {
                        if (blockCount.ContainsKey(block.item))
                        {
                            ++blockCount[block.item];
                        }
                    }
                }
            }



            bool flag_0 = string.IsNullOrEmpty(searchText);

            string name = string.Empty;
            string count = string.Empty;

            int newlineCount = 0;

            foreach (KeyValuePair<ItemDefinition, int> keyAndValue in blockCount)
            {
                bool flag_1 = flag_0 && keyAndValue.Value == 0;
                bool flag_2 = keyAndValue.Key.ComponentId.Name.ToLower().Contains(searchText.ToLower());

                if (!flag_1 && flag_2)
                {
                    name += keyAndValue.Key.ComponentId.Name;
                    count += ":    " + keyAndValue.Value.ToString();

                    if (newlineCount < 15)
                    {
                        name += "\n";
                        count += "\n";

                        ++newlineCount;
                    }
                    else
                    {
                        textList.Add(new string[] { name, count });

                        name = string.Empty;
                        count = string.Empty;

                        newlineCount = 0;
                    }
                }
            }

            if (name != string.Empty)
            {
                textList.Add(new string[] { name, count });
            }



            ScreenSegmentStandard ScreenSegment_0 = CreateStandardSegment(InsertPosition.OnCursor);
            ScreenSegment_0.SpaceAbove = 30f;

            ScreenSegment_0.AddInterpretter(TextInput<ConstructInfo>.Quick(_focus, M.m((ConstructInfo I) => searchText), "Filter : ", new ToolTip("Display only specific blocks in the list."), (ConstructInfo I, string text) =>
            {
                if (searchText != text)
                {
                    searchText = text;
                    TriggerScreenRebuild();
                }
            }));



            ScreenSegmentTable ScreenSegment_1 = CreateTableSegment(2, textList.Count);
            ScreenSegment_1.SpaceAbove = 10f;

            foreach (string[] t in textList)
            {
                ScreenSegment_1.AddInterpretter(SubjectiveDisplay<int>.Quick(default, M.m<int>(I => t[0]))).Justify = TextAnchor.UpperRight;
                ScreenSegment_1.AddInterpretter(SubjectiveDisplay<int>.Quick(default, M.m<int>(I => t[1]))).Justify = TextAnchor.UpperLeft;
            }
        }
    }
}
