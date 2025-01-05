using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaMimicTriangleGeneratorTab : TriangleGeneratorSuperScreen
    {
        private Mimic mimic;

        public HuwaMimicTriangleGeneratorTab(ConsoleWindow window, Mimic mimic) : base(window)
        {
            this.mimic = mimic;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            MAD_CD = new MimicAndDecorationCommonData(mimic);
            return true;
        }

        public override void GetWorldPositionAndWorldRotation(out Vector3 worldPosition, out Quaternion worldRotation)
        {
            worldPosition = mimic.GameWorldPosition;
            worldRotation = mimic.GameWorldRotation;
        }
    }
}
