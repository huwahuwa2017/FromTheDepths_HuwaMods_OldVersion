using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaMimicTriangleGeneratorTab : HuwaTriangleGeneratorTab
    {
        private Mimic _mimic;

        public HuwaMimicTriangleGeneratorTab(ConsoleWindow window, Mimic mimic) : base(window)
        {
            _mimic = mimic;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = new MimicAndDecorationCommonData(_mimic);
            return true;
        }

        public override void GetWorldPositionAndWorldRotation(out Vector3 worldPosition, out Quaternion worldRotation)
        {
            worldPosition = _mimic.GameWorldPosition;
            worldRotation = _mimic.GameWorldRotation;
        }
    }
}
