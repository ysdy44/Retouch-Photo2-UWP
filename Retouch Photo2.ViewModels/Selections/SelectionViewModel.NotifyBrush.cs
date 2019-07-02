using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.ILayer;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.ViewModels.Selections
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke;

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
        private BrushType brushType;

        /// <summary> Brush's color. </summary>
        public Color BrushColor
        {
            get => this.brushColor;
            set
            {
                this.brushColor = value;
                this.OnPropertyChanged(nameof(this.BrushColor));//Notify 
            }
        }
        private Color brushColor;
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


        /// <summary> Brush''s linear gradient start point. </summary>
        public Vector2 BrushLinearGradientStartPoint;
        /// <summary> Brush''s linear gradient end point. </summary>
        public Vector2 BrushLinearGradientEndPoint;

        /// <summary> Brush''s radial gradient center point. </summary>
        public Vector2 BrushRadialGradientCenter;
        /// <summary> Brush''s radial gradient control point. </summary>
        public Vector2 BrushRadialGradientPoint;

        /// <summary> Brush''s elliptical gradient center point. </summary>
        public Vector2 BrushEllipticalGradientCenter;
        /// <summary> Brush''s elliptical gradient x-point. </summary>
        public Vector2 BrushEllipticalGradientXPoint;
        /// <summary> Brush''s elliptical gradient y-point. </summary>
        public Vector2 BrushEllipticalGradientYPoint;


        /// <summary> Sets GeometryLayer's brush. </summary>     
        public void SetBrush(Brush brush)
        {
            switch (brush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    {
                        this.BrushColor = brush.Color;
                    }
                    break;
                case BrushType.LinearGradient:
                    {
                        this.BrushLinearGradientStartPoint = brush.LinearGradientStartPoint;
                        this.BrushLinearGradientEndPoint = brush.LinearGradientEndPoint;

                        this.BrushArray = brush.Array;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.BrushRadialGradientCenter = brush.RadialGradientCenter;
                        this.BrushRadialGradientPoint = brush.RadialGradientPoint;

                        this.BrushArray = brush.Array;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.BrushEllipticalGradientCenter = brush.EllipticalGradientCenter;
                        this.BrushEllipticalGradientXPoint = brush.EllipticalGradientXPoint;
                        this.BrushEllipticalGradientYPoint = brush.EllipticalGradientYPoint;

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

            this.BrushType = BrushType.None;
        }


        public void InitializeLinearGradient(Vector2 startPoint, Vector2 endPoint)
        {
            //Brush
            this.BrushType = BrushType.LinearGradient;
            this.BrushArray = Brush.GetNewArray();
            this.BrushLinearGradientStartPoint = startPoint;
            this.BrushLinearGradientEndPoint = endPoint;

            //Selection
            this.SetValue((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    switch (this.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            {
                                geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                                geometryLayer.FillBrush.Array = Brush.GetNewArray();
                                geometryLayer.FillBrush.LinearGradientStartPoint = startPoint;
                                geometryLayer.FillBrush.LinearGradientEndPoint = endPoint;
                            }
                            break;
                        case FillOrStroke.Stroke:
                            {
                                geometryLayer.StrokeBrush.Type = BrushType.LinearGradient;
                                geometryLayer.StrokeBrush.Array = Brush.GetNewArray();
                                geometryLayer.StrokeBrush.LinearGradientStartPoint = startPoint;
                                geometryLayer.StrokeBrush.LinearGradientEndPoint = endPoint;
                            }
                            break;
                    }
                }
            });
        }


    }
}
