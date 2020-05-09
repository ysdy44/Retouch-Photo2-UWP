using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

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
        
        public void SetFillColor(Color color)
        {
            //Selection
            this.FillBrush = new ColorBrush(color);
            this.SetValue((layer) =>
            {
                switch (layer.StyleManager.FillBrush.Type)
                {
                    case BrushType.Image:
                        break;
                    default:
                        layer.StyleManager.FillBrush = new ColorBrush(color);
                        break;
                }
            });
        }
        public void SetStrokeColor(Color color)
        {
            //Selection
            this.StrokeBrush = new ColorBrush(color);
            this.SetValue((layer) =>
            {
                switch (layer.StyleManager.StrokeBrush.Type)
                {
                    case BrushType.Image:
                        break;
                    default:
                        layer.StyleManager.StrokeBrush = new ColorBrush(color);
                        break;
                }
            });
        }



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
        private IBrush strokeBrush = new ColorBrush(Colors.Black);
        
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
        


        /// <summary> Gets style-manager. </summary>  
        public StyleManager GetStyleManager()
        {
            return new StyleManager
            {
                FillBrush = this.FillBrush.Clone(),
                StrokeBrush = this.StrokeBrush.Clone(),
                StrokeWidth = this.StrokeWidth
            };
        }
        /// <summary> Sets style-manager. </summary>  
        public void SetStyleManager(StyleManager styleManager)
        {
            if (styleManager == null) return;

            this.FillBrush = styleManager.FillBrush;
            this.StrokeBrush = styleManager.StrokeBrush;
            this.StrokeWidth = styleManager.StrokeWidth;

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
}