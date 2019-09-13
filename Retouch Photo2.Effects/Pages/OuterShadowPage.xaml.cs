using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "OuterShadowEffect"/>.
    /// </summary>
    public sealed partial class OuterShadowPage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public OuterShadowPage()
        {
            this.InitializeComponent();

            this.RadiusSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Radius = (float)e.NewValue;
                });
            };
            this.OpacitySlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Opacity = (float)(e.NewValue / 100.0f);
                });
            };
            this.OffsetSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Offset = (float)e.NewValue;
                });
            };
            this.AnglePicker.RadiansChange += (s, radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Angle = radians;
                });
            };

            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SolidColorBrush.Color = value;

                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.OuterShadow_Color = value;
                });
            };
        }

        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.OpacitySlider.Value = 50f;
            this.SolidColorBrush.Color = Windows.UI.Colors.Black;

            this.OffsetSlider.Value = 0;
            this.AnglePicker.Radians = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.OuterShadow_Radius = 0;
            effectManager.OuterShadow_Opacity = 0.5f;
            effectManager.OuterShadow_Color = Windows.UI.Colors.Black;

            effectManager.OuterShadow_Offset = 0;
            effectManager.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.RadiusSlider.Value = effectManager.OuterShadow_Radius;
            this.OpacitySlider.Value = effectManager.OuterShadow_Opacity * 100.0f;
            this.SolidColorBrush.Color = effectManager.OuterShadow_Color;

            this.OffsetSlider.Value = effectManager.OuterShadow_Offset;
            this.AnglePicker.Radians = effectManager.OuterShadow_Angle;
        }
    }
}