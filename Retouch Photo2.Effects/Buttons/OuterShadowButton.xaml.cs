using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects.Buttons
{
    /// <summary>
    /// Button of <see cref = "OuterShadowEffect"/>.
    /// </summary>
    public sealed partial class OuterShadowButton : UserControl, IEffectButton
    {
        //@Content
        public FrameworkElement Self=> this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;

        //@Construct
        public OuterShadowButton()
        {
            this.InitializeComponent();
        }


        public void FollowEffectManager(EffectManager effectManager)
        {
            this.Button.ToggleSwitch.IsOn = effectManager.OuterShadow_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.OuterShadow_IsOn = this.Button.ToggleSwitch.IsOn;
        }
    }
}