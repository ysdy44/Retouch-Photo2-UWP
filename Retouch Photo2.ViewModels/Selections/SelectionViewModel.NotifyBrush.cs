using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.ILayer;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels.Selections
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke;


        //////////////////////////////////////

            
        /// <summary> Brush's type. </summary>     
        public BrushType BrushType
        {
            get => this.brushType;
            set
            {
                this.brushType = value;
                this.OnPropertyChanged(nameof(this.BrushType));//Notify 
            }
        }
        private BrushType brushType = BrushType.Disabled;

        /// <summary> Brush's gradient stops. </summary>     
        public CanvasGradientStop[] BrushArray
        {
            get => this.brushArray;
            set
            {
                this.brushArray = value;
                this.OnPropertyChanged(nameof(this.BrushArray));//Notify 
            }
        }
        private CanvasGradientStop[] brushArray;
        
        /// <summary> Brush. </summary>     
        public BrushPoints BrushPoints;
                
        /// <summary> Sets GeometryLayer's brush. </summary>     
        public void SetBrush(Brush brush)
        {
            switch (brush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    {
                        this.Color = brush.Color;
                    }
                    break;
                case BrushType.LinearGradient:
                    {
                        this.BrushPoints.LinearGradientStartPoint = brush.Points.LinearGradientStartPoint;
                        this.BrushPoints.LinearGradientEndPoint = brush.Points.LinearGradientEndPoint;

                        this.BrushArray = brush.Array;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.BrushPoints.RadialGradientCenter = brush.Points.RadialGradientCenter;
                        this.BrushPoints.RadialGradientPoint = brush.Points.RadialGradientPoint;

                        this.BrushArray = brush.Array;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.BrushPoints.EllipticalGradientCenter = brush.Points.EllipticalGradientCenter;
                        this.BrushPoints.EllipticalGradientXPoint = brush.Points.EllipticalGradientXPoint;
                        this.BrushPoints.EllipticalGradientYPoint = brush.Points.EllipticalGradientYPoint;

                        this.BrushArray = brush.Array;
                    }
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }

            this.BrushType = brush.Type;
        }

        /// <summary> Sets GeometryLayer. </summary>     
        private void SetGeometryLayer(Layer layer)
        {
            if (layer is IGeometryLayer geometryLayer)
            {
                this.FillColor = geometryLayer.FillBrush.Color;
                this.StrokeColor = geometryLayer.StrokeBrush.Color;
                this.StrokeWidth = geometryLayer.StrokeWidth;

                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SetBrush(geometryLayer.FillBrush);
                        break;
                    case FillOrStroke.Stroke:
                        this.SetBrush(geometryLayer.StrokeBrush);
                        break;
                }
                return;
            }

            this.BrushType = BrushType.Disabled;
        }


        //////////////////////////////////////


        /// <summary>
        /// Sets the brush form SingleMode.
        /// </summary>
        /// <param name="fillOrStroke"></param>
        public void SetBrushFormSingleMode(FillOrStroke fillOrStroke)
        {
            //Brush
            this.FillOrStroke = fillOrStroke;

            //Selection
            if (this.Mode == ListViewSelectionMode.Single)
            {
                if (this.Layer is IGeometryLayer geometryLayer)
                {
                    switch (fillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            {
                                this.BrushType = geometryLayer.FillBrush.Type;
                                this.SetBrush(geometryLayer.FillBrush);
                            }
                            break;
                        case FillOrStroke.Stroke:
                            {
                                this.BrushType = geometryLayer.StrokeBrush.Type;
                                this.SetBrush(geometryLayer.StrokeBrush);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the brush to LinearGradient.
        /// </summary>
        /// <param name="startPoint"> start point. </param>
        /// <param name="endPoint"> end point. </param>
        public void SetBrushToLinearGradient(Vector2 startPoint, Vector2 endPoint)
        {
            //Brush
            this.BrushType = BrushType.LinearGradient;
            this.BrushArray = Brush.GetNewArray();
            this.BrushPoints.LinearGradientStartPoint = startPoint;
            this.BrushPoints.LinearGradientEndPoint = endPoint;

            //FillOrStroke
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    {
                        //Selection
                        this.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                                geometryLayer.FillBrush.Array = Brush.GetNewArray();
                                geometryLayer.FillBrush.Points.LinearGradientStartPoint = startPoint;
                                geometryLayer.FillBrush.Points.LinearGradientEndPoint = endPoint;
                            }
                        }, true);
                    }
                    break;
                case FillOrStroke.Stroke:
                    {
                        //Selection
                        this.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.StrokeBrush.Type = BrushType.LinearGradient;
                                geometryLayer.StrokeBrush.Array = Brush.GetNewArray();
                                geometryLayer.StrokeBrush.Points.LinearGradientStartPoint = startPoint;
                                geometryLayer.StrokeBrush.Points.LinearGradientEndPoint = endPoint;
                            }
                        }, true);
                    }
                    break;
            }
        }       

    }
}