using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Layers
{
    public partial class LayerControl : UserControl, ILayerControl
    {


        public void SetTagType(TagType value)
        {
            this.TagColor.Color = (value == TagType.None) ?
            Colors.Transparent :
            this.TagColor.Color = TagControl.TagConverter(value);
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