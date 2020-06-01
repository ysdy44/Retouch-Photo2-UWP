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

        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
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


        public void SetIsSelected(bool value)
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
        public void SetBackground(BackgroundMode value)
        {
            this._vsBackgroundMode = value;
            this.VisualState = this.VisualState;//State
        }


        public void SetIsExpand(bool value) => this.ExpanedFontIcon.Glyph = value ? "\xEDDC" : "\xEDD9"; 
        public void SetChildrenZero(bool value) => this.ExpanedButton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
        public void SetVisibility(Visibility value) => this.VisualFontIcon.Opacity = value == Visibility.Visible ? 1.0 : 0.5;
        public void SetTagType(TagType value) => this.TagColor.Color = TagTypeHelper.TagConverter(value);


        public void SetControlHeight(double value)
        {
            this.Height = value;
            this.IconBorder.Width = value;
            this.IconBorder.Height = value;

            //Overlay
            double heightOver7 = value / 7;
            this.OverlayShowTopBorder.Height = heightOver7 + heightOver7;
            this.OverlayShowCenterBorder.Height = heightOver7 + heightOver7 + heightOver7;
            this.OverlayShowBottomBorder.Height = heightOver7 + heightOver7;
        }
        public void SetDepth (int value)
        {
            double pixels = value * 20.0d;
            GridLength gridLength = new GridLength(pixels, GridUnitType.Pixel);
            this.DepthColumn.Width = gridLength;
        }



    }
}