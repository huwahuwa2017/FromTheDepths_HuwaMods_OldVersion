using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using ES2_PolygonControl;

namespace AdvancedMimicUi
{
    public class HuwaDecorationMeshSelectTab : MeshSelectSuperScreen
    {
        private HuwaMultiDecorationUi huwaMultiDecorationUi;

        public HuwaDecorationMeshSelectTab(ConsoleWindow window, HuwaMultiDecorationUi huwaMultiDecorationUi) : base(window)
        {
            this.huwaMultiDecorationUi = huwaMultiDecorationUi;
        }

        public override bool GetCommonData(out MimicAndDecoration_CommonData MAD_CD)
        {
            MAD_CD = default;

            if (huwaMultiDecorationUi.GetDecoration(out Decoration deco))
            {
                MAD_CD = new MimicAndDecoration_CommonData(deco);
                return true;
            }

            return false;
        }
    }
}
