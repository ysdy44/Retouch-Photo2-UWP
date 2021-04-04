// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★★
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer control.
    /// </summary>
    public partial class LayerControl : UserControl
    {


        //@Construct
        /// <summary>
        /// Initializes a layer-control.
        /// </summary>      
        /// <param name="controlHeight"> The control-height. </param>
        /// <param name="type"> The type. </param>
        public LayerControl(int controlHeight, string type)
        {
            this.InitializeComponent();
            this.ControlHeight = controlHeight;
            this.Type = type;
        }
        /// <summary>
        /// Initializes a layer-control.
        /// </summary>      
        /// <param name="layer"> The layer. </param>
        public LayerControl(ILayer layer)
        {
            this.InitializeComponent();
            this.ConstructStringsCore(layer);
            this.ControlHeight = LayerManager.ControlsHeight;

            this.ConstructIcon(LayerManager.CanvasDevice);
            this.ConstructTapped(layer);
            this.ConstructButton(layer);
            this.ConstructManipulation(layer);
            this.ConstructPointer(layer);
        }

    }
}