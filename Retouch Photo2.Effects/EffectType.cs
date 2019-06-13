namespace Retouch_Photo2.Effects
{  
    /// <summary> An <see cref = "Effect"/> corresponds to a <see cref = "EffectType" /> </summary>
    public enum EffectType
    {
        /// <summary> GaussianBlur (高斯模糊) </summary>
        GaussianBlur,
        /// <summary> DirectionalBlur (定向模糊) </summary>
        DirectionalBlur,
        /// <summary> Sharpen (锐化) </summary>
        Sharpen,
        /// <summary> OuterShadow (外部投影) </summary>
        OuterShadow,

        /// <summary> Outline (轮廓) </summary>
        Outline,

        /// <summary> Emboss (浮雕) </summary>
        Emboss,
        /// <summary> Straighten (拉直) </summary>
        Straighten
    }       
}