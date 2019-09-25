using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {
        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke { get; set; }

        //////////////////////////////////////

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
        private BrushType brushType = BrushType.Disabled;

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

        /// <summary> Brush. </summary>     
        public BrushPoints BrushPoints;

        /// <summary> Sets GeometryLayer's brush. </summary>     
        public void SetBrush(Brush brush)
        {
            switch (brush.Type)
            {
                case BrushType.None: break;
                case BrushType.Color: this.Color = brush.Color; break;
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.BrushPoints = brush.Points;
                    this.BrushArray = brush.Array;
                    break;
                case BrushType.Image: break;
            }

            this.BrushType = brush.Type;
        }
                       

    }
}