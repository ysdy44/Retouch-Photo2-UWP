using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Blends.Models;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Blends
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
}
