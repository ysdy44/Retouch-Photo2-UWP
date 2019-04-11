using System.Numerics;

namespace Retouch_Photo.Brushs.LinearGradient
{
    public class LinearGradientManager
    {
        public LinearGradientType Type;

        public Vector2 StartPoint;
        public Vector2 EndPoint;

        public void Initialize(Vector2 startPoint, Vector2 endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
    }
}
