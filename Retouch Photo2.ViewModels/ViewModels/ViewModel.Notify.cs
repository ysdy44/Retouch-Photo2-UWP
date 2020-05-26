using FanKit.Transformers;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Retouch_Photo2's the only AccentColor. </summary>
        public Color AccentColor { get; set; }

        
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
               

        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.TipText" />. </summary>
        public string TipText
        {
            get => this.tipText;
            set
            {
                this.tipText = value;
                this.OnPropertyChanged(nameof(this.TipText));//Notify 
            }
        }
        private string tipText = string.Empty;
        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.TipTextVisibility" />. </summary>
        public Visibility TipTextVisibility
        {
            get => this.tipTextVisibility;
            set
            {
                this.tipTextVisibility = value;
                this.OnPropertyChanged(nameof(this.TipTextVisibility));//Notify 
            }
        }
        private Visibility tipTextVisibility = Visibility.Collapsed;

        public async void TipTextBegin(string text)
        {
            this.TipText = text;
            this.TipTextVisibility = Visibility.Visible;
            await Task.Delay(2000);
            this.TipTextVisibility = Visibility.Collapsed;
        }
        public void SetTipText()
        {
            if (this.TipText.Length > 44) this.TipText = string.Empty;
            else this.TipText += "O";
        }


        int _width;
        int _height;
        public void SetTipTextWidthHeight(Transformer transformer)
        {
            Vector2 horizontal = transformer.Horizontal;
            Vector2 vertical = transformer.Vertical;

            int width = (int)horizontal.Length();
            int height = (int)vertical.Length();

            if (this._width != width || this._height != height)
            {
                this._width = width;
                this._height = height;
                this.TipText = $"W: {width} px  H:{height} px";
            }
        }

        int _x;
        int _y;
        public void SetTipTextPosition()
        {
            int x = (int)this.CanvasTransformer.Position.X;
            int y = (int)this.CanvasTransformer.Position.X;

            if (this._x != x || this._y != y)
            {
                this._x = x;
                this._y = y;
                this.TipText = $"X: {x} px  Y:{y} px";
            }
        }

        int _percent;
        public void SetTipTextScale()
        {
            int percent = (int)(this.CanvasTransformer.Scale * 100);

            if (this._percent != percent)
            {
                this._percent = percent;
                this.TipText = $"{percent} %";
            }
        }
        

    }
}