using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;

namespace Retouch_Photo2.Brushs.RadialGradient
{
    public class RadialGradientManager
    {
        public RadialGradientType Type;


        public Vector2 Point;
        public Vector2 Center;
        public float GetRadius(Vector2 center, Vector2 point) => Vector2.Distance(center, point);


        public RadialGradientManager() { }

        public RadialGradientManager(Vector2 center, Vector2 point) => this.Initialize(center, point);
        public void Initialize(Vector2 center, Vector2 point)
        {
            this.Center = center;
            this.Point = point;
        }

        public RadialGradientManager(Vector2 center, float radius) => this.Initialize(center, radius);
        public void Initialize(Vector2 center, float radius)
        {
            this.Center = center;
            this.Point = new Vector2(center.X+radius, center.Y);
        }

        public CanvasRadialGradientBrush GetBrush(ICanvasResourceCreator creator, Matrix3x2 matrix, CanvasGradientStop[] array)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 point = Vector2.Transform(this.Point, matrix);
            float radius = Vector2.Distance(center, point);
            return new CanvasRadialGradientBrush(creator, array)
            {
                RadiusX = radius,
                RadiusY = radius,
                Center = center
            };
        }
    }
}
