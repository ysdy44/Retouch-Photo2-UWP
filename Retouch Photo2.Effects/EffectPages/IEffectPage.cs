using Windows.UI.Xaml;

namespace Retouch_Photo2.Effects
{
    public interface IEffectPage
    {
        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }

        /// <summary>
        /// Reset the page's value.
        /// </summary>
        void Reset();
        /// <summary>
        /// Reset the effect-manager's data.
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void ResetEffectManager(EffectManager effectManager);
        /// <summary>
        /// Page's value follows the effect-manager.
        /// </summary>
        /// <param name="effectManager"> The effect-manager. </param>
        void FollowEffectManager(EffectManager effectManager);
    }
}