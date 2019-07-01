using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Blends.Models;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Blend Classes.
    /// </summary>
    public abstract class Blend
    {
        /// <summary> <see cref="Blend">'s type. </summary>
        public BlendType Type;
        /// <summary> <see cref="Blend">'s icon. </summary>
        public FrameworkElement Icon => this.GetIcon();

        //@Abstract
        /// <summary>
        /// Gets icon.
        /// </summary>
        /// <returns></returns>
        protected abstract FrameworkElement GetIcon();
        /// <summary>
        /// Gets a specific rended-blend.
        /// </summary>
        /// <param name="background"> Background image. </param>
        /// <param name="foreground"> Foreground image. </param>
        /// <returns> ICanvasImage </returns>
        protected abstract ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground);

        //@Static
        /// <summary>
        /// Render images and layers together.
        /// </summary>      
        /// <param name="background"> Background image. </param>
        /// <param name="foreground"> Foreground image. </param>
        /// <param name="type"> Type </param>
        /// <returns> ICanvasImage </returns>
        public static ICanvasImage Render(ICanvasImage background, ICanvasImage foreground, BlendType type) => Blend.BlendList.FirstOrDefault(e => e.Type == type).GetRender(background, foreground);

        //@Static
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