using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Blends;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer control.
    /// </summary>
    public partial class LayerControl : UserControl
    {

        //@VisualState
        ClickMode _vsClickMode;
        BackgroundMode _vsBackgroundMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsBackgroundMode)
                {
                    case BackgroundMode.UnSelected:
                        switch (this._vsClickMode)
                        {
                            case ClickMode.Release: return this.Normal;
                            case ClickMode.Hover: return this.PointerOver;
                            case ClickMode.Press: return this.Pressed;
                        }
                        break;
                    case BackgroundMode.Selected:
                        return this.Selected;
                    case BackgroundMode.ParentsSelected:
                        return this.ParentsSelected;
                    case BackgroundMode.ChildSelected:
                        return this.ChildSelected;
                    default:
                        return this.Normal;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> VisualState's ClickMode. </summary>
        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }



        //Control
        /// <summary> Gets or sets the height. </summary>
        public int ControlHeight
        {
            get => this.controlHeight;
            set
            {
                this.Height = value;
                this.IconBorder.Width = value;
                this.IconBorder.Height = value;

                //Overlay
                double heightOver7 = value / 7;
                this.OverlayShowTopBorder.Height = heightOver7 + heightOver7;
                this.OverlayShowCenterBorder.Height = heightOver7 + heightOver7 + heightOver7;
                this.OverlayShowBottomBorder.Height = heightOver7 + heightOver7;

                this.controlHeight = value;
            }
        }
        private int controlHeight = 40;

        /// <summary> Gets or sets the overlay show status. </summary>
        public OverlayMode OverlayMode
        {
            get => this.overlayMode;
            set
            {
                if (this.overlayMode == value) return;

                this.OverlayShowTopBorder.Visibility = (value == OverlayMode.Top) ? Visibility.Visible : Visibility.Collapsed;
                this.OverlayShowCenterBorder.Visibility = (value == OverlayMode.Center) ? Visibility.Visible : Visibility.Collapsed;
                this.OverlayShowBottomBorder.Visibility = (value == OverlayMode.Bottom) ? Visibility.Visible : Visibility.Collapsed;

                this.overlayMode = value;
            }
        }
        private OverlayMode overlayMode;
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

        

        //Property
        /// <summary> Gets or sets the type. </summary>
        public string Type { set => this.TypeRun.Text = value; }
        /// <summary> Gets or sets the name. </summary>
        public string Name2 { set => this.NameRun.Text = value; }

        /// <summary> Gets or sets the visibility. </summary>
        public Visibility Visibility2 { set => this.VisualFontIcon.Opacity = value == Visibility.Visible ? 1.0 : 0.5; }
        /// <summary> Gets or sets the tag type. </summary>
        public TagType TagType { set => this.TagColor.Color = value.ToColor(); }

        /// <summary> Gets or sets the IsExpand. </summary>
        public bool IsExpand { set => this.ExpanedFontIcon.Glyph = value ? "\xEDDC" : "\xEDD9"; }
        /// <summary> Gets or sets the IsSelected. </summary>
        public bool IsSelected
        {
            set
            {
                if (value == true)
                {
                    this.ManipulationMode = ManipulationModes.TranslateY;
                    this.SelectedFontIcon.Glyph = "\xEC61";
                }
                else
                {
                    this.ManipulationMode = ManipulationModes.System;
                    this.SelectedFontIcon.Glyph = "\xECCA";
                }
            }
        }

               

        //LayerageCollection
        /// <summary> Gets or sets the depth. </summary>
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
        private int depth = 0;
        /// <summary> Gets or sets the children count. </summary>
        public int ChildrenCount { set => this.ExpanedButton.Visibility = value == 0 ? Visibility.Collapsed : Visibility.Visible; }
        /// <summary> Gets or sets the background mode. </summary>
        public BackgroundMode BackgroundMode
        {
            set
            {
                this._vsBackgroundMode = value;
                this.VisualState = this.VisualState;//State
            }
        }

    }
}