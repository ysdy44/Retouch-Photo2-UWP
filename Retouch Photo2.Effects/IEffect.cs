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
        /// <summary> Gets IEffect's button. </summary>
        Retouch_Photo2.Effects.Button Button { get; }
        /// <summary> Gets IEffect's page. </summary>
        Page Page { get; }


        /// <summary>
        /// Gets the <see cref = "EffectManager" />'s isOn.
        /// </summary>
        /// <param name="effectManager"> EffectManager </param>
        /// <returns> isOn </returns>
        bool GetIsOn(EffectManager effectManager);
        /// <summary>
        /// Sets the <see cref = "EffectManager" />'s isOn.
        /// </summary>
        /// <param name="effectManager"> EffectManager </param>
        /// <param name="isOn"> isOn </param>
        void SetIsOn(EffectManager effectManager, bool isOn);

        /// <summary>
        /// Reset the <see cref = "EffectManager" />'s data.
        /// </summary>
        /// <param name="effectManager"></param>
        void Reset(EffectManager effectManager);
        /// <summary>
        /// Sets <see cref = "IEffect.Page" />'s value by <see cref = "EffectManager" />.
        /// </summary>
        /// <param name="effectManager"></param>
        void SetPageValueByEffectManager(EffectManager effectManager);
    }
}