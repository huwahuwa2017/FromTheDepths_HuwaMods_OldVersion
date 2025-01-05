using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;
using ES2_PolygonControl;

namespace AdvancedMimicUi
{
    public class HuwaMimicMeshSelect : MeshSelectSuperScreen
    {
        private Mimic mimic;

        public HuwaMimicMeshSelect(ConsoleWindow window, Mimic mimic) : base(window)
        {
            this.mimic = mimic;
        }

        public override bool GetCommonData(out MimicAndDecoration_CommonData MAD_CD)
        {
            MAD_CD = new MimicAndDecoration_CommonData(mimic);
            return true;
        }
    }
}
