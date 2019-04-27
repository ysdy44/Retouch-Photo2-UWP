using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs.LinearGradient
{
    public class LinearGradientManager : IGradientManager
    {
        public LinearGradientType Type;

        #region Gradient


        public Vector2 StartPoint, OldStartPoint;
        public Vector2 EndPoint, OldEndPoint;
    
        #endregion


        public void Initialize(Vector2 startPoint, Vector2 endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        public ICanvasBrush GetBrush(ICanvasResourceCreator creator, Matrix3x2 matrix, CanvasGradientStop[] array) => new CanvasLinearGradientBrush(creator, array)
        {
            StartPoint = Vector2.Transform(this.StartPoint, matrix),
            EndPoint = Vector2.Transform(this.EndPoint, matrix)
        };


        #region Tool


        public void Start(Vector2 point, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            if (Transformer2222.OutNodeDistance(point, startPoint) == false)
            {
                this.Type = LinearGradientType.StartPoint;
                return;
            }

            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);
            if (Transformer2222.OutNodeDistance(point, endPoint) == false)
            {
                this.Type = LinearGradientType.EndPoint;
                return;
            }
        }
        public void Delta(Vector2 point, Matrix3x2 inverseMatrix)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Type)
            {
                case LinearGradientType.None:
                    return;

                case LinearGradientType.StartPoint:
                    this.StartPoint = canvasPoint;
                    break;

                case LinearGradientType.EndPoint:
                    this.EndPoint = canvasPoint;
                    break;

                default:
                    return;
            }
        }
        public void Complete() => this.Type = LinearGradientType.None;

        public void Draw(CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);

            ds.DrawLine(startPoint, endPoint, Colors.DodgerBlue);
            Transformer2222.DrawNode(ds, startPoint);
            Transformer2222.DrawNode(ds, endPoint);
        }


        #endregion
    }
}