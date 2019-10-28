using Retouch_Photo2.Blends;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Layers
{
    public partial class LayerControl : UserControl, ILayerControl
    {

        //@Content
        public FrameworkElement Self => this;
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        public object Icon { get => this.IconContentControl.Content; set => this.IconContentControl.Content = value; }


        private int controlHeight = 40;
        public int ControlHeight
        {
            get => this.controlHeight;
            set
            {
                this.Height = value;
                
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

        
        //@Construct
        public LayerControl(ILayer layer)
        {
            this.InitializeComponent();

            this.ControlHeight = LayerCollection.ControlsHeight;

            //LayerCollection
            {
                this.Loaded += (s, e) =>
                {
                    if (layer.ExpandMode == ExpandMode.None)
                        layer.ExpandMode = ExpandMode.NoChildren;
                    if (layer.SelectMode == SelectMode.None)
                        layer.SelectMode = SelectMode.UnSelected;
                };

                this.Tapped += (s, e) =>
                {
                    LayerCollection.ItemClick?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
                this.RightTapped += (s, e) =>
                {
                    LayerCollection.RightTapped?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
                this.VisualButton.Tapped += (s, e) =>
                {
                    LayerCollection.VisualChanged?.Invoke(layer);//Delegate
                    e.Handled = true;
                };
            }

            //Mode
            {
                this.DoubleTapped += (s, e) =>
                {
                    layer.Expaned();
                    e.Handled = true;
                };
                this.ExpanedButton.Tapped += (s, e) =>
                {
                    layer.Expaned();
                    e.Handled = true;
                };
                this.SelectedButton.Tapped += (s, e) =>
                {
                    layer.Selected(); 
                    LayerCollection.SelectChanged?.Invoke();//Delegate   
                    e.Handled = true;
                };
            }

            //Manipulation
            {
                this.ManipulationStarted += (s, e) =>
                {
                    LayerCollection.IsOverlay = true;
                    LayerCollection.DragItemsStarted?.Invoke(layer, layer.SelectMode);//Delegate     
                };
                this.ManipulationCompleted += (s, e) =>
                {
                    if (LayerCollection.IsOverlay)
                    {
                        LayerCollection.DragItemsCompleted?.Invoke();//Delegate

                        LayerCollection.IsOverlay = false;
                        layer.OverlayMode = OverlayMode.None;
                    }
                };
            }

            //Pointer
            {
                this.PointerMoved += (s, e) =>
                {
                    if (LayerCollection.IsOverlay)
                    {
                        Point position = e.GetCurrentPoint(this).Position;
                        OverlayMode overlayMode = this.GetOverlay(position.Y);

                        layer.OverlayMode = overlayMode;
                        LayerCollection.DragItemsDelta?.Invoke(layer, overlayMode);//Delegate
                    }
                };
                this.PointerExited += (s, e) => layer.OverlayMode = OverlayMode.None;
                this.PointerReleased += (s, e) => layer.OverlayMode = OverlayMode.None;
            }
        }


        public void SetVisibility(Visibility value)
        {
            switch (value)
            {
                case Visibility.Visible:
                    this.VisualFontIcon.Opacity = 1.0;
                    break;
                case Visibility.Collapsed:
                    this.VisualFontIcon.Opacity = 0.5;
                    break;
            }
        }
        public void SetTagType(TagType value)
        {
            this.TagColor.Color = (value == TagType.None) ?
            Colors.Transparent :
            this.TagColor.Color = TagTypeHelper.TagConverter(value);
        }

    }
}