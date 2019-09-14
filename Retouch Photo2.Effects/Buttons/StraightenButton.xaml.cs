using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects.Buttons
{
    /// <summary>
    /// Button of <see cref = "StraightenEffect"/>.
    /// </summary>
    public sealed partial class StraightenButton : UserControl, IEffectButton
    {
        //@Content
        public FrameworkElement Self=> this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;

        //@Construct
        public StraightenButton()
        {
            this.InitializeComponent();
        }

        public void FollowEffectManager(EffectManager effectManager)
        {
            this.Button.ToggleSwitch.IsOn = effectManager.Straighten_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Straighten_IsOn = this.Button.ToggleSwitch.IsOn;
        }
    }
}