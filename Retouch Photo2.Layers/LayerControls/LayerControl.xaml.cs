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
        public LayerControl Self => this;
        public string Text { get => this.NameRun.Text; set => this.NameRun.Text = value; }
        public string Type { get => this.TypeRun.Text; set => this.TypeRun.Text = value; }


        /// <summary> Gets or sets the height. </summary>
        public int ControlHeight
        {
            get => this.controlHeight;
            set
            {
                this.SetControlHeight(value);
                this.controlHeight = value;
            }
        }
        private int controlHeight = 40;

        /// <summary> Gets or sets the depth. </summary>
        public int Depth
        {
            get => this.depth;
            set
            {
                this.SetDepth(value);
                this.depth = value;
            }
        }
        private int depth = 0;

        /// <summary> Gets or sets the overlay show status. </summary>
        public OverlayMode OverlayMode
        {
            get => this.overlayMode;
            set
            {
                if (this.overlayMode == value) return;
                this.SetOverlayMode(value);
                this.overlayMode = value;
            }
        }
        private OverlayMode overlayMode;

        /// <summary> Gets or sets the icon. </summary>
        public ICanvasImage IconRender
        {
            get => this.iconRender;
            set
            {
                this.iconRender = value;
                this.IconCanvasControl.Invalidate();
            }
        }
        private ICanvasImage iconRender = null;


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