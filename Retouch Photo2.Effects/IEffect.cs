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
        IEffectPage Page { get; }
        /// <summary> Gets IEffect's button. </summary>
        IEffectButton Button { get; }
    }
}