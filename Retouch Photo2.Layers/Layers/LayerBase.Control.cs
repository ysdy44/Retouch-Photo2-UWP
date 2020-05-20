using Windows.UI.Xaml;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase 
    {

        /// <summary> Gets or sets <see cref = "LayerBase" />'s control. </summary>
        public ILayerControl Control { get; protected set; }
        

        /// <summary> Gets or sets <see cref = "LayerControl" />'s overlay show status. </summary>
        public OverlayMode OverlayMode
        {
            get => this.overlayMode;
            set
            {
                if (this.overlayMode == value) return;
                this.Control.SetOverlayMode(value);
                this.overlayMode = value;
            }
        }
        private OverlayMode overlayMode;

               
        public bool IsExpand
        {
            get => this.isExpand;
            set
            {
                this.Control.SetIsExpand(value);
                this.isExpand = value;
            }
        }
        private bool isExpand;


        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.Control.SetIsSelected(value);
                this.isSelected = value;
            }
        }
        private bool isSelected;
        
    }
}