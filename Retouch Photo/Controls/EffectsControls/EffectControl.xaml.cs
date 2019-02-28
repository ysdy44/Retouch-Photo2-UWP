using Retouch_Photo.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Controls.EffectsControls
{
    public sealed partial class EffectControl : UserControl
    {
        //Delegate
        public delegate void EffectHandler(Effect effect);
        public event EffectHandler EffectTapped = null;
        public event EffectHandler EffectToggle = null;
        

        #region DependencyProperty

        public Effect Effect
        {
            get { return (Effect)GetValue(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register(nameof(Effect), typeof(Effect), typeof(EffectControl), new PropertyMetadata(null));

        #endregion


        public EffectControl()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Effect.IsOn == false) return;

            this.EffectTapped?.Invoke(this.Effect);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            this.EffectToggle?.Invoke(this.Effect);
        }
    }
}
