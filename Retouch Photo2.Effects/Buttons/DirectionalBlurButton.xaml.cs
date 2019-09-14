using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects.Buttons
{
    /// <summary>
    /// Button of <see cref = "DirectionalBlurEffect"/>.
    /// </summary>
    public sealed partial class DirectionalBlurButton : UserControl, IEffectButton
    {
        //@Content
        public FrameworkElement Self => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;

        //@Construct
        public DirectionalBlurButton()
        {
            this.InitializeComponent(); 
        }

        public void FollowEffectManager(EffectManager effectManager)
        {
            this.Button.ToggleSwitch.IsOn = effectManager.DirectionalBlur_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.DirectionalBlur_IsOn = this.Button.ToggleSwitch.IsOn;
        }
    }
}