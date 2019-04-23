using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs.LinearGradient;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.Models.PaintBrushTools
{
    public class ILinearGradient
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;


        public void Start(LinearGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 matrix = transformer.Matrix * this.ViewModel.MatrixTransformer.Matrix;

            Vector2 startPoint = Vector2.Transform(manager.StartPoint, matrix);
            if (Transformer.OutNodeDistance(point, startPoint) == false)
            {
                manager.Type = LinearGradientType.StartPoint;
                return;
            }

            Vector2 endPoint = Vector2.Transform(manager.EndPoint, matrix);
            if (Transformer.OutNodeDistance(point, endPoint) == false)
            {
                manager.Type = LinearGradientType.EndPoint;
                return;
            }

            this.ViewModel.Invalidate();
        }
        public void Delta(LinearGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix * transformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (manager.Type)
            {
                case LinearGradientType.None:
                    return;

                case LinearGradientType.StartPoint:
                    manager.StartPoint = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                case LinearGradientType.EndPoint:
                    manager.EndPoint = canvasPoint;
                    break;

                default:
                    return;
            }
        }
        public void Complete(LinearGradientManager manager)
        {
            manager.Type = LinearGradientType.None;
        }

        public void Draw(CanvasDrawingSession ds, LinearGradientManager manager, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(manager.StartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(manager.EndPoint, matrix);

            ds.DrawLine(startPoint, endPoint, Colors.DodgerBlue);
            Transformer.DrawNode(ds, startPoint);
            Transformer.DrawNode(ds, endPoint);
        }
    }

}
