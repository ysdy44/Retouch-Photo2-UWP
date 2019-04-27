using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs.RadialGradient
{
    public class RadialGradientManager:IGradientManager
    {
        public RadialGradientType Type;

        #region Gradient


                 
        public Vector2 Point,OldPoint;

        private Vector2 center;
        public Vector2 Center
        {
            get => this.center;
            set
            {
                //Inner
                Vector2 vector = value - this.center;
                this.Point += vector;

                this.center = value;
            }
        }
        public Vector2 OldCenter;

        public float GetRadius(Vector2 center, Vector2 point) => Vector2.Distance(center, point);


        #endregion


        public RadialGradientManager() { }

        public RadialGradientManager(Vector2 center, Vector2 point) => this.Initialize(center, point);
        public void Initialize(Vector2 center, Vector2 point)
        {
            this.center = center;
            this.Point = point;
        }

        public RadialGradientManager(Vector2 center, float radius) => this.Initialize(center, radius);
        public void Initialize(Vector2 center, float radius)
        {
            this.center = center;
            this.Point = new Vector2(center.X+radius, center.Y);
        }


        public ICanvasBrush GetBrush(ICanvasResourceCreator creator, Matrix3x2 matrix, CanvasGradientStop[] array)
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

        #region Tool


        public void Start(Vector2 point, Matrix3x2 matrix)
        {
            Vector2 point2 = Vector2.Transform(this.Point, matrix);
            if (Transformer2222.OutNodeDistance(point, point2) == false)
            {
                this.Type = RadialGradientType.Point;
                return;
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (Transformer2222.OutNodeDistance(point, center) == false)
            {
                this.Type = RadialGradientType.Center;
                return;
            }
        }
        public void Delta(Vector2 point, Matrix3x2 inverseMatrix)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Type)
            {
                case RadialGradientType.None:
                    return;

                case RadialGradientType.Point:
                    this.Point = canvasPoint;
                    break;

                case RadialGradientType.Center:
                    this.Center = canvasPoint;
                    break;

                default:
                    return;
            }
        }
        public void Complete() => this.Type = RadialGradientType.None;

        public void Draw(CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            Vector2 point = Vector2.Transform(this.Point, matrix);
            Vector2 center = Vector2.Transform(this.Center, matrix);

            ds.DrawLine(point, center, Colors.DodgerBlue);
            Transformer2222.DrawNode(ds, point);
            Transformer2222.DrawNode(ds, center);
        }


        #endregion
    }
}
