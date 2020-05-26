using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Style's IsFollowTransform. </summary>     
        public bool IsFollowTransform
        {
            get => this.isFollowTransform;
            set
            {
                if (this.isFollowTransform == value) return;
                this.isFollowTransform = value;
                this.OnPropertyChanged(nameof(this.IsFollowTransform));//Notify 
            }
        }
        private bool isFollowTransform = true;

        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                if (this.fillOrStroke == value) return;
                this.fillOrStroke = value;
                this.OnPropertyChanged(nameof(this.FillOrStroke));//Notify 
            }
        }
        private FillOrStroke fillOrStroke = FillOrStroke.Fill;


        
        /// <summary> Retouch_Photo2's the only color. </summary>
        public Color Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.OnPropertyChanged(nameof(this.Color));//Notify 
            }
        }
        private Color color = Colors.LightGray;


        //////////////////////////


        /// <summary> Retouch_Photo2's the only fill. </summary>
        public IBrush Fill
        {
            get => this.fill;
            set
            {
                this.fill = value;
                this.OnPropertyChanged(nameof(this.Fill));//Notify 
            }
        }
        private IBrush fill = BrushBase.ColorBrush(Colors.LightGray);

        /// <summary> Retouch_Photo2's the only stroke. </summary>
        public IBrush Stroke
        {
            get => this.stroke;
            set
            {
                this.stroke = value;
                this.OnPropertyChanged(nameof(this.Stroke));//Notify 
            }
        }
        private IBrush stroke = new BrushBase();

        /// <summary> Retouch_Photo2's the only stroke-width. </summary>
        public float StrokeWidth
        {
            get => this.strokeWidth;
            set
            {
                this.strokeWidth = value;
                this.OnPropertyChanged(nameof(this.StrokeWidth));//Notify 
            }
        }
        private float strokeWidth = 1;
        
        /// <summary> Retouch_Photo2's the only stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle
        {
            get => this.strokeStyle;
            set
            {
                this.strokeStyle = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle();

        /// <summary> Sets style. </summary>  
        public void SetStyle(Style style)
        {
            if (style == null)
            {
                this.IsFollowTransform = true;
                this.Fill = new BrushBase();
                this.Stroke = new BrushBase();
                this.StrokeWidth = 0;
                this.StrokeStyle = new CanvasStrokeStyle();
            }
            else
            {
                this.IsFollowTransform = style.IsFollowTransform;
                this.Fill = style.Fill;
                this.Stroke = style.Stroke;
                this.StrokeWidth = style.StrokeWidth;
                this.StrokeStyle = style.StrokeStyle;

                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        if (style.Fill.Type == BrushType.Color)
                            this.Color = style.Fill.Color;
                        break;
                    case FillOrStroke.Stroke:
                        if (style.Stroke.Type == BrushType.Color)
                            this.Color = style.Stroke.Color;
                        break;
                }
            }
        }
                     
        /// <summary> Set mode. </summary>  
        public void SetModeStyle()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    this.SetStyle(null);
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer = this.SelectionLayerage.Self;
                    this.SetStyle(layer.Style);
                    break;
                case ListViewSelectionMode.Multiple:
                    ILayer layer2 = this.SelectionLayerages.First().Self;
                    this.SetStyle(layer2.Style);
                    break;
            }
        }


        //////////////////////////


        /// <summary>
        /// Sets the <see cref="Style"/>,
        /// switch by <see cref="Retouch_Photo2.Layers.LayerType"/> to
        /// <see cref="ViewModel.GeometryStyle"/>
        /// <see cref="ViewModel.CurveStyle"/>
        /// <see cref="ViewModel.TextStyle"/>
        /// </summary>
        public Layerage StyleLayerage
        {
            set
            {
                ILayer layer = value.Self;

                //Switch
                switch (layer.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        if (value != curveStyleLayerage)
                            this.curveStyleLayerage = value;
                        break;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        if (value != textStyleLayerage)
                            this.textStyleLayerage = value;
                        break;

                    default:
                        if (value != geometryStyleLayerage)
                            this.geometryStyleLayerage = value;
                        break;
                }
            }
        }


        /// <summary>
        /// Gets the geometry style.
        /// </summary>
        public Style GeometryStyle
        {
            get
            {
                if (this.geometryStyleLayerage != null)
                {
                    ILayer layer = this.geometryStyleLayerage.Self;

                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    Style style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = BrushBase.ColorBrush(Colors.LightGray),
                    Stroke = new BrushBase(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private Layerage geometryStyleLayerage;
        
        /// <summary>
        /// Gets the curve style.
        /// </summary>
        public Style CurveStyle
        {
            get
            {
                if (this.curveStyleLayerage != null)
                {
                    ILayer layer = this.curveStyleLayerage.Self;

                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    Style style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = new BrushBase(),
                    Stroke = BrushBase.ColorBrush(Colors.Black),
                    StrokeWidth = 3,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private Layerage curveStyleLayerage;
        
        /// <summary>
        /// Gets the text style.
        /// </summary>
        public Style TextStyle
        {
            get
            {
                if (this.textStyleLayerage != null)
                {
                    ILayer layer = this.textStyleLayerage.Self;

                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    Style style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = BrushBase.ColorBrush(Colors.Black),
                    Stroke = new BrushBase(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private Layerage textStyleLayerage;

    }
}