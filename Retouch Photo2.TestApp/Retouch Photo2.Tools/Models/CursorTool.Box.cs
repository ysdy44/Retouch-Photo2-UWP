using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Transformers;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s CursorTool .
    /// </summary>
    public partial class CursorTool : Tool
    {
        /// <summary> <see cref = "CursorTool.Delta" />'s method. </summary>
        private void BoxDelta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 pointA = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 pointB = Vector2.Transform(point, inverseMatrix);
            this.boxCanvasRect = new TransformerRect(pointA, pointB);
        }

        /// <summary> <see cref = "CursorTool.Complete" />'s method. </summary>
        private void BoxComplete()
        {
            //Add
            foreach (Layer layer in this.ViewModel.Layers)
            {
                bool contained = Transformer.Contained(this.boxCanvasRect, layer.TransformerMatrix.Destination);

                //Add
                switch (this.KeyboardViewModel.CompositeMode)
                {
                    case CompositeMode.New:
                        {
                            layer.IsChecked = contained;
                        }
                        break;
                    case CompositeMode.Add:
                        {
                            if (contained)
                                layer.IsChecked = true;
                        }
                        break;
                    case CompositeMode.Subtract:
                        {
                            if (contained)
                                layer.IsChecked = false;
                        }
                        break;
                    case CompositeMode.Intersect:
                        {
                            if (contained == false)
                                layer.IsChecked = false;
                        }
                        break;
                }
            }
        }

        /// <summary> <see cref = "CursorTool.Draw" />'s method. </summary>
        public void BoxDraw(CanvasDrawingSession ds)
        {
            //Points
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2[] points = new Vector2[]
            {
                 Vector2.Transform(this.boxCanvasRect.LeftTop, matrix),
                 Vector2.Transform(this.boxCanvasRect.RightTop, matrix),
                 Vector2.Transform(this.boxCanvasRect.RightBottom, matrix),
                 Vector2.Transform(this.boxCanvasRect.LeftBottom, matrix),
            };

            //Geometry
            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(this.ViewModel.CanvasDevice, points);
            ds.FillGeometry(geometry, Windows.UI.Color.FromArgb(128, 30, 144, 255));
            ds.DrawGeometry(geometry, Windows.UI.Colors.DodgerBlue, 1);
        }
    }
}
