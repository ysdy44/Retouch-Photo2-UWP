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

        /// <summary> Brush's IsFollowTransform. </summary>     
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

        /// <summary> Sets color (width fill-brush and stroke-brush). </summary>  
        public void SetColor(Color color, FillOrStroke fillOrStroke)
        {
            if (this.FillOrStroke == fillOrStroke) this.Color = color;

            switch (fillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillBrush = new ColorBrush(color);
                    this.SetValue((layer) =>
                    {
                        layer.StyleManager.FillBrush = new ColorBrush(color);
                        this.StyleLayer = layer;
                    });
                    break;

                case FillOrStroke.Stroke:
                    this.StrokeBrush = new ColorBrush(color);
                    this.SetValue((layer) =>
                    {
                        layer.StyleManager.StrokeBrush = new ColorBrush(color);
                        this.StyleLayer = layer;
                    });
                    break;
            }
        }


        //////////////////////////


        /// <summary> Retouch_Photo2's the only fill-brush. </summary>
        public IBrush FillBrush
        {
            get => this.fillBrush;
            set
            {
                this.fillBrush = value;
                this.OnPropertyChanged(nameof(this.FillBrush));//Notify 
            }
        }
        private IBrush fillBrush = new ColorBrush(Colors.LightGray);

        /// <summary> Retouch_Photo2's the only stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            get => this.strokeBrush;
            set
            {
                this.strokeBrush = value;
                this.OnPropertyChanged(nameof(this.StrokeBrush));//Notify 
            }
        }
        private IBrush strokeBrush = new NoneBrush();

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

        /// <summary> Sets style-manager. </summary>  
        private void SetStyleManager(StyleManager styleManager)
        {
            if (styleManager == null)
            {
                this.IsFollowTransform = true;
                this.FillBrush = new NoneBrush();
                this.StrokeBrush = new NoneBrush();
                this.StrokeWidth = 0;
                this.StrokeStyle = new CanvasStrokeStyle();
            }
            else
            {
                this.IsFollowTransform = styleManager.IsFollowTransform;
                this.FillBrush = styleManager.FillBrush;
                this.StrokeBrush = styleManager.StrokeBrush;
                this.StrokeWidth = styleManager.StrokeWidth;
                this.StrokeStyle = styleManager.StrokeStyle;

                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        if (styleManager.FillBrush.Type == BrushType.Color)
                            this.Color = styleManager.FillBrush.Color;
                        break;
                    case FillOrStroke.Stroke:
                        if (styleManager.StrokeBrush.Type == BrushType.Color)
                            this.Color = styleManager.StrokeBrush.Color;
                        break;
                }
            }
        }
                     
        /// <summary> Set mode. </summary>  
        public void SetModeStyleManager()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    this.SetStyleManager(null);
                    break;
                case ListViewSelectionMode.Single:
                    this.SetStyleManager(this.Layer.StyleManager);
                    break;
                case ListViewSelectionMode.Multiple:
                    ILayer firstLayer = this.Layers.First();
                    this.SetStyleManager(firstLayer.StyleManager);
                    break;
            }
        }


        //////////////////////////


        /// <summary>
        /// Sets the <see cref="StyleManager"/>,
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
        public StyleManager GeometryStyle
        {
            get
            {
                if (this.geometryStylelayer != null)
                {
                    //CacheBrush
                    Transformer transformer = this.geometryStylelayer.TransformManager.Destination;
                    StyleManager styleManager = this.geometryStylelayer.StyleManager.Clone();
                    styleManager.CacheBrush(transformer);
                    return styleManager;
                }

                return new StyleManager
                {
                    IsFollowTransform = true,
                    FillBrush = new ColorBrush(Colors.LightGray),
                    StrokeBrush = new NoneBrush(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private ILayer geometryStylelayer;
        
        /// <summary>
        /// Gets the curve style.
        /// </summary>
        public StyleManager CurveStyle
        {
            get
            {
                if (this.curveStyleLayer != null)
                {
                    //CacheBrush
                    Transformer transformer = this.curveStyleLayer.TransformManager.Destination;
                    StyleManager styleManager = this.curveStyleLayer.StyleManager.Clone();
                    styleManager.CacheBrush(transformer);
                    return styleManager;
                }

                return new StyleManager
                {
                    IsFollowTransform = true,
                    FillBrush = new NoneBrush(),
                    StrokeBrush = new ColorBrush(Colors.Black),
                    StrokeWidth = 3,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private ILayer curveStyleLayer;
        
        /// <summary>
        /// Gets the text style.
        /// </summary>
        public StyleManager TextStyle
        {
            get
            {
                if (this.textStyleLayer != null)
                {
                    //CacheBrush
                    Transformer transformer = this.textStyleLayer.TransformManager.Destination;
                    StyleManager styleManager = this.textStyleLayer.StyleManager.Clone();
                    styleManager.CacheBrush(transformer);
                    return styleManager;
                }

                return new StyleManager
                {
                    IsFollowTransform = true,
                    FillBrush = new ColorBrush(Colors.Black),
                    StrokeBrush = new NoneBrush(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        private ILayer textStyleLayer;

    }
}