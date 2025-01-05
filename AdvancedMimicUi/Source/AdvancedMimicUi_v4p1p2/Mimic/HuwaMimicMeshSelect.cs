using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaMimicMeshSelect : HuwaMeshSelectTab
    {
        private Mimic _mimic;

        public HuwaMimicMeshSelect(ConsoleWindow window, Mimic mimic) : base(window)
        {
            _mimic = mimic;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = new MimicAndDecorationCommonData(_mimic);
            return true;
        }
    }
}
