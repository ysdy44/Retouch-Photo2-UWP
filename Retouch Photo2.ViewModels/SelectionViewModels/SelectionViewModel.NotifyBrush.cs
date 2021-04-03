using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Styles;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        
        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                if (this.fillOrStroke == value) return;
                this.fillOrStroke = value;
                this.OnPropertyChanged(nameof(FillOrStroke));//Notify 
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
                this.OnPropertyChanged(nameof(Color));//Notify 
            }
        }
        private Color color = Colors.LightGray;


        //////////////////////////


        /// <summary> Gets or sets the style.IsFollowTransform. </summary>
        public bool IsFollowTransform
        {
            get => this.isFollowTransform;
            set
            {
                if (this.isFollowTransform == value) return;
                this.isFollowTransform = value;
                this.OnPropertyChanged(nameof(IsFollowTransform));//Notify 
            }
        }
        private bool isFollowTransform = true;

        /// <summary> Gets or sets whether the stroke is behind the fill. </summary>
        public bool IsStrokeBehindFill
        {
            get => this.isStrokeBehindFill;
            set
            {
                if (this.isStrokeBehindFill == value) return;
                this.isStrokeBehindFill = value;
                this.OnPropertyChanged(nameof(IsStrokeBehindFill));//Notify 
            }
        }
        private bool isStrokeBehindFill = false;

        /// <summary> Gets or sets the current fill. </summary>
        public IBrush Fill
        {
            get => this.fill;
            set
            {
                this.fill = value;
                this.OnPropertyChanged(nameof(Fill));//Notify 
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
                this.OnPropertyChanged(nameof(Stroke));//Notify 
            }
        }
        private IBrush stroke = new BrushBase();
        
        /// <summary> Gets or sets the style.IsStrokeWidthFollowScale. </summary>
        public bool IsStrokeWidthFollowScale
        {
            get => this.isStrokeWidthFollowScale;
            set
            {
                if (this.isStrokeWidthFollowScale == value) return;
                this.isStrokeWidthFollowScale = value;
                this.OnPropertyChanged(nameof(IsStrokeWidthFollowScale));//Notify 
            }
        }
        private bool isStrokeWidthFollowScale = false;
                
        /// <summary> Gets or sets the current stroke-width. </summary>
        public float StrokeWidth
        {
            get => this.strokeWidth;
            set
            {
                this.strokeWidth = value;
                this.OnPropertyChanged(nameof(StrokeWidth));//Notify 
            }
        }
        private float strokeWidth = 1;


        //////////////////////////


        /// <summary> Gets the current stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle { get; private set; } = new CanvasStrokeStyle();


        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashStyle"/>. </summary>
        public CanvasDashStyle StrokeStyle_Dash
        {
            get => this.strokeStyle_Dash;
            set
            {
                this.strokeStyle_Dash = value;
                this.OnPropertyChanged(nameof(StrokeStyle_Dash));//Notify 
                this.StrokeStyle.DashStyle = value;
                this.OnPropertyChanged(nameof(StrokeStyle));//Notify 
            }
        }
        private CanvasDashStyle strokeStyle_Dash = CanvasDashStyle.Solid;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashCap"/>. </summary>
        public CanvasCapStyle StrokeStyle_Cap
        {
            get => this.strokeStyle_Cap;
            set
            {
                this.strokeStyle_Cap = value;
                this.OnPropertyChanged(nameof(StrokeStyle_Cap));//Notify 
                this.StrokeStyle.DashCap = value;
                this.OnPropertyChanged(nameof(StrokeStyle));//Notify 
            }
        }
        private CanvasCapStyle strokeStyle_Cap = CanvasCapStyle.Flat;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.LineJoin"/>. </summary>
        public CanvasLineJoin StrokeStyle_Join
        {
            get => this.strokeStyle_Join;
            set
            {
                this.strokeStyle_Join = value;
                this.OnPropertyChanged(nameof(StrokeStyle_Join));//Notify 
                this.StrokeStyle.LineJoin = value;
                this.OnPropertyChanged(nameof(StrokeStyle));//Notify 
            }
        }
        private CanvasLineJoin strokeStyle_Join = CanvasLineJoin.Miter;
        
        /// <summary> Gets or sets the current stroke-style's <see cref="CanvasStrokeStyle.DashOffset"/>. </summary>
        public float StrokeStyle_Offset
        {
            get => this.strokeStyle_Offset;
            set
            {
                this.strokeStyle_Offset = value;
                this.OnPropertyChanged(nameof(StrokeStyle_Offset));//Notify 
                this.StrokeStyle.DashOffset = value;
                this.OnPropertyChanged(nameof(StrokeStyle));//Notify 
            }
        }
        private float strokeStyle_Offset = 0.0f;


        //////////////////////////


        /// <summary> Gets or sets the current transparency. </summary>
        public IBrush Transparency
        {
            get => this.transparency;
            set
            {
                this.transparency = value;
                this.OnPropertyChanged(nameof(Transparency));//Notify 
            }
        }
        private IBrush transparency = new BrushBase();


        //////////////////////////


        /// <summary> Sets the style. </summary>  
        public void SetStyle(IStyle style)
        {
            if (style == null) return;


            this.IsFollowTransform = style.IsFollowTransform;

            this.IsStrokeBehindFill = style.IsStrokeBehindFill;
            
            this.Fill = style.Fill;
            this.Stroke = style.Stroke;

            this.IsStrokeWidthFollowScale = style.IsStrokeWidthFollowScale;

            this.StrokeWidth = style.StrokeWidth;

            this.StrokeStyle = style.StrokeStyle;

            this.StrokeStyle_Dash = style.StrokeStyle.DashStyle;
            this.StrokeStyle_Cap = style.StrokeStyle.DashCap;
            this.strokeStyle_Join = style.StrokeStyle.LineJoin;
            this.StrokeStyle_Offset = style.StrokeStyle.DashOffset;

            this.Transparency = style.Transparency;


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