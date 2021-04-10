// Core:              ★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Page of <see cref="Effect"/>.
    /// </summary>
    public interface IEffectPage
    {
        /// <summary> Gets the type. </summary>
        EffectType Type { get; }

        /// <summary> Gets the self. </summary>
        FrameworkElement Self { get; }

        /// <summary>
        /// Reset the <see cref="Effect"/> and <see cref="IEffectPage"/>'s data.
        /// </summary>
        void Reset();

        void Switch(bool isOn);
        /// <summary>
        /// IsChecked follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        bool FollowButton(Effect effect);
        /// <summary>
        /// <see cref="IEffectPage"/>'s value follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void FollowPage(Effect effect);
    }
}