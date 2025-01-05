using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;

namespace AdvancedMimicUi
{
    public class HuwaDecorationMeshSelectTab : HuwaMeshSelectTab
    {
        private HuwaMultiDecorationUi _huwaMultiDecorationUi;

        public HuwaDecorationMeshSelectTab(ConsoleWindow window, HuwaMultiDecorationUi huwaMultiDecorationUi) : base(window)
        {
            _huwaMultiDecorationUi = huwaMultiDecorationUi;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = default;

            if (_huwaMultiDecorationUi.GetDecoration(out Decoration deco))
            {
                MAD_CD = new MimicAndDecorationCommonData(deco);
                return true;
            }

            return false;
        }
    }
}
