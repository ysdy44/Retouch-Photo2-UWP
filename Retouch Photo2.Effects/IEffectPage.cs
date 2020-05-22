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
        EffectButton Button { get; }
        
        /// <summary>
        /// Reset the <see cref="Effect"/> and <see cref="IEffectPage"/>'s data.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        void Reset();
        /// <summary>
        /// <see cref="IEffectPage"/>'s value follows the <see cref="Effect"/>.
        /// </summary>
        /// <param name="effect"> The effect. </param>
        /// <param name="isOnlyButton"> Only button follow. </param>
        void FollowEffect(Effect effect, bool isOnlyButton);
    }
}