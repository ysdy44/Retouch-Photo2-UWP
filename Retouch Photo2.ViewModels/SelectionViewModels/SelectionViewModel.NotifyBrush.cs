using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
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

        /// <summary> Sets color (width fill and stroke). </summary>  
        public void SetColor(Color color, FillOrStroke fillOrStroke)
        {
            if (this.FillOrStroke == fillOrStroke) this.Color = color;

            switch (fillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Fill = new ColorBrush(color);
                    this.SetValue((layer) =>
                    {
                        layer.Style.Fill = new ColorBrush(color);
                        this.StyleLayer = layer;
                    });
                    break;

                case FillOrStroke.Stroke:
                    this.Stroke = new ColorBrush(color);
                    this.SetValue((layer) =>
                    {
                        layer.Style.Stroke = new ColorBrush(color);
                        this.StyleLayer = layer;
                    });
                    break;
            }
        }


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
        private IBrush fill = new ColorBrush(Colors.LightGray);

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
        private IBrush stroke = new NoneBrush();

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
        private void SetStyle(Style style)
        {
            if (style == null)
            {
                this.IsFollowTransform = true;
                this.Fill = new NoneBrush();
                this.Stroke = new NoneBrush();
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
                    this.SetStyle((Style)this.Layer.Style);
                    break;
                case ListViewSelectionMode.Multiple:
                    ILayer firstLayer = this.Layers.First();
                    this.SetStyle((Style)firstLayer.Style);
                    break;
            }
        }


        //////////////////////////


        /// <summary>
        /// Sets the <see cref="Style"/>,
        /// switch by <see cref="LayerType"/> to
        /// <see cref="SelectionViewModel.GeometryStyle"/>
        /// <see cref="SelectionViewModel.CurveStyle"/>
        /// <see cref="SelectionViewModel.TextStyle"/>
        /// </summary>
        public ILayer StyleLayer
        {
            set
            {                
                //Switch
                switch (value.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        if (value != curveStyleLayer)
                            this.curveStyleLayer = value;
                        break;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        if (value != textStyleLayer)
                            this.textStyleLayer = value;
                        break;

                    default:
                        if (value != geometryStylelayer)
                            this.geometryStylelayer = value;
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
                if (this.geometryStylelayer != null)
                {
                    //CacheBrush
                    Transformer transformer = this.geometryStylelayer.Transform.Destination;
                    Style style = this.geometryStylelayer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = new ColorBrush(Colors.LightGray),
                    Stroke = new NoneBrush(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private ILayer geometryStylelayer;
        
        /// <summary>
        /// Gets the curve style.
        /// </summary>
        public Style CurveStyle
        {
            get
            {
                if (this.curveStyleLayer != null)
                {
                    //CacheBrush
                    Transformer transformer = this.curveStyleLayer.Transform.Destination;
                    Style style = this.curveStyleLayer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = new NoneBrush(),
                    Stroke = new ColorBrush(Colors.Black),
                    StrokeWidth = 3,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private ILayer curveStyleLayer;
        
        /// <summary>
        /// Gets the text style.
        /// </summary>
        public Style TextStyle
        {
            get
            {
                if (this.textStyleLayer != null)
                {
                    //CacheBrush
                    Transformer transformer = this.textStyleLayer.Transform.Destination;
                    Style style = this.textStyleLayer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = new ColorBrush(Colors.Black),
                    Stroke = new NoneBrush(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private ILayer textStyleLayer;

    }
}