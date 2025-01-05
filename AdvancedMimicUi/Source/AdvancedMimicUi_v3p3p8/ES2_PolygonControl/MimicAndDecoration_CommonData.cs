using BrilliantSkies.Blocks.Decorative;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using System;
using UnityEngine;

namespace ES2_PolygonControl
{
    public class MimicAndDecoration_CommonData
    {
        public static void Copy(MimicAndDecoration_CommonData source, MimicAndDecoration_CommonData destination)
        {
            destination.MeshGuid = source.MeshGuid;
            destination.Positioning = source.Positioning;
            destination.Scaling = source.Scaling;
            destination.Orientation = source.Orientation;
            destination.ColorIndex = source.ColorIndex;
        }



        private MAD_DataType dataType = MAD_DataType.None;

        private object data;

        private Guid meshGuid = new Guid();

        private Vector3 positioning = Vector3.zero;

        private Vector3 scaling = Vector3.one;

        private Vector3 orientation = Vector3.zero;

        private int colorIndex = 0;

        public Guid MeshGuid
        {
            get
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        return meshGuid;
                    case MAD_DataType.Mimic:
                        return ((Mimic)data).Data.MeshGuid.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)data).MeshGuid.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        meshGuid = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)data).Data.MeshGuid.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)data).MeshGuid.Us = value;
                        break;
                }
            }
        }

        public Vector3 Positioning
        {
            get
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        return positioning;
                    case MAD_DataType.Mimic:
                        return ((Mimic)data).Data.Positioning.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)data).Positioning.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        positioning = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)data).Data.Positioning.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)data).Positioning.Us = value;
                        break;
                }
            }
        }

        public Vector3 Scaling
        {
            get
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        return scaling;
                    case MAD_DataType.Mimic:
                        return ((Mimic)data).Data.Scaling.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)data).Scaling.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        scaling = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)data).Data.Scaling.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)data).Scaling.Us = value;
                        break;
                }
            }
        }

        public Vector3 Orientation
        {
            get
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        return orientation;
                    case MAD_DataType.Mimic:
                        return ((Mimic)data).Data.Orientation.Us;
                    case MAD_DataType.Decoration:
                        return ((Decoration)data).Orientation.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        orientation = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)data).Data.Orientation.Us = value;
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)data).Orientation.Us = value;
                        break;
                }
            }
        }

        public int ColorIndex
        {
            get
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        return colorIndex;
                    case MAD_DataType.Mimic:
                        return ((Mimic)data).color;
                    case MAD_DataType.Decoration:
                        return ((Decoration)data).Color.Us;
                    default:
                        return default;
                }
            }
            set
            {
                switch (dataType)
                {
                    case MAD_DataType.None:
                        colorIndex = value;
                        break;
                    case MAD_DataType.Mimic:
                        ((Mimic)data).SetColor(value);
                        break;
                    case MAD_DataType.Decoration:
                        ((Decoration)data).Color.Us = value;
                        break;
                }
            }
        }



        public float PositioningX
        {
            get
            {
                return Positioning.x;
            }
            set
            {
                Vector3 v = Positioning;
                v.x = value;
                Positioning = v;
            }
        }

        public float PositioningY
        {
            get
            {
                return Positioning.y;
            }
            set
            {
                Vector3 v = Positioning;
                v.y = value;
                Positioning = v;
            }
        }

        public float PositioningZ
        {
            get
            {
                return Positioning.z;
            }
            set
            {
                Vector3 v = Positioning;
                v.z = value;
                Positioning = v;
            }
        }

        public float ScalingX
        {
            get
            {
                return Scaling.x;
            }
            set
            {
                Vector3 v = Scaling;
                v.x = value;
                Scaling = v;
            }
        }

        public float ScalingY
        {
            get
            {
                return Scaling.y;
            }
            set
            {
                Vector3 v = Scaling;
                v.y = value;
                Scaling = v;
            }
        }

        public float ScalingZ
        {
            get
            {
                return Scaling.z;
            }
            set
            {
                Vector3 v = Scaling;
                v.z = value;
                Scaling = v;
            }
        }

        public float OrientationX
        {
            get
            {
                return Orientation.x;
            }
            set
            {
                Vector3 v = Orientation;
                v.x = value;
                Orientation = v;
            }
        }

        public float OrientationY
        {
            get
            {
                return Orientation.y;
            }
            set
            {
                Vector3 v = Orientation;
                v.y = value;
                Orientation = v;
            }
        }

        public float OrientationZ
        {
            get
            {
                return Orientation.z;
            }
            set
            {
                Vector3 v = Orientation;
                v.z = value;
                Orientation = v;
            }
        }



        public MimicAndDecoration_CommonData()
        {
        }

        public MimicAndDecoration_CommonData(Mimic mimic)
        {
            dataType = MAD_DataType.Mimic;
            data = mimic;
        }

        public MimicAndDecoration_CommonData(Decoration decoration)
        {
            dataType = MAD_DataType.Decoration;
            data = decoration;
        }

        private enum MAD_DataType
        {
            None,
            Mimic,
            Decoration
        }
    }
}
