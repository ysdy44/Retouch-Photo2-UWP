namespace Retouch_Photo.Effects
{  
    /// <summary> An <see cref = "Effect"/> corresponds to a <see cref = "EffectType" /> </summary>
    public enum EffectType
    {
        /// <summary> 高斯模糊 </summary>
        GaussianBlur,
        /// <summary> 定向模糊 </summary>
        DirectionalBlur,

        /// <summary> 外部投影 </summary>
        OuterShadow,

        /// <summary> 轮廓 </summary>
        Outline,

        /// <summary> 浮雕 </summary>
        Emboss,
        /// <summary> 拉直 </summary>
        Straighten
    }       
}
