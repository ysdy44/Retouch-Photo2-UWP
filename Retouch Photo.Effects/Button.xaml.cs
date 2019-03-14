using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Effects
{
    public sealed partial class Button : UserControl
    {
        //Delegate
        public event EffectHandler EffectTapped = null;
        public event EffectHandler EffectToggle = null;


        #region DependencyProperty

        public Effect Effect
        {
            get { return (Effect)GetValue(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register(nameof(Effect), typeof(Effect), typeof(Button), new PropertyMetadata(null));

        #endregion


        public Button()
        {
            this.InitializeComponent();

            this.Loaded += (s, e) =>
            {
                if (this.Effect == null) return;

                //Icon
                this.RootButton.Content = this.Effect.Icon;

                //Effect
                this.Effect.Button = this.RootButton;
                this.Effect.ToggleSwitch = this.ToggleSwitch;
            };

            this.RootButton.Tapped += (s, e) =>
            {
                if (this.Effect.IsOn == false) return;

                this.EffectTapped?.Invoke(this.Effect);
            };
            this.ToggleSwitch.Toggled += (s, e) => this.EffectToggle?.Invoke(this.Effect);
        }
    }
}
