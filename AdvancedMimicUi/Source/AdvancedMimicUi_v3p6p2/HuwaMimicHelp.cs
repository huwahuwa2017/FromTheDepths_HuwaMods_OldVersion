using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Core.Id;
using BrilliantSkies.Ftd.Avatar.Build;
using BrilliantSkies.Ftd.Avatar.Build.Marker;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Containers;
using BrilliantSkies.Modding.Types;
using EndlessShapes2.Polygon;
using System;

namespace AdvancedMimicUi
{
    public static class HuwaMimicHelp
    {
        public static string GetItemName(Guid searchGuid)
        {
            ModComponentAbstract mca;

            if (MimicHelp.TryGetItemOrObject(searchGuid, out mca))
            {
                return mca.ComponentId.Name;
            }

            return "Empty";
        }

        public static bool GetItemDefinition(Guid searchGuid, out ItemDefinition itemDefinition)
        {
            itemDefinition = Configured.i.Get<ModificationComponentContainerItem>().Find(searchGuid);
            return itemDefinition != null;
        }

        public static Guid GetMirrorMesh(Guid guid)
        {
            if (GetItemDefinition(guid, out ItemDefinition selectItem))
            {
                ReferenceToComponentId mirrorReference = selectItem.MirrorLaterialFlipReplacementReference;

                if (mirrorReference.IsValidReference)
                {
                    return mirrorReference.Reference.Guid;
                }
            }

            return guid;
        }

        public static bool GetMirrorStuff(out MirrorStuff mirrorStuff)
        {
            mirrorStuff = null;

            cBuild cBuildMe = cBuild.GetSingleton();

            if (cBuildMe != null && cBuildMe.buildMarker != null)
            {
                mirrorStuff = cBuildMe.buildMarker.mirror;
                return true;
            }

            return false;
        }

        public static void XAxisFlip(MimicAndDecorationCommonData data)
        {
            data.PositioningX *= -1f;
            data.OrientationY *= -1f;
            data.OrientationZ *= -1f;

            data.MeshGuid = GetMirrorMesh(data.MeshGuid);
        }

        public static void YAxisFlip(MimicAndDecorationCommonData data)
        {
            data.PositioningY *= -1f;
            data.OrientationX *= -1f;
            data.OrientationZ *= -1f;

            data.OrientationZ += 180f;

            data.MeshGuid = GetMirrorMesh(data.MeshGuid);
        }

        public static void ZAxisFlip(MimicAndDecorationCommonData data)
        {
            data.PositioningZ *= -1f;
            data.OrientationY *= -1f;
            data.OrientationZ *= -1f;

            data.OrientationY += 180f;

            data.MeshGuid = GetMirrorMesh(data.MeshGuid);
        }
    }
}
