// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★★
using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer control.
    /// </summary>
    public partial class LayerControl : UserControl
    {

        //@Content
        /// <summary> Self </summary>
        public LayerControl Self => this;


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
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layer"> The layer. </param>
        public LayerControl(CanvasDevice customDevice, ILayer layer)
        {
            this.InitializeComponent();
            this.ControlHeight = LayerageCollection.ControlsHeight;
            this.ConstructIcon(customDevice);
            this.ConstructTapped(layer);
            this.ConstructButton(layer);
            this.ConstructManipulation(layer);
            this.ConstructPointer(layer);
        }

    }
}