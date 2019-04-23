using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Tools.Models.CursorTools
{
    public class IBlueBox : ITool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        bool IsCursorBox = false;

        public Vector2 StartPoint;
        public Vector2 EndPoint;

        public override bool Start(Vector2 point)
        {
            this.IsCursorBox = true;

            this.StartPoint =
            this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);

            return true;
        }
        public override bool Delta(Vector2 point)
        {
            if (this.IsCursorBox)
            {
                this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
                this.ViewModel.Invalidate();
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Complete(Vector2 point)
        {
            if (this.IsCursorBox)
            {
                this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
                this.IsCursorBox = false;

                this.ViewModel.CurrentLayer = null;
                this.ViewModel.Invalidate();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Draw(CanvasDrawingSession ds)
        {
            if (this.IsCursorBox)
            {
                var matrix = this.ViewModel.MatrixTransformer.Matrix;

                Vector2[] points = new Vector2[4];
                points[0] = Vector2.Transform(this.StartPoint, matrix);
                points[1] = Vector2.Transform(new Vector2(this.StartPoint.X, this.EndPoint.Y), matrix);
                points[2] = Vector2.Transform(this.EndPoint, matrix);
                points[3] = Vector2.Transform(new Vector2(this.EndPoint.X, this.StartPoint.Y), matrix);
                CanvasGeometry geometry = CanvasGeometry.CreatePolygon(this.ViewModel.CanvasDevice, points);

                ds.FillGeometry(geometry, Color.FromArgb(128, 30, 144, 255));
                ds.DrawGeometry(geometry, Colors.DodgerBlue, 1);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
