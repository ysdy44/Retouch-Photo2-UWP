using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

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


        ///////////////////////////////////////////////////////////////////////////////////////


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

        /// <summary> Flyout of <see cref = "ViewModel.FillColor" />. </summary>
        public Flyout FillColorFlyout { get; set; }

        /// <summary> ColorPicker of <see cref = "ViewModel.FillColor" />. </summary>
        public HSVColorPickers.ColorPicker FillColorPicker { get; set; }


        ///////////////////////////////////////////////////////////////////////////////////////


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

        /// <summary> Flyout of <see cref = "ViewModel.FillColor" />. </summary>
        public Flyout StrokeColorFlyout { get; set; }

        /// <summary> ColorPicker of <see cref = "ViewModel.StrokeColor" />. </summary>
        public HSVColorPickers.ColorPicker StrokeColorPicker { get; set; }


        ///////////////////////////////////////////////////////////////////////////////////////


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
    }
}
