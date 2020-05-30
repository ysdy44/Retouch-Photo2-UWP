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

        /// <summary> Gets or sets the current stroke-style. </summary>
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

        /// <summary> Sets the style. </summary>  
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

    }
}