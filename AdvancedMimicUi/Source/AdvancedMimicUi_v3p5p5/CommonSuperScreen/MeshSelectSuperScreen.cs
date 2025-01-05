using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Containers;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedMimicUi
{
    public class MeshSelectSuperScreen : SuperScreen<int>
    {
        public override Content Name
        {
            get
            {
                return new Content("Pick a mesh", new ToolTip("Pick a mesh to render."), "MeshSelectSuperScreen");
            }
        }

        public MeshSelectSuperScreen(ConsoleWindow window) : base(window, default)
        {
        }

        public override void Build()
        {
            ScreenSegmentStandard screenSegment_0 = CreateStandardSegment();
            screenSegment_0.SpaceAbove = 10f;
            screenSegment_0.SpaceBelow = 20f;

            screenSegment_0.AddInterpretter(SubjectiveDisplay<int>.Quick(_focus, M.m(
                (int I) =>
                {
                    if (GetCommonData(out MimicAndDecorationCommonData data))
                    {
                        return $"Currently selected mesh  :  <<{HuwaMimicHelp.GetItemName(data.MeshGuid)}>>";
                    }

                    return "Currently selected mesh  :  <<Empty>>";
                })));

            screenSegment_0.AddInterpretter(new Blank(10f));

            screenSegment_0.AddInterpretter(SubjectiveButton<int>.Quick(
                _focus, "Select mirror mesh", new ToolTip("Select the mesh provided for mirror. (Only the structure block and some blocks)"),
                (int I) =>
                {
                    if (GetCommonData(out MimicAndDecorationCommonData data))
                    {
                        data.MeshGuid = HuwaMimicHelp.GetMirrorMesh(data.MeshGuid);
                    }
                }
                )).FadeOut = M.m(
                (int I) =>
                {
                    if (GetCommonData(out MimicAndDecorationCommonData data) && HuwaMimicHelp.GetItemDefinition(data.MeshGuid, out ItemDefinition selectItem))
                    {
                        return !selectItem.MirrorLaterialFlipReplacementReference.IsValidReference;
                    }

                    return true;
                });

            screenSegment_0.AddInterpretter(new Blank(15f));

            screenSegment_0.AddInterpretter(new HuwaSearcher<ModComponentAbstract>(() => "Filter:", () => new ToolTip("Select a mesh to use"),
                () =>
                {
                    IEnumerable<ItemDefinition> components_1 = Configured.i.Get<ModificationComponentContainerItem>().Components.Where(id => id.MeshReference.IsValidReference);
                    IEnumerable<ObjectDefinition> components_2 = Configured.i.Get<BrilliantSkies.Modding.Containers.Objects>().Components.Where(od => od.MeshReference.IsValidReference);

                    return components_1.Cast<ModComponentAbstract>().Concat(components_2).ToList();
                },
                (ModComponentAbstract mca, string filter) =>
                {
                    filter = filter.ToLower();

                    bool flag_0 = SettingsSuperScreen.SearchName && mca.ComponentId.Name.ToLower().Contains(filter);
                    bool flag_1 = SettingsSuperScreen.SearchRuntimeId && mca.ComponentId.RuntimeId.ToString().ToLower().Contains(filter);
                    bool flag_2 = SettingsSuperScreen.SearchGuid && mca.ComponentId.Guid.ToString().ToLower().Contains(filter);

                    return flag_0 || flag_1 || flag_2;
                },
                M.m<ModComponentAbstract>(I => I.ComponentId.Name), M.m<ModComponentAbstract>(I => new ToolTip(I.Description.ToString())),
                (ModComponentAbstract selected) =>
                {
                    if (GetCommonData(out MimicAndDecorationCommonData data))
                    {
                        data.MeshGuid = selected.ComponentId.Guid;
                    }
                }));
        }

        public virtual bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = default;
            return false;
        }
    }
}
