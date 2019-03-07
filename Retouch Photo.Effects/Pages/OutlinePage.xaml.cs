using Retouch_Photo.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class OutlinePage : Page
    {

        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            OutlinePage con = (OutlinePage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                OutlineEffectItem item = effectManager.OutlineEffectItem;

                con.SizeSlider.Value = item.Size;
            }
        }));

        #endregion


        public OutlinePage()
        {
            this.InitializeComponent();
        }

        private void SizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OutlineEffectItem.Size = (int)e.NewValue;
            Effect.Invalidate();
        }
    }
}
