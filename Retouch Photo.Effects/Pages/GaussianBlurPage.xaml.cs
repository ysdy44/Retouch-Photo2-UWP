using Retouch_Photo.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class GaussianBlurPage : Page
    {

        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            GaussianBlurPage con = (GaussianBlurPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                GaussianBlurEffectItem item = effectManager.GaussianBlurEffectItem;

                con.BlurAmountSlider.Value = item.BlurAmount;
            }
        }));

        #endregion


        public GaussianBlurPage()
        {
            this.InitializeComponent();
        }
        
        private void BlurAmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.GaussianBlurEffectItem.BlurAmount = (float)e.NewValue;
            Effect.Invalidate();
        }
    }
}
