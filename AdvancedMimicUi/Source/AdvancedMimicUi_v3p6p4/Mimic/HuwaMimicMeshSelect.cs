using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaMimicMeshSelect : MeshSelectSuperScreen
    {
        private Mimic mimic;

        public HuwaMimicMeshSelect(ConsoleWindow window, Mimic mimic) : base(window)
        {
            this.mimic = mimic;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = new MimicAndDecorationCommonData(mimic);
            return true;
        }
    }
}
