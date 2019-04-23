using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;

namespace Retouch_Photo2.Brushs.LinearGradient
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

        public CanvasLinearGradientBrush GetBrush(ICanvasResourceCreator creator, Matrix3x2 matrix, CanvasGradientStop[] array) => new CanvasLinearGradientBrush(creator, array)
        {
            StartPoint = Vector2.Transform(this.StartPoint, matrix),
            EndPoint = Vector2.Transform(this.EndPoint, matrix)
        };
    }
}
