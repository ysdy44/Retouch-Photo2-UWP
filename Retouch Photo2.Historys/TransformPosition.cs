using FanKit.Transformers;
using Retouch_Photo2.Layers;
using System.Numerics;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Transform position of the <see cref="ILayer.Style"/> and <see cref="ILayer.Transform"/>.
    /// </summary>
    internal struct TransformPosition
    {
        public Vector2 Style_Fill_Center;
        public Vector2 Style_Fill_XPoint;
        public Vector2 Style_Fill_YPoint;

        public Vector2 Style_Stroke_Center;
        public Vector2 Style_Stroke_XPoint;
        public Vector2 Style_Stroke_YPoint;

        public Transformer Transform_Transformer;
        public Transformer Transform_CropTransformer;
    }
}