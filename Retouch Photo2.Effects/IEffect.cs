using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Represents a special effect page that adds effects to layers
    /// </summary>
    public interface IEffectPage
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
        /// Reset the <see cref="IEffectPage"/>'s value.
        /// </summary>
        void Reset();
        /// <summary>
        /// Reset the <see cref="Effect"/>'s data.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void ResetEffect(Effect effect);
        /// <summary>
        /// <see cref="IEffectPage"/>'s value follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void FollowEffect(Effect effect);
        /// <summary>
        /// Overwriting the <see cref="Effect"/> according to ToggleSwitch's IsOn
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void OverwritingEffect(Effect effect);
    }    
}