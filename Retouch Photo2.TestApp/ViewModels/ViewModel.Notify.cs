using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
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
