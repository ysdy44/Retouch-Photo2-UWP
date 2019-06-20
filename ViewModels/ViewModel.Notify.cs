using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {
                     
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

        /// <summary> Flyout of <see cref = "ViewModel.FillColor" />.  </summary>
        public Flyout FillColorFlyout;

        /// <summary> ColorPicker of <see cref = "ViewModel.FillColor" />.  </summary>
        public HSVColorPickers.ColorPicker FillColorPicker;



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

        /// <summary> Flyout of <see cref = "ViewModel.FillColor" />.  </summary>
        public Flyout StrokeColorFlyout;

        /// <summary> ColorPicker of <see cref = "ViewModel.StrokeColor" />.  </summary>
        public HSVColorPickers.ColorPicker StrokeColorPicker;
         

        /// <summary> Sets or Gets the on state of the ruler on the canvas. </summary>
        public bool IsRuler
        {
            get => this.isRuler;
            set
            {
                this.isRuler = value;
                this.OnPropertyChanged(nameof(this.IsRuler));//Notify 
            }
        }
        private bool isRuler;


        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.Text" />. </summary>
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));//Notify 
            }
        }
        private string text = string.Empty;
        /// <summary> Sets's the <see cref = "ViewModel.Text" />. </summary>
        public void SetText()
        {
            if (App.ViewModel.Text.Length > 44) App.ViewModel.Text = string.Empty;
            else App.ViewModel.Text += "O";
        }


    }
}