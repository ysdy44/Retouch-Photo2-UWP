using Retouch_Photo.Effects.Items;
using Retouch_Photo.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class EmbossPage : Page
    {

        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            EmbossPage con = (EmbossPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                EmbossEffectItem item = effectManager.EmbossEffectItem;

                con.AmountSlider.Value = item.Amount;
                con.AnglePicker.Radians = item.Angle;
            }
        }));

        #endregion


        public EmbossPage()
        {
            this.InitializeComponent();
        }
        
        private void AmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.EmbossEffectItem.Amount = (float)e.NewValue;
            Effect.Invalidate();
        }

        private void AnglePicker_AngleChange(float radians)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.EmbossEffectItem.Angle = radians;
            Effect.Invalidate();
        } 

    }
}
