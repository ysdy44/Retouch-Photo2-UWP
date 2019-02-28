using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Blends;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models
{
    public abstract class Blend
    {
        public BlendType Type;
        public FrameworkElement Icon => this.GetIcon();
        protected abstract FrameworkElement GetIcon();

        protected abstract ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground);
        public static ICanvasImage Render(ICanvasImage background, ICanvasImage foreground, BlendType type) => Blend.BlendList.FirstOrDefault(e => e.Type == type).GetRender(background, foreground);
        
        //@static
        public static List<Blend> BlendList = new List<Blend>
        {
             new NormalBlend(),
            
             new MultiplyBlend(),
             new ScreenBlend(),            
             new DissolveBlend(),
            
             new DarkenBlend(),            
             new LightenBlend(),          
             new DarkerColorBlend(),           
             new LighterColorBlend(),
          
             new ColorBurnBlend(),
             new ColorDodgeBlend(),
             new LinearBurnBlend(),
             new LinearDodgeBlend(),
                     
             new OverlayBlend(),            
             new SoftLightBlend(),          
             new HardLightBlend(),      
             new VividLightBlend(),         
             new LinearLightBlend(),         
             new PinLightBlend(),
            
             new HardMixBlend(),     
             new DifferenceBlend(),            
             new ExclusionBlend(),
          
             new HueBlend(),
            new SaturationBlend(),          
             new ColorBlend(),
           
             new LuminosityBlend(),          
             new SubtractBlend(),           
             new DivisionBlend(),
        };
    }

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
