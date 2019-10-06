using Windows.Foundation;
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

                //Depth
                {
                    double pixels = this.depth * value / 2.0d;
                    GridLength gridLength = new GridLength(pixels, GridUnitType.Pixel);
                    this.DepthColumn.Width = gridLength;
                }

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
                double pixels = value * this.ControlHeight / 2.0d;
                GridLength gridLength = new GridLength(pixels, GridUnitType.Pixel);
                this.DepthColumn.Width = gridLength;

                this.depth = value;
            }
        }


        public Button ExpanedButton => this._ExpanedButton;
        public Button SelectedButton => this._SelectedButton;


        //@Construct
        public LayerControl(ILayer layer)
        {
            this.InitializeComponent();

            this.ControlHeight = LayerCollection.ControlsHeight;

            //LayerCollection
            {
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
                this.Loaded += (s, e) =>
                {
                    if (layer.ExpandMode == ExpandMode.None)
                        layer.ExpandMode = ExpandMode.NoChildren;
                    if (layer.SelectMode == SelectMode.None)
                        layer.SelectMode = SelectMode.UnSelected;
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


        public void SetExpandMode(ExpandMode value)
        {
            this.ExpanedButton.Visibility = (value == ExpandMode.NoChildren) ? Visibility.Collapsed : Visibility.Visible;
            this.ExpanedFontIcon.Glyph = (value == ExpandMode.Expand) ? "\xE011" : "\xE014";
        }
        public void SetSelectMode(SelectMode value)
        {
            this.RootGrid.Background = this.GetBackground(value);

            if (value.ToBool())
            {
                this.ManipulationMode = ManipulationModes.TranslateY;
                this.IconContentControl.Foreground =
                    this.TextBlock.Foreground = 
                    this.ExpanedFontIcon.Foreground =
                    this.SelectedFontIcon.Foreground = this.CheckColor;
                this.SelectedFontIcon.Glyph = "\xEC61";
            }
            else
            {
                this.ManipulationMode = ManipulationModes.System;
                this.IconContentControl.Foreground =
                    this.TextBlock.Foreground =
                    this.ExpanedFontIcon.Foreground =
                    this.SelectedFontIcon.Foreground = this.UnCheckColor;
                this.SelectedFontIcon.Glyph = "\xECCA";
            }
        }
        public void SetOverlayMode(OverlayMode value)
        {
            this.OverlayShowTopBorder.Visibility = (value == OverlayMode.Top) ? Visibility.Visible : Visibility.Collapsed;
            this.OverlayShowCenterBorder.Visibility = (value == OverlayMode.Center) ? Visibility.Visible : Visibility.Collapsed;
            this.OverlayShowBottomBorder.Visibility = (value == OverlayMode.Bottom) ? Visibility.Visible : Visibility.Collapsed;
        }


        private OverlayMode GetOverlay(double y)
        {
            double height = this.ControlHeight;

            if (y > 0 && y < height)
            {
                double heightOver7To2 = height / 7.0d * 2.0d;
                double heightOver7Cut2 = height - heightOver7To2;

                if (y < heightOver7To2) return OverlayMode.Top;
                else if (y > heightOver7Cut2) return OverlayMode.Bottom;
                else
                    return OverlayMode.Center;
            }

            return OverlayMode.None;
        }

        private SolidColorBrush GetBackground(SelectMode newMode)
        {
            switch (newMode)
            {
                case SelectMode.UnSelected: return this.UnAccentColor;
                case SelectMode.Selected: return this.AccentColor;
                case SelectMode.ParentsSelected: return this.ThreeStateColor;
                case SelectMode.ChildSelected: return this.FourStateColor;
            }
            return this.UnAccentColor;
        }

    }
}