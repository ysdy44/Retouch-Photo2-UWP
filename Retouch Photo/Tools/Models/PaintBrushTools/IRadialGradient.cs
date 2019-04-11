using Microsoft.Graphics.Canvas;
using Retouch_Photo.Brushs.RadialGradient;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models.PaintBrushTools
{
    public class IRadialGradient
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        public void Start(RadialGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 matrix = transformer.Matrix * this.ViewModel.MatrixTransformer.Matrix;

            Vector2 point2 = Vector2.Transform(manager.Point, matrix);
            if (Transformer.OutNodeDistance(point, point2) == false)
            {
                manager.Type = RadialGradientType.Point;
                return;
            }

            Vector2 center = Vector2.Transform(manager.Center, matrix);
            if (Transformer.OutNodeDistance(point, center) == false)
            {
                manager.Type = RadialGradientType.Center;
                return;
            }

            this.ViewModel.Invalidate();
        }
        public void Delta(RadialGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix * transformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (manager.Type)
            {
                case RadialGradientType.None:
                    return;

                case RadialGradientType.Point:
                    manager.Point = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                case RadialGradientType.Center:
                    manager.Center = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                default:
                    return;
            }
        }
        public void Complete(RadialGradientManager manager)
        {
            manager.Type = RadialGradientType.None;
        }

        public void Draw(CanvasDrawingSession ds, RadialGradientManager manager, Matrix3x2 matrix)
        {
            Vector2 point = Vector2.Transform(manager.Point, matrix);
            Vector2 center = Vector2.Transform(manager.Center, matrix);

            ds.DrawLine(point, center, Colors.DodgerBlue);
            Transformer.DrawNode(ds, point);
            Transformer.DrawNode(ds, center);
        }
    }
}
