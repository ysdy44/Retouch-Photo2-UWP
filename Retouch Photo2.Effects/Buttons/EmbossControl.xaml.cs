using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects.Buttons
{
    /// <summary>
    /// Button of <see cref = "EmbossEffect"/>.
    /// </summary>
    public sealed partial class EmbossButton : UserControl, IEffectButton
    {
        //@Content
        public FrameworkElement Self=> this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;

        //@Construct
        public EmbossButton()
        {
            this.InitializeComponent();
        }
        
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.Button.ToggleSwitch.IsOn = effectManager.Emboss_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Emboss_IsOn = this.Button.ToggleSwitch.IsOn;
        }
    }
}