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
    /// Type of <see cref="LinearGradientTool">.
    /// </summary>
    public enum LinearGradientType
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Start point. </summary>
        StartPoint,
        /// <summary> End point. </summary>
        EndPoint,
    }


    /// <summary>
    /// <see cref="BrushTool"/>'s BrushLinearGradientTool.
    /// </summary>
    public class LinearGradientTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        Vector2 StartPoint { get => this.SelectionViewModel.BrushPoints.LinearGradientStartPoint; set => this.SelectionViewModel.BrushPoints.LinearGradientStartPoint = value; }
        Vector2 EndPoint { get => this.SelectionViewModel.BrushPoints.LinearGradientEndPoint; set => this.SelectionViewModel.BrushPoints.LinearGradientEndPoint = value; }


        /// <summary> Type of <see cref="LinearGradientTool">. </summary>
        public LinearGradientType Type = LinearGradientType.None;


        //@Override
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            if (Transformer.InNodeRadius(startingPoint, startPoint))
            {
                this.Type = LinearGradientType.StartPoint;
                return;
            }

            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);
            if (Transformer.InNodeRadius(startingPoint, endPoint))
            {
                this.Type = LinearGradientType.EndPoint;
                return;
            }

            this.Type = LinearGradientType.None;
            return;
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case LinearGradientType.None:
                    break;
                case LinearGradientType.StartPoint:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 startPoint = Vector2.Transform(point, inverseMatrix);

                        //Brush
                        this.StartPoint = startPoint;

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
                                            geometryLayer.FillBrush.Points.LinearGradientStartPoint = startPoint;
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
                                            geometryLayer.StrokeBrush.Points.LinearGradientStartPoint = startPoint;
                                        }
                                    }, true);
                                }
                                break;
                        }

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
                case LinearGradientType.EndPoint:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 endPoint = Vector2.Transform(point, inverseMatrix);

                        //Brush
                        this.EndPoint = endPoint;

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
                                            geometryLayer.FillBrush.Points.LinearGradientEndPoint = endPoint;
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
                                            geometryLayer.StrokeBrush.Points.LinearGradientEndPoint = endPoint;
                                        }
                                    }, true);
                                }
                                break;
                        }

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }


        public void Draw(CanvasDrawingSession ds)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);
            ds.DrawThickLine(startPoint, endPoint);

            foreach (CanvasGradientStop stop in this.SelectionViewModel.BrushArray)
            {
                Vector2 position = startPoint * (1.0f - stop.Position) + endPoint * stop.Position;
                ds.DrawNode2(position, stop.Color);
            }
        }
    }
}