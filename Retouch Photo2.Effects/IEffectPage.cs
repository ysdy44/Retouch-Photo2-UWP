// Core:              ★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★★★
using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Page of <see cref="Effect"/>.
    /// </summary>
    public interface IEffectPage
    {
        /// <summary> Gets the type. </summary>
        EffectType Type { get; }
        /// <summary> Gets the page. </summary>
        FrameworkElement Page { get; }
        /// <summary> Gets the button. </summary>
        EffectButton Button { get; }
        
        /// <summary>
        /// Reset the <see cref="Effect"/> and <see cref="IEffectPage"/>'s data.
        /// </summary>
        void Reset();
        /// <summary>
        /// <see cref="EffectButton"/>'s value follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void FollowButton(Effect effect);
        /// <summary>
        /// <see cref="IEffectPage"/>'s value follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void FollowPage(Effect effect);
    }
}