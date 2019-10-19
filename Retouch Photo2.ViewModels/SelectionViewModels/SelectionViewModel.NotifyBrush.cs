using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;

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
            get
            {
                if (this.FillOrStroke == FillOrStroke.Fill)
                    return this.StyleManager.FillBrush.Type;
                else
                    return this.StyleManager.StrokeBrush.Type;
            }
            set
            {
                if (this.FillOrStroke == FillOrStroke.Fill)
                    this.StyleManager.FillBrush.Type = value;
                else
                    this.StyleManager.StrokeBrush.Type = value;

                this.OnPropertyChanged(nameof(this.BrushType));//Notify 
            }
        }

        /// <summary> Brush's gradient stops. </summary>     
        public CanvasGradientStop[] BrushArray
        {
            get
            {
                if (this.FillOrStroke == FillOrStroke.Fill)
                    return this.StyleManager.FillBrush.Array;
                else
                    return this.StyleManager.StrokeBrush.Array;
            }
            set
            {
                if (this.FillOrStroke == FillOrStroke.Fill)
                    this.StyleManager.FillBrush.Array = value;
                else
                    this.StyleManager.StrokeBrush.Array = value;

                this.OnPropertyChanged(nameof(this.BrushArray));//Notify 
            }
        }

        /// <summary> Brush points. </summary>     
        public BrushPoints BrushPoints
        {
            get
            {
                if (this.FillOrStroke == FillOrStroke.Fill)
                    return this.StyleManager.FillBrush.Points;
                else
                    return this.StyleManager.StrokeBrush.Points;
            }
            set
            {
                if (this.FillOrStroke == FillOrStroke.Fill)
                    this.StyleManager.FillBrush.Points = value;
                else
                    this.StyleManager.StrokeBrush.Points = value;

                this.OnPropertyChanged(nameof(this.BrushPoints));//Notify 
            }
        }


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

            this.OnPropertyChanged(nameof(this.BrushType));//Notify 
            this.OnPropertyChanged(nameof(this.BrushArray));//Notify 
            this.OnPropertyChanged(nameof(this.BrushPoints));//Notify 
        }

    }
}