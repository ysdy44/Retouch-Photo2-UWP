using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects.Buttons
{
    /// <summary>
    /// Button of <see cref = "GaussianBlurEffect"/>.
    /// </summary>
    public sealed partial class GaussianBlurButton : UserControl, IEffectButton
    {
        //@Content
        public FrameworkElement Self=> this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;

        //@Construct
        public GaussianBlurButton()
        {
            this.InitializeComponent();
        }

        public void FollowEffectManager(EffectManager effectManager)
        {
            this.Button.ToggleSwitch.IsOn = effectManager.GaussianBlur_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.GaussianBlur_IsOn = this.Button.ToggleSwitch.IsOn;
        }
    }
}