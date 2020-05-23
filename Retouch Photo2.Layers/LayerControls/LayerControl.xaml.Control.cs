using Retouch_Photo2.Blends;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    public partial class LayerControl : UserControl
    {

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
            switch (value)
            {
                case BackgroundMode.UnSelected: this.RootGrid.Background = this.UnAccentColor; break;
                case BackgroundMode.Selected: this.RootGrid.Background = this.AccentColor; break;
                case BackgroundMode.ParentsSelected: this.RootGrid.Background = this.ThreeStateColor; break;
                case BackgroundMode.ChildSelected: this.RootGrid.Background = this.FourStateColor; break;
            }

            if (value == BackgroundMode.Selected)
            {
                this.IconContentControl.Foreground = this.CheckColor;
                this.TextBlock.Foreground =
                    this.ExpanedFontIcon.Foreground =
                    this.SelectedFontIcon.Foreground =
                    this.VisualFontIcon.Foreground =
                    this.CheckColor;
            }
            else
            {
                this.IconContentControl.Foreground = this.HighlightColor;
                this.TextBlock.Foreground =
                this.ExpanedFontIcon.Foreground =
                this.SelectedFontIcon.Foreground =
                this.VisualFontIcon.Foreground =
                this.UnCheckColor;
            }
        }


        public void SetIsExpand(bool value)
        {
            this.ExpanedFontIcon.Glyph = value ? "\xE011" : "\xE014";
        }


        public void SetChildrenZero(bool value)
        {
            this.ExpanedButton.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
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
            this.TagColor.Color = TagTypeHelper.TagConverter(value);
        }

    }
}