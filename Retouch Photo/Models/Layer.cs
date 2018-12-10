using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.ComponentModel;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models.Layers;

namespace Retouch_Photo.Models
{
    public abstract class Layer: INotifyPropertyChanged
    {
        
        private string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double opacity = 100;
        public double Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }

        private bool isVisual = true;
        public bool IsVisual
        {
            get => isVisual;
            set
            {
                isVisual = value;
                OnPropertyChanged(nameof(IsVisual));
            }
        }

        private int blendIndex;
        public int BlendIndex
        {
            get => blendIndex;
            set
            {
                blendIndex = value;
                OnPropertyChanged(nameof(BlendIndex));
            }
        }

        public abstract ICanvasImage GetRender(ICanvasResourceCreator creator);

        public static Layer CreateFromXElement(ICanvasResourceCreatorWithDpi resourceCreator, XElement element)
        {
            int width = (int)element.Element("LayerWidth");
            int height = (int)element.Element("LayerHeight");

            string strings = element.Element("CanvasRenderTarget").Value;
            byte[] bytes = Convert.FromBase64String(strings);

           CanvasRenderTarget renderTarget = new CanvasRenderTarget(resourceCreator, width, height);
           renderTarget.SetPixelBytes(bytes);

            return new PixelLayer
            {
                Name = element.Element("LayerName").Value,
                IsVisual = (bool)element.Element("LayerVisual"),
                Opacity = (double)element.Element("LayerOpacity"),
                BlendIndex = (int)element.Element("LayerBlendIndex"),
                CanvasRenderTarget=renderTarget
            };
        }







        /// <summary>
        /// 渲染图层
        /// </summary>
        /// <param name="layer">当前图层</param>
        /// <param name="image">从当前图层上面 传下来的 图像</param>
        /// <returns>新的 向下传递的 图像</returns>
        public static ICanvasImage Render(ICanvasResourceCreator creator, Layer layer, ICanvasImage image)
        {
            if (layer.IsVisual == false || layer.Opacity == 0) return image;

            return Layer.BlendRender
            (
               foreground: image,
               blendIndex: layer.BlendIndex,
               background: (layer.Opacity == 100) ? layer.GetRender(creator) : new OpacityEffect
               {
                   Source = layer.GetRender(creator),
                   Opacity = (float)(layer.Opacity / 100)
               }
            );
        }

        private static ICanvasImage BlendRender(ICanvasImage background, ICanvasImage foreground, int blendIndex)
        {
            if (blendIndex == 0) return new CompositeEffect
            {
                Sources = { foreground, background }
            };

            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = Layer.BlendMode(blendIndex)
            };
        }
        
        public static BlendEffectMode BlendMode(int blendIndex)
        {
            switch (blendIndex)
            {
                case 1: return BlendEffectMode.Multiply;

                case 2: return BlendEffectMode.Screen;
                case 3: return BlendEffectMode.Dissolve;

                case 4: return BlendEffectMode.Darken;
                case 5: return BlendEffectMode.Lighten;
                case 6: return BlendEffectMode.DarkerColor;
                case 7: return BlendEffectMode.LighterColor;

                case 8: return BlendEffectMode.ColorBurn;
                case 9: return BlendEffectMode.ColorDodge;
                case 10: return BlendEffectMode.LinearBurn;
                case 11: return BlendEffectMode.LinearDodge;

                case 12: return BlendEffectMode.Overlay;
                case 13: return BlendEffectMode.SoftLight;
                case 14: return BlendEffectMode.HardLight;
                case 15: return BlendEffectMode.VividLight;
                case 16: return BlendEffectMode.LinearLight;
                case 17: return BlendEffectMode.PinLight;

                case 18: return BlendEffectMode.HardMix;
                case 19: return BlendEffectMode.Difference;
                case 20: return BlendEffectMode.Exclusion;

                case 21: return BlendEffectMode.Hue;
                case 22: return BlendEffectMode.Saturation;
                case 23: return BlendEffectMode.Color;

                case 24: return BlendEffectMode.Luminosity;
                case 25: return BlendEffectMode.Subtract;
                case 26: return BlendEffectMode.Division;

                default: return BlendEffectMode.Multiply;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
