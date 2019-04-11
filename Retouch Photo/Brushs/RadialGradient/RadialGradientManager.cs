using System.Numerics;

namespace Retouch_Photo.Brushs.RadialGradient
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
    }
}
