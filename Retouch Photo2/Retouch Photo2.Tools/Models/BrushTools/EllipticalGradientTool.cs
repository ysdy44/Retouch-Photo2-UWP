using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Numerics;

namespace Retouch_Photo2.Retouch_Photo2.Tools.Models.BrushTools
{
    /// <summary>
    /// Type of <see cref="RadialGradientTool">.
    /// </summary>
    public enum EllipticalGradientType
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Center point. </summary>
        Center,
        /// <summary> X point. </summary>
        XPoint,
        /// <summary> Y point. </summary>
        YPoint,
    }


    /// <summary>
    /// <see cref="BrushTool"/>'s BrushEllipticalGradientTool.
    /// </summary>
    public class EllipticalGradientTool 
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        Vector2 Center { get => this.SelectionViewModel.BrushPoints.EllipticalGradientCenter; set => this.SelectionViewModel.BrushPoints.EllipticalGradientCenter = value; }
        Vector2 XPoint { get => this.SelectionViewModel.BrushPoints.EllipticalGradientXPoint; set => this.SelectionViewModel.BrushPoints.EllipticalGradientXPoint = value; }
        Vector2 YPoint { get => this.SelectionViewModel.BrushPoints.EllipticalGradientYPoint; set => this.SelectionViewModel.BrushPoints.EllipticalGradientYPoint = value; }


        /// <summary> Type of <see cref="RadialGradientTool">. </summary>
        public EllipticalGradientType Type = EllipticalGradientType.None;
        Vector2 OldCenter;
        Vector2 OldXPoint;
        Vector2 OldYPoint;


        //@Override
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            if (Transformer.InNodeRadius(startingPoint, xPoint))
            {
                this.OldCenter = this.Center;
                this.OldYPoint = this.YPoint;

                this.Type = EllipticalGradientType.XPoint;
                return;
            }

            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            if (Transformer.InNodeRadius(startingPoint, yPoint))
            {
                this.OldCenter = this.Center;
                this.OldXPoint = this.XPoint;

                this.Type = EllipticalGradientType.YPoint;
                return;
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (Transformer.InNodeRadius(startingPoint, center))
            {
                this.OldCenter = this.Center;
                this.OldXPoint = this.XPoint;
                this.OldYPoint = this.YPoint;

                this.Type = EllipticalGradientType.Center;
                return;
            }

            this.Type = EllipticalGradientType.None;
            return;
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case EllipticalGradientType.None:
                    break;
                case EllipticalGradientType.XPoint:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 xPoint = Vector2.Transform(point, inverseMatrix);

                        Vector2 normalize = Vector2.Normalize(xPoint - this.OldCenter);
                        float radiusY = Vector2.Distance(this.OldYPoint, this.OldCenter);
                        Vector2 reflect = new Vector2(-normalize.Y, normalize.X);
                        Vector2 yPoint = radiusY * reflect + this.OldCenter;

                        //Brush
                        this.XPoint = xPoint;
                        this.YPoint = yPoint;

                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                switch (this.SelectionViewModel.FillOrStroke)
                                {
                                    case FillOrStroke.Fill:
                                        {
                                            geometryLayer.FillBrush.Points.EllipticalGradientXPoint = xPoint;
                                            geometryLayer.FillBrush.Points.EllipticalGradientYPoint = yPoint;
                                        }
                                        break;
                                    case FillOrStroke.Stroke:
                                        {
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientXPoint = xPoint;
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientYPoint = yPoint;
                                        }
                                        break;
                                }
                            }
                        }, true);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case EllipticalGradientType.YPoint:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 yPoint = Vector2.Transform(point, inverseMatrix);

                        Vector2 normalize = Vector2.Normalize(yPoint - this.OldCenter);
                        float radiusX = Vector2.Distance(this.OldXPoint, this.OldCenter);
                        Vector2 reflect = new Vector2(normalize.Y, -normalize.X);
                        Vector2 xPoint = radiusX * reflect + this.OldCenter;

                        //Brush
                        this.YPoint = yPoint;
                        this.XPoint = xPoint;

                        //FillOrStroke
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                {
                                    //Selection
                                    this.SelectionViewModel.SetValue((layer) =>
                                    {
                                        if (layer is IGeometryLayer geometryLayer)
                                        {
                                            geometryLayer.FillBrush.Points.EllipticalGradientYPoint = yPoint;
                                            geometryLayer.FillBrush.Points.EllipticalGradientXPoint = xPoint;
                                        }
                                    }, true);
                                }
                                break;
                            case FillOrStroke.Stroke:
                                {
                                    //Selection
                                    this.SelectionViewModel.SetValue((layer) =>
                                    {
                                        if (layer is IGeometryLayer geometryLayer)
                                        {
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientYPoint = yPoint;
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientXPoint = xPoint;
                                        }
                                    }, true);
                                }
                                break;
                        }

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case EllipticalGradientType.Center:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 center = Vector2.Transform(point, inverseMatrix);
                        Vector2 xPoint = center + this.OldXPoint - this.OldCenter;
                        Vector2 yPoint = center + this.OldYPoint - this.OldCenter;

                        //Brush
                        this.Center = center;
                        this.XPoint = xPoint;
                        this.YPoint = yPoint;

                        //FillOrStroke
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                {
                                    //Selection
                                    this.SelectionViewModel.SetValue((layer) =>
                                    {
                                        if (layer is IGeometryLayer geometryLayer)
                                        {
                                            geometryLayer.FillBrush.Points.EllipticalGradientCenter = center;
                                            geometryLayer.FillBrush.Points.EllipticalGradientXPoint = xPoint;
                                            geometryLayer.FillBrush.Points.EllipticalGradientYPoint = yPoint;
                                        }
                                    }, true);
                                }
                                break;
                            case FillOrStroke.Stroke:
                                {
                                    //Selection
                                    this.SelectionViewModel.SetValue((layer) =>
                                    {
                                        if (layer is IGeometryLayer geometryLayer)
                                        {
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientCenter = center;
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientXPoint = xPoint;
                                            geometryLayer.StrokeBrush.Points.EllipticalGradientYPoint = yPoint;
                                        }
                                    }, true);
                                }
                                break;
                        }

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            drawingSession.DrawThickLine(center, xPoint);
            drawingSession.DrawThickLine(center, yPoint);

            foreach (CanvasGradientStop stop in this.SelectionViewModel.BrushArray)
            {
                Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;
                drawingSession.DrawNode2(position, stop.Color);
            }
            drawingSession.DrawNode2(xPoint);
        }
    }
}