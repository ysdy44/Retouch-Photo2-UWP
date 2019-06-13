using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Elements;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// <see cref = "OuterShadowEffect" /> 's Page.
    /// </summary>
    public sealed partial class OuterShadowPage : Page
    {
        /// <summary> <see cref = "OuterShadowPage" />'s RadiusSlider. </summary>
        public Slider RadiusSlider => this._RadiusSlider;
        /// <summary> <see cref = "OuterShadowPage" />'s OpacitySlider. </summary>
        public Slider OpacitySlider => this._OpacitySlider;
        /// <summary> <see cref = "OuterShadowPage" />'s OffsetSlider. </summary>
        public Slider OffsetSlider => this._OffsetSlider;
    
        /// <summary> <see cref = "OuterShadowPage" />'s AnglePicker. </summary>
        public RadiansPicker AnglePicker => this._AnglePicker;

        /// <summary> <see cref = "OuterShadowPage" />'s SolidColorBrush. </summary>
        public SolidColorBrush SolidColorBrush => this._SolidColorBrush;
        
        //@Construct
        public OuterShadowPage()
        {
            this.InitializeComponent();

            this._RadiusSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Radius = (float)e.NewValue;
                });
            };
            this._OpacitySlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Opacity = (float)(e.NewValue / 100.0f);
                });
            };
            this._OffsetSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Offset = (float)e.NewValue;
                });
            };
            this._AnglePicker.RadiansChange += (radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Angle = radians;
                });
            };

            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this._SolidColorBrush.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this._SolidColorBrush.Color = value;

                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Color = value;
                });
            };
        }
    }
}