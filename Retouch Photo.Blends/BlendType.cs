namespace Retouch_Photo.Blends
{
    public enum BlendType
    {
        /// <summary> 正常 </summary>
        Normal,

        /// <summary> 正片叠底 </summary>    
        Multiply,
        /// <summary> 屏幕 </summary>
        Screen,
        /// <summary> 溶解 </summary>
        Dissolve,

        /// <summary> 暗 </summary>
        Darken,
        /// <summary> 亮 </summary>
        Lighten,
        /// <summary> 更暗的颜色 </summary>
        DarkerColor,
        /// <summary> 更亮的颜色 </summary>              
        LighterColor,

        /// <summary> 颜色加深 </summary>                   
        ColorBurn,
        /// <summary> 颜色减淡 </summary>                    
        ColorDodge,
        /// <summary> 线性加深 </summary>                     
        LinearBurn,
        /// <summary> 线性减淡 </summary>                      
        LinearDodge,

        /// <summary> 覆盖 </summary>                           
        Overlay,
        /// <summary> 柔光 </summary>                            
        SoftLight,
        /// <summary> 硬光 </summary>                             
        HardLight,
        /// <summary> 艳光 </summary>                              
        VividLight,
        /// <summary> 线性光 </summary>                               
        LinearLight,
        /// <summary> 射灯 </summary>                                
        PinLight,

        /// <summary> 硬混 </summary>                                     
        HardMix,
        /// <summary> 差异 </summary>                                      
        Difference,
        /// <summary> 排除 </summary>                                       
        Exclusion,

        /// <summary> 色相 </summary>                                            
        Hue,
        /// <summary> 饱和度 </summary>                                             
        Saturation,
        /// <summary> 颜色 </summary>                                              
        Color,

        /// <summary> 光度 </summary>                                                   
        Luminosity,
        /// <summary> 减去 </summary>                                                    
        Subtract,
        /// <summary> 反色 </summary>                                                     
        Division,
    }
}
