using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Represents a effect button.
    /// </summary>
    public interface IEffectButton
    {
        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets button's ToggleSwitch. </summary>
        ToggleSwitch ToggleSwitch { get; }

        /// <summary>
        /// ToggleSwitch's IsOn follows the effect-manager.
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void FollowEffectManager(EffectManager effectManager);
        /// <summary>
        /// Overwriting the effect-manager according to ToggleSwitch's IsOn
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void OverwritingEffectManager(EffectManager effectManager);
    }
}