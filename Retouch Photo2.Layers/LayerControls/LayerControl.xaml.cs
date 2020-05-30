using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
    public partial class LayerControl : UserControl
    {

        //@Content
        public LayerControl Self => this;
        public string Text { get => this.NameRun.Text; set=>this.NameRun.Text = value; }
        public string Type { get => this.TypeRun.Text; set => this.TypeRun.Text = value; }


        private int controlHeight = 40;
        public int ControlHeight
        {
            get => this.controlHeight;
            set
            {
                this.Height = value;
                this.IconBorder.Width = value;
                this.IconBorder.Height = value;

                //Overlay
                {
                    double heightOver7 = value / 7;
                    this.OverlayShowTopBorder.Height = heightOver7 + heightOver7;
                    this.OverlayShowCenterBorder.Height = heightOver7 + heightOver7 + heightOver7;
                    this.OverlayShowBottomBorder.Height = heightOver7 + heightOver7;
                }

                this.controlHeight = value;
            }
        }


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
        

        private int depth = 0;
        public int Depth
        {
            get => this.depth;
            set
            {
                double pixels = value * 20.0d;
                GridLength gridLength = new GridLength(pixels, GridUnitType.Pixel);
                this.DepthColumn.Width = gridLength;

                this.depth = value;
            }
        }


        /// <summary> Gets or sets <see cref = "LayerControl" />'s overlay show status. </summary>
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

            //IconCanvasControl
            this.IconCanvasControl.UseSharedDevice = true;
            this.IconCanvasControl.CustomDevice = customDevice;
            this.IconCanvasControl.Draw += (s, arge) =>
            {
                if (this.IconRender == null) return;

                arge.DrawingSession.DrawImage(this.IconRender);
            };

            //LayerageCollection
            {
                this.Tapped += (s, e) =>
                {
                    LayerageCollection.ItemClick?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
                this.RightTapped += (s, e) =>
                {
                    LayerageCollection.RightTapped?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
                this.Holding += (s, e) =>
                {
                    LayerageCollection.RightTapped?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
                this.DoubleTapped += (s, e) =>
                {
                    LayerageCollection.RightTapped?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
                this.VisualButton.Tapped += (s, e) =>
                {
                    LayerageCollection.VisibilityChanged?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
            }

            //Mode
            {
                this.ExpanedButton.Tapped += (s, e) =>
                {
                    LayerageCollection.IsExpandChanged?.Invoke(layer);//Delegate   
                    e.Handled = true;
                };
                this.SelectedButton.Tapped += (s, e) =>
                {
                    LayerageCollection.IsSelectedChanged?.Invoke(layer);//Delegate   
                    e.Handled = true;
                };
            }

            //Manipulation
            {
                this.ManipulationStarted += (s, e) =>
                {
                    LayerageCollection.IsOverlay = true;
                    LayerageCollection.DragItemsStarted?.Invoke(layer, this.ManipulationMode);//Delegate     
                };
                this.ManipulationCompleted += (s, e) =>
                {
                    if (LayerageCollection.IsOverlay)
                    {
                        LayerageCollection.DragItemsCompleted?.Invoke();//Delegate

                        LayerageCollection.IsOverlay = false;
                        this.OverlayMode = OverlayMode.None;
                    }
                };
            }

            //Pointer
            {
                this.PointerMoved += (s, e) =>
                {
                    if (LayerageCollection.IsOverlay)
                    {
                        Point position = e.GetCurrentPoint(this).Position;
                        OverlayMode overlayMode = this.GetOverlay(position.Y);

                        this.OverlayMode = overlayMode;
                        LayerageCollection.DragItemsDelta?.Invoke(layer, overlayMode);//Delegate
                    }
                };
                this.PointerExited += (s, e) => this.OverlayMode = OverlayMode.None;
                this.PointerReleased += (s, e) => this.OverlayMode = OverlayMode.None;
            }
        }


    }
}