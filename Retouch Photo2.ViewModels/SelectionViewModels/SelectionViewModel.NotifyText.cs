using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.ComponentModel;
using Windows.UI.Text;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged, ITextLayer
    {
      
        /// <summary> Gets or sets FontText. </summary>     
        public string FontText
        {
            get => this.fontText;
            set
            {
                if (this.fontText == value) return;
                this.fontText = value;
                this.OnPropertyChanged(nameof(this.FontText));//Notify 
            }
        }
        private string fontText = null;

        /// <summary> Gets or sets FontFamily. </summary>     
        public string FontFamily
        {
            get => this.fontFamily;
            set
            {
                this.fontFamily = value;
                this.OnPropertyChanged(nameof(this.FontFamily));//Notify 
            }
        }
        private string fontFamily = "Arial";
        
        /// <summary> Gets or sets FontSize. </summary>
        public float FontSize
        {
            get => this.fontSize;
            set
            {
                this.fontSize = value;
                this.OnPropertyChanged(nameof(this.FontSize));//Notify 
            }
        }
        private float fontSize = 22;


        /// <summary> Gets or sets FontAlignment. </summary>
        public CanvasHorizontalAlignment FontAlignment
        {
            get => this.fontAlignment;
            set
            {
                this.fontAlignment = value;
                this.OnPropertyChanged(nameof(this.FontAlignment));//Notify 
            }
        }
        private CanvasHorizontalAlignment fontAlignment = CanvasHorizontalAlignment.Left;
       
        /// <summary> Gets or sets FontStyle. </summary>
        public FontStyle FontStyle
        {
            get => this.fontStyle;
            set
            {
                this.fontStyle = value;
                this.OnPropertyChanged(nameof(this.FontStyle));//Notify 
            }
        }
        private FontStyle fontStyle = FontStyle.Normal;

        /// <summary> Gets or sets FontWeight. </summary>
        public FontWeight FontWeight
        {
            get => this.fontWeight;
            set
            {
                this.fontWeight = value;
                this.OnPropertyChanged(nameof(this.FontWeight));//Notify 
            }
        }
        private FontWeight fontWeight = FontWeights.Normal;

        
        /// <summary> Sets FontLayer. </summary>     
        private void SetFontLayer(ILayer layer)
        {
            if (layer == null) return;

            ITextLayer fextLayer = this.GetFontLayer(layer);
            if (fextLayer == null) return;
            
            TextLayer.FontCopyWith(this, fextLayer);
        }
        private ITextLayer GetFontLayer(ILayer layer)
        {
            if (layer == null) return null;

            if (layer.Type == LayerType.TextArtistic)
            {
                return (TextArtisticLayer)layer;
            }
            else if (layer.Type == LayerType.TextFrame)
            {
                return (TextFrameLayer)layer;
            }

            return null;
        }



    }
}