using Windows.UI.Xaml;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class Layer 
    {

        /// <summary> Gets or sets <see cref = "Layer" />'s control. </summary>
        public LayerControl Control { get; protected set; } 

               
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