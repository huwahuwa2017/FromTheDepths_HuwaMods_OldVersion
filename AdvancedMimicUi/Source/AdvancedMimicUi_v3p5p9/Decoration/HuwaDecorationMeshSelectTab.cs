using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaDecorationMeshSelectTab : MeshSelectSuperScreen
    {
        private HuwaMultiDecorationUi huwaMultiDecorationUi;

        public HuwaDecorationMeshSelectTab(ConsoleWindow window, HuwaMultiDecorationUi huwaMultiDecorationUi) : base(window)
        {
            this.huwaMultiDecorationUi = huwaMultiDecorationUi;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = default;

            if (huwaMultiDecorationUi.GetDecoration(out Decoration deco))
            {
                MAD_CD = new MimicAndDecorationCommonData(deco);
                return true;
            }

            return false;
        }
    }
}
