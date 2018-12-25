using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Models.Blends;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models
{
    public abstract class Blend
    {
        public BlendType Type;
        public FrameworkElement Icon => this.GetIcon();

        protected abstract FrameworkElement GetIcon( );
        protected abstract ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground);


        public static ICanvasImage Render(ICanvasImage background, ICanvasImage foreground, BlendType type) => Blend.BlendList.FirstOrDefault(e => e.Type == type).GetRender(background, foreground);
        public static List<Blend> BlendList = new List<Blend>
        {
             new NormalBlend(),//正常
            
             new MultiplyBlend(),//正片叠底            
             new ScreenBlend(),//屏幕            
             new DissolveBlend(),//溶解
            
             new DarkenBlend(),//暗            
             new LightenBlend(),//亮          
             new DarkerColorBlend(),//更暗的颜色           
             new LighterColorBlend(),//更亮的颜色
          
             new ColorBurnBlend(),//颜色加深            
             new ColorDodgeBlend(),//颜色减淡          
             new LinearBurnBlend(),//线性加深          
             new LinearDodgeBlend(),//线性减淡
                     
             new OverlayBlend(),//覆盖            
             new SoftLightBlend(),//柔光          
             new HardLightBlend(),//硬光      
             new VividLightBlend(),//艳光         
             new LinearLightBlend(),//线性光         
             new PinLightBlend(),//射灯
            
             new HardMixBlend(),//硬混     
             new DifferenceBlend(),//差异            
             new ExclusionBlend(),//排除
          
             new HueBlend(),//色相          
             new SaturationBlend(),//饱和度          
             new ColorBlend(),//颜色
           
             new LuminosityBlend(),//光度          
             new SubtractBlend(),//减去           
             new DivisionBlend(),//反色
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
