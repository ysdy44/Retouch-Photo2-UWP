using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke { get; private set; } = FillOrStroke.Fill;

        /// <summary> Gets StyleManager. </summary>
        public StyleManager StyleManager { get; } = new StyleManager
        {
            FillBrush = new Brush
            {
                Type = BrushType.Color,
                Color = Color.FromArgb(255, 214, 214, 214),
            },
            StrokeBrush = new Brush
            {
                Type = BrushType.Color,
                Color = Color.FromArgb(255, 0, 0, 0),
            },
            StrokeWidth = 1,
        };


        #region Color


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
        private Color color = Color.FromArgb(255, 214, 214, 214);

        /// <summary> Retouch_Photo2's the only fill-color. </summary>
        public Color FillColor
        {
            get => this.fillColor;
            set
            {
                this.fillColor = value;
                this.OnPropertyChanged(nameof(this.FillColor));//Notify 
            }
        }
        private Color fillColor = Color.FromArgb(255, 214, 214, 214);

        /// <summary> Retouch_Photo2's the only stroke-color. </summary>
        public Color StrokeColor
        {
            get => this.strokeColor;
            set
            {
                this.strokeColor = value;
                this.OnPropertyChanged(nameof(this.StrokeColor));//Notify 
            }
        }
        private Color strokeColor = Color.FromArgb(255, 0, 0, 0);


        /// <summary> Retouch_Photo2's the only stroke-width. </summary>
        public float StrokeWidth
        {
            get => this.StyleManager.StrokeWidth;
            set
            {
                this.StyleManager.StrokeWidth = value;
                this.OnPropertyChanged(nameof(this.StrokeWidth));//Notify 
            }
        }


        #endregion


        #region Brush


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

        /// <summary> Brush points. </summary>     
        public BrushPoints BrushPoints
        {
            get => this.brushPoints;
            set
            {
                this.brushPoints = value;
                this.OnPropertyChanged(nameof(this.BrushPoints));//Notify 
            }
        }
        private BrushPoints brushPoints;


        #endregion


        /// <summary>
        /// Points turn to _oldPoints in Transformer.One.
        /// </summary>
        public void OneBrushPoints()
        {
            Transformer transformer = this.Transformer;
            this.StyleManager.FillBrush.OneBrushPoints(transformer);
            this.StyleManager.StrokeBrush.OneBrushPoints(transformer);
        }
        /// <summary>
        /// _oldPoints turn to Points in Transformer Destination.
        /// </summary>
        /// <param name="layer"> The source layer. </param>
        public void DeliverBrushPoints(ILayer layer)
        {
            Transformer transformer = layer.TransformManager.Destination;
            layer.StyleManager.FillBrush.DeliverBrushPoints(transformer);
            layer.StyleManager.StrokeBrush.DeliverBrushPoints(transformer);
        }


        /// <summary> Sets FillOrStroke. </summary>  
        public void SetFillOrStroke(FillOrStroke fillOrStroke)
        {
            this.FillOrStroke = fillOrStroke;

            switch (fillOrStroke)
            {
                case FillOrStroke.Fill: this.SetBrush(this.StyleManager.FillBrush, FillOrStroke.Fill); break;
                case FillOrStroke.Stroke: this.SetBrush(this.StyleManager.StrokeBrush, FillOrStroke.Stroke); break;
            }
        }
        /// <summary> Sets brush. </summary>  
        public void SetBrush(Brush brush, FillOrStroke fillOrStroke)
        {
            BrushType brushType = brush.Type;
            this.BrushType = brushType;

            switch (brushType)
            {
                case BrushType.None: break;
                case BrushType.Color:
                    this.Color = brush.Color;
                    switch (fillOrStroke)
                    {
                        case FillOrStroke.Fill: this.FillColor = color; break;
                        case FillOrStroke.Stroke: this.StrokeColor = color; break;
                    }
                    break;
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.BrushArray = brush.Array;
                    this.BrushPoints = brush.Points;
                    break;
                case BrushType.Image: break;
            }
        }
        /// <summary> Sets style-manager. </summary>  
        public void SetStyleManager(StyleManager styleManager)
        {
            if (styleManager == null)
            {
                this.BrushType = BrushType.None;
                return;
            }

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill: this.SetBrush(styleManager.FillBrush, FillOrStroke.Fill); break;
                case FillOrStroke.Stroke: this.SetBrush(styleManager.StrokeBrush, FillOrStroke.Stroke); break;
            }

            this.StyleManager.CopyWith(StyleManager);
        }
        
    }
}