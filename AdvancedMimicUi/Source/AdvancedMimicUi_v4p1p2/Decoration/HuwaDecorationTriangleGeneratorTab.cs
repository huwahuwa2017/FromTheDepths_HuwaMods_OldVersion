using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;
using HarmonyLib;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaDecorationTriangleGeneratorTab : HuwaTriangleGeneratorTab
    {
        private HuwaMultiDecorationUi _huwaMultiDecorationUi;

        public HuwaDecorationTriangleGeneratorTab(ConsoleWindow window, HuwaMultiDecorationUi huwaMultiDecorationUi) : base(window)
        {
            _huwaMultiDecorationUi = huwaMultiDecorationUi;
        }

        public override bool GetCommonData(out MimicAndDecorationCommonData MAD_CD)
        {
            if (_huwaMultiDecorationUi.GetDecoration(out Decoration deco))
            {
                MAD_CD = new MimicAndDecorationCommonData(deco);
                return true;
            }

            MAD_CD = default;
            return false;
        }

        public override void GetWorldPositionAndWorldRotation(out Vector3 worldPosition, out Quaternion worldRotation)
        {
            DecorationsSubject decorationsSubject = _huwaMultiDecorationUi.GetDecorationsSubject();
            AllConstruct allConstruct = Traverse.Create(decorationsSubject.ConstructDecorations).Property("_construct").GetValue<AllConstruct>();
            Vector3i localPosition = Traverse.Create(decorationsSubject).Field("_point").GetValue<Vector3i>();

            worldPosition = allConstruct.SafeLocalToGlobal(localPosition);
            worldRotation = allConstruct.SafeRotation;
        }
    }
}
