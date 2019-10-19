using FanKit.Transformers;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    public static class GeometryUtil
    {
        public static readonly Transformer OneTransformer = new Transformer(2f, 2f, new Vector2(-1f, -1f));
        public const float StartingRotation = -FanKit.Math.Pi / 2.0f;
        public static Vector2 GetRotationVector(float rotation) => new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
    }
}