// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      
namespace Retouch_Photo2.Effects
{
    /// <summary> 
    /// Type of <see cref="Effect"/>.
    /// </summary>
public enum EffectType
    {
        /// <summary> Nornal </summary>
        None,

        /// <summary> GaussianBlur </summary>
        GaussianBlur,

        /// <summary> DirectionalBlur </summary>
        DirectionalBlur,

        /// <summary> Sharpen </summary>
        Sharpen,

        /// <summary> OuterShadow </summary>
        OuterShadow,

        /// <summary> Edge </summary>
        Edge,

        /// <summary> Morphology </summary>
        Morphology,

        /// <summary> Emboss </summary>
        Emboss,

        /// <summary> Straighten </summary>
        Straighten
    }       
}