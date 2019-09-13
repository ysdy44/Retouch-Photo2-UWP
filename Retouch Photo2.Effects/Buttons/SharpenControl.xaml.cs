using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects.Buttons
{
    /// <summary>
    /// Button of <see cref = "SharpenEffect"/>.
    /// </summary>
    public sealed partial class SharpenButton : UserControl, IEffectButton
    {
        //@Content
        public FrameworkElement Self=> this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;

        //@Construct
        public SharpenButton()
        {
            this.InitializeComponent();
        }

        public void FollowEffectManager(EffectManager effectManager)
        {
            this.Button.ToggleSwitch.IsOn = effectManager.Sharpen_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Sharpen_IsOn = this.Button.ToggleSwitch.IsOn;
        }
    }
}