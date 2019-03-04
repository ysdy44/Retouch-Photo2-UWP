using Retouch_Photo.Effects.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class OuterShadowPage : Page
    {
        
        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            OuterShadowPage con = (OuterShadowPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                OuterShadowEffectItem item = effectManager.OuterShadowEffectItem;

                con.RadiusSlider.Value = item.Radius;
                con.OpacitySlider.Value = item.Opacity * 100.0;
                con.OffsetSlider.Value = item.Offset;
                con.AnglePicker.Radians = item.Angle;
                con.SolidColorBrush.Color = item.Color;
            }
        }));

        #endregion


        public OuterShadowPage()
        {
            this.InitializeComponent();
        }

        private void RadiusSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Radius = (float)e.NewValue;
            Effect.Invalidate();
        }

        private void OpacitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Opacity = (float)(e.NewValue / 100.0);
            Effect.Invalidate();
        }


        private void OffsetSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Offset = (float)e.NewValue;
            Effect.Invalidate();
        }

        private void AnglePicker_AngleChange(float radians)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.OuterShadowEffectItem.Angle = radians;
            Effect.Invalidate();
        }
         
        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ColorFlyout.ShowAt(this.ColorButton);
            this.ColorPicker.Color = this.EffectManager.OuterShadowEffectItem.Color;
        }
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.SolidColorBrush.Color = value;

            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Color = value;
            Effect.Invalidate();
        }

    }
}
