using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Retouch_Photo2's the only AccentColor. </summary>
        public Color AccentColor { get; set; }


        /// <summary> Sets or Gets the on state of the ruler on the canvas. </summary>
        public bool CanvasRulerVisible
        {
            get => this.canvasRulerVisible;
            set
            {
                this.canvasRulerVisible = value;
                this.OnPropertyChanged(nameof(this.CanvasRulerVisible));//Notify 
            }
        }
        private bool canvasRulerVisible;
        
        /// <summary> Sets or Gets the on state of the IsHitTestVisible on the canvas. </summary>
        public bool CanvasHitTestVisible
        {
            get => this.canvasHitTestVisible;
            set
            {
                this.canvasHitTestVisible = value; 
                this.OnPropertyChanged(nameof(this.CanvasHitTestVisible));//Notify 
            }
        }
        private bool canvasHitTestVisible = true;
               

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
            if (this.Text.Length > 44) this.Text = string.Empty;
            else this.Text += "O";
        }
    }
}