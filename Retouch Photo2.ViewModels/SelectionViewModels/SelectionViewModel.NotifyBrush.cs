using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Styles;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the style.IsFollowTransform. </summary>
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

                

        /// <summary> Gets or sets the current color. </summary>
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


        /// <summary> Gets or sets the current fill. </summary>
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

        /// <summary> Gets or sets the current stroke. </summary>
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

        /// <summary> Gets or sets the current stroke-width. </summary>
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


        //////////////////////////


        /// <summary> Gets the current stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle { get; private set; } = new CanvasStrokeStyle();


        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashStyle"/>. </summary>
        public CanvasDashStyle StrokeStyleDash
        {
            get => this.strokeStyleDash;
            set
            {
                this.strokeStyleDash = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleDash));//Notify 
                this.StrokeStyle.DashStyle = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasDashStyle strokeStyleDash = CanvasDashStyle.Solid;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashCap"/>. </summary>
        public CanvasCapStyle StrokeStyleCap
        {
            get => this.strokeStyleCap;
            set
            {
                this.strokeStyleCap = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleCap));//Notify 
                this.StrokeStyle.DashCap = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasCapStyle strokeStyleCap = CanvasCapStyle.Flat;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.LineJoin"/>. </summary>
        public CanvasLineJoin StrokeStyleJoin
        {
            get => this.strokeStyleJoin;
            set
            {
                this.strokeStyleJoin = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleJoin));//Notify 
                this.StrokeStyle.LineJoin = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasLineJoin strokeStyleJoin = CanvasLineJoin.Miter;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashOffset"/>. </summary>
        public float StrokeStyleOffset
        {
            get => this.strokeStyleOffset;
            set
            {
                this.strokeStyleOffset = value;
                this.OnPropertyChanged(nameof(this.StrokeStyleOffset));//Notify 
                this.StrokeStyle.DashOffset = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private float strokeStyleOffset = 0.0f;
      

        //////////////////////////


        /// <summary> Sets the style. </summary>  
        public void SetStyle(Style style)
        {
            if (style == null) return;

            this.IsFollowTransform = style.IsFollowTransform;
            this.Fill = style.Fill;
            this.Stroke = style.Stroke;
            this.StrokeWidth = style.StrokeWidth;

            this.StrokeStyle = style.StrokeStyle;

            this.StrokeStyleDash = style.StrokeStyle.DashStyle;
            this.StrokeStyleCap = style.StrokeStyle.DashCap;
            this.strokeStyleJoin = style.StrokeStyle.LineJoin;
            this.StrokeStyleOffset = style.StrokeStyle.DashOffset;

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
}