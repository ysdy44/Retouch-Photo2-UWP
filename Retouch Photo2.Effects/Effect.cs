namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Effect Classes.
    /// </summary>
    public abstract class Effect
    {

        //@Abstract
        /// <summary>
        /// Gets the <see cref = "EffectManager" />'s isOn.
        /// </summary>
        /// <param name="effectManager"> EffectManager </param>
        /// <returns> isOn </returns>
        public abstract bool GetIsOn(EffectManager effectManager);
        /// <summary>
        /// Sets the <see cref = "EffectManager" />'s isOn.
        /// </summary>
        /// <param name="effectManager"> EffectManager </param>
        /// <param name="isOn"> isOn </param>
        public abstract void SetIsOn(EffectManager effectManager, bool isOn);

        /// <summary>
        /// Reset the <see cref = "EffectManager" />'s data.
        /// </summary>
        /// <param name="effectManager"></param>
        public abstract void Reset(EffectManager effectManager);
        /// <summary>
        /// Sets <see cref = "Effect.Page" />'s value by <see cref = "EffectManager" />.
        /// </summary>
        /// <param name="effectManager"></param>
        public abstract void SetPageValueByEffectManager(EffectManager effectManager);


        /// <summary> <see cref = "Effect" />'s name. </summary>
        public EffectType Type;
        /// <summary> <see cref = "Effect" />'s button. </summary>
        public Retouch_Photo2.Effects.Button Button;      
        /// <summary> <see cref = "Effect" />'s page. </summary>
        public Windows.UI.Xaml.Controls.Page Page;
    }
}