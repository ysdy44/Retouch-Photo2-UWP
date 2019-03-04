using Retouch_Photo.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class StraightenPage : Page
    {

        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            StraightenPage con = (StraightenPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                StraightenEffectItem item = effectManager.StraightenEffectItem;

                con.AnglePicker.Radians = item.Angle * 4.0f;
            }
        }));

        #endregion


        public StraightenPage()
        {
            this.InitializeComponent();
        }

        private void AnglePicker_AngleChange(float radians)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.StraightenEffectItem.Angle = radians / 4.0f;
            Effect.Invalidate();
        }
    }
}
