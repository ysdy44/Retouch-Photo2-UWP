using FanKit.Transformers;
using System.ComponentModel;
using System.Numerics;
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


        /// <summary> Sets or Gets the on state of the ruler on the canvas. </summary>
        public bool CanvasRulerVisible
        {
            get => this.canvasRulerVisible;
            set
            {
                this.Invalidate();//Invalidate

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
        /// <summary> Retouch_Photo2's the only <see cref = "ViewModel.TextVisibility" />. </summary>
        public Visibility TextVisibility
        {
            get => this.textVisibility;
            set
            {
                this.textVisibility = value;
                this.OnPropertyChanged(nameof(this.TextVisibility));//Notify 
            }
        }
        private Visibility textVisibility = Visibility.Collapsed;

        public void SetText()
        {
            if (this.Text.Length > 44) this.Text = string.Empty;
            else this.Text += "O";
        }


        int _width;
        int _height;
        public void SetTextWidthHeight(Transformer transformer)
        {
            Vector2 horizontal = transformer.Horizontal;
            Vector2 vertical = transformer.Vertical;

            int width = (int)horizontal.Length();
            int height = (int)vertical.Length();

            if (this._width != width || this._height != height)
            {
                this._width = width;
                this._height = height;
                this.Text = $"W: {width} px  H:{height} px";
            }
        }

        int _x;
        int _y;
        public void SetTextPosition()
        {
            int x = (int)this.CanvasTransformer.Position.X;
            int y = (int)this.CanvasTransformer.Position.X;

            if (this._x != x || this._y != y)
            {
                this._x = x;
                this._y = y;
                this.Text = $"X: {x} px  Y:{y} px";
            }
        }

        int _percent;
        public void SetTextScale()
        {
            int percent = (int)(this.CanvasTransformer.Scale * 100);

            if (this._percent != percent)
            {
                this._percent = percent;
                this.Text = $"{percent} %";
            }
        }
        

    }
}