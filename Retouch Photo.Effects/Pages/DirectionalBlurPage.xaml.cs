using Retouch_Photo.Effects.Items;
using Retouch_Photo.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class DirectionalBlurPage : Page
    {
        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            DirectionalBlurPage con = (DirectionalBlurPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                DirectionalBlurEffectItem item = effectManager.DirectionalBlurEffectItem;

                con.BlurAmountSlider.Value = item.BlurAmount;
                con.AnglePicker.Radians = item.Angle;
            }
        }));

        #endregion


        public DirectionalBlurPage()
        {
            this.InitializeComponent();
        }
        
        private void BlurAmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.DirectionalBlurEffectItem.BlurAmount = (float)e.NewValue;
            Effect.Invalidate();
        }

        private void AnglePicker_AngleChange(float radians)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.DirectionalBlurEffectItem.Angle = radians;
            Effect.Invalidate();
        }
    }
}
