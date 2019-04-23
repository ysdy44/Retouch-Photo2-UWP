namespace Retouch_Photo2.Adjustments
{
    /// <summary> An <see cref = "Adjustment"/> corresponds to a <see cref = "AdjustmentType" /> </summary>
    public enum AdjustmentType
    {
        /// <summary> 灰度 </summary>
        Gray,
        /// <summary> 反色 </summary>
        Invert,

        /// <summary> 曝光 </summary>
        Exposure,
        /// <summary> 明度 </summary>
        Brightness,
        /// <summary> 饱和度 </summary>
        Saturation,
        /// <summary> 色相旋转 </summary>
        HueRotation,
        /// <summary> 对比度 </summary>
        Contrast,
        /// <summary> 冷暖 </summary>
        Temperature,

        /// <summary> 高亮/阴影 </summary>
        HighlightsAndShadows,
        /// <summary> 伽马转移 </summary>
        GammaTransfer,
        /// <summary> 装饰图案 </summary>
        Vignette
    }
}
