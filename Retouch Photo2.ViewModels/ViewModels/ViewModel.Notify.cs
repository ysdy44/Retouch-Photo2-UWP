using FanKit.Transformers;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the accent color. </summary>
        public Color AccentColor { get; set; } = Colors.DodgerBlue;


        /// <summary> Gets or sets the <see cref="ToolTip.IsOpen"/>. </summary>
        public bool IsOpen
        {
            get => this.isOpen;
            set
            {
                this.isOpen = value;
                this.OnPropertyChanged(nameof(this.IsOpen));//Notify 
            }
        }
        private bool isOpen;


        /// <summary> Gets or sets the tip text. </summary>
        public string TipText
        {
            get => this.tipText;
            set
            {
                if (this.tipText == value) return;
                this.tipText = value;
                this.OnPropertyChanged(nameof(TipText));//Notify 
            }
        }
        private string tipText = string.Empty;
        /// <summary> Gets or sets the visibility of tip text. </summary>
        public Visibility TipTextVisibility
        {
            get => this.tipTextVisibility;
            set
            {
                if (this.tipTextVisibility == value) return;
                this.tipTextVisibility = value;
                this.OnPropertyChanged(nameof(TipTextVisibility));//Notify 
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


        public void SetTipTextWidthHeight(Transformer transformer)
        {
            Vector2 horizontal = transformer.Horizontal;
            Vector2 vertical = transformer.Vertical;

            int width = (int)horizontal.Length();
            int height = (int)vertical.Length();

            this.TipText = $"{width} {height}";
        }

        public void SetTipTextPosition()
        {
            int x = (int)this.CanvasTransformer.Position.X;
            int y = (int)this.CanvasTransformer.Position.X;

            this.TipText = $"{x} {y}";
        }

        public void SetTipTextScale()
        {
            int percent = (int)(this.CanvasTransformer.Scale * 100.0f);

            this.TipText = $"{percent} %";
        }

    }
}