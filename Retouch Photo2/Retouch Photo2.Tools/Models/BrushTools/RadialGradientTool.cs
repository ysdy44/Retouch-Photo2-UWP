using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.ILayer;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Numerics;

namespace Retouch_Photo2.Retouch_Photo2.Tools.Models.BrushTools
{
    /// <summary>
    /// Type of <see cref="RadialGradientTool">.
    /// </summary>
    public enum RadialGradientType
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Cener point. </summary>
        Center,
        /// <summary> Point. </summary>
        Point
    }

    /// <summary>
    /// <see cref="BrushTool"/>'s BrushRadialGradientTool.
    /// </summary>
    public class RadialGradientTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        Vector2 Center { get => this.SelectionViewModel.BrushRadialGradientCenter; set => this.SelectionViewModel.BrushRadialGradientCenter = value; }
        Vector2 Point { get => this.SelectionViewModel.BrushRadialGradientPoint; set => this.SelectionViewModel.BrushRadialGradientPoint = value; }


        /// <summary> Type of <see cref="RadialGradientTool">. </summary>
        public RadialGradientType Type = RadialGradientType.None;
        Vector2 OldCenter;
        Vector2 OldPoint;


        //@Override
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (Transformer.InNodeRadius(startingPoint, center))
            {
                this.OldCenter = this.Center;
                this.OldPoint = this.Point;

                this.Type = RadialGradientType.Center;
                return;
            }

            Vector2 point2 = Vector2.Transform(this.Point, matrix);
            if (Transformer.InNodeRadius(startingPoint, point2))
            {
                this.Type = RadialGradientType.Point;
                return;
            }

            this.Type = RadialGradientType.None;
            return;
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case RadialGradientType.None:
                    break;
                case RadialGradientType.Center:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 center = Vector2.Transform(point, inverseMatrix);
                        Vector2 point2 = center + this.OldPoint - this.OldCenter;

                        //Brush
                        this.Center = center;
                        this.Point = point2;

                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                switch (this.SelectionViewModel.FillOrStroke)
                                {
                                    case FillOrStroke.Fill:
                                        {
                                            geometryLayer.FillBrush.RadialGradientCenter = center;
                                            geometryLayer.FillBrush.RadialGradientPoint = point2;
                                        }
                                        break;
                                    case FillOrStroke.Stroke:
                                        {
                                            geometryLayer.StrokeBrush.RadialGradientCenter = center;
                                            geometryLayer.StrokeBrush.RadialGradientPoint = point2;
                                        }
                                        break;
                                }
                            }
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case RadialGradientType.Point:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 point2 = Vector2.Transform(point, inverseMatrix);

                        //Brush
                        this.Point = point2;

                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                switch (this.SelectionViewModel.FillOrStroke)
                                {
                                    case FillOrStroke.Fill:
                                        {
                                            geometryLayer.FillBrush.RadialGradientPoint = point2;
                                        }
                                        break;
                                    case FillOrStroke.Stroke:
                                        {
                                            geometryLayer.StrokeBrush.RadialGradientPoint = point2;
                                        }
                                        break;
                                }
                            }
                        });

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }


        public void Draw(CanvasDrawingSession ds)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 point2 = Vector2.Transform(this.Point, matrix);
            ds.DrawThickLine(center, point2);

            foreach (CanvasGradientStop stop in this.SelectionViewModel.BrushArray)
            {
                Vector2 position = center * (1.0f - stop.Position) + point2 * stop.Position;
                ds.DrawNode2(position, stop.Color);
            }
        }
    }
}