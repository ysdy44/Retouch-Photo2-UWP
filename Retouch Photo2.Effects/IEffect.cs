using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Represents a special effect that adds effects to layers
    /// </summary>
    public interface IEffect
    {
        /// <summary> Gets IEffect's name. </summary>
        EffectType Type { get; }

        /// <summary> Gets IEffect's page. </summary>
        FrameworkElement Page { get; }
        /// <summary> Gets IEffect's button. </summary>
        Control Button { get; }
        /// <summary> Gets button's ToggleSwitch. </summary>
        ToggleSwitch ToggleSwitch { get; }

        /// <summary>
        /// Reset the <see cref="IEffect"/>'s value.
        /// </summary>
        void Reset();
        /// <summary>
        /// Reset the <see cref="EffectManager"/>'s data.
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void ResetEffectManager(EffectManager effectManager);
        /// <summary>
        /// <see cref="IEffect"/>'s value follows the <see cref="EffectManager"/>.
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void FollowEffectManager(EffectManager effectManager);
        /// <summary>
        /// Overwriting the <see cref="EffectManager"/> according to ToggleSwitch's IsOn
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void OverwritingEffectManager(EffectManager effectManager);
    }    
}