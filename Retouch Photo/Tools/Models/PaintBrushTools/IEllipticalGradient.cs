using Microsoft.Graphics.Canvas;
using Retouch_Photo.Brushs.EllipticalGradient;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models.PaintBrushTools
{
    public class IEllipticalGradient
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public void Start(EllipticalGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 matrix = transformer.Matrix * this.ViewModel.MatrixTransformer.Matrix;

            Vector2 xPoint = Vector2.Transform(manager.XPoint, matrix);
            if (Transformer.OutNodeDistance(point, xPoint) == false)
            {
                manager.Type = EllipticalGradientType.XPoint;
                return;
            }

            Vector2 yPoint = Vector2.Transform(manager.YPoint, matrix);
            if (Transformer.OutNodeDistance(point, yPoint) == false)
            {
                manager.Type = EllipticalGradientType.YPoint;
                return;
            }

            Vector2 center = Vector2.Transform(manager.Center, matrix);
            if (Transformer.OutNodeDistance(point, center) == false)
            {
                manager.Type = EllipticalGradientType.Center;
                return;
            }

            this.ViewModel.Invalidate();
        }
        public void Delta(EllipticalGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix * transformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (manager.Type)
            {
                case EllipticalGradientType.None:
                    return;

                case EllipticalGradientType.XPoint:
                    manager.XPoint = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                case EllipticalGradientType.YPoint:
                    manager.YPoint = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                case EllipticalGradientType.Center:
                    manager.Center = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                default:
                    return;
            }
        }
        public void Complete(EllipticalGradientManager manager)
        {
            manager.Type = EllipticalGradientType.None;
        }

        public void Draw(CanvasDrawingSession ds, EllipticalGradientManager manager, Matrix3x2 matrix)
        {
            Vector2 xPoint = Vector2.Transform(manager.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(manager.YPoint, matrix);
            Vector2 center = Vector2.Transform(manager.Center, matrix);

            ds.DrawLine(xPoint, center, Colors.DodgerBlue);
            ds.DrawLine(yPoint, center, Colors.DodgerBlue);
            Transformer.DrawNode(ds, xPoint);
            Transformer.DrawNode(ds, yPoint);
            Transformer.DrawNode(ds, center);
        }
    }

}
