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
using Windows.Foundation;
using Retouch_Photo.ViewModels;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Windows.Graphics.Effects;
using System.Numerics;

namespace Retouch_Photo.Models
{
    public abstract class Layer: INotifyPropertyChanged
    {
        //Property

        private string name = "Layer";
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


        public Transformer Transformer;


        //abstract
        public abstract ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix);
        


        //Create
        public static Layer CreateFromXElement(ICanvasResourceCreatorWithDpi creator, XElement element)
        {
            int width = (int)element.Element("LayerWidth");
            int height = (int)element.Element("LayerHeight");

            string strings = element.Element("CanvasRenderTarget").Value;
            byte[] bytes = Convert.FromBase64String(strings);

            ImageLayer layer = ImageLayer.CreateFromBytes(creator, bytes, width, height);
            layer.Name = element.Element("LayerName").Value;
            layer.IsVisual = (bool)element.Element("LayerVisual");
            layer.Opacity = (double)element.Element("LayerOpacity");
            layer.BlendIndex = (int)element.Element("LayerBlendIndex");

            return layer;
        }



        /// <summary>Render</summary>
        /// <param name="layer">当前图层</param>
        /// <param name="image">从当前图层上面 传下来的 图像</param>
        /// <returns>新的 向下传递的 图像</returns>
        public static ICanvasImage Render(ICanvasResourceCreator creator, Layer layer, ICanvasImage image,Matrix3x2 canvasToVirtualMatrix)
        {
            if (layer.IsVisual == false || layer.Opacity == 0) return image;
           
            return Layer.BlendRender
            (
               foreground: image,
               blendIndex: layer.BlendIndex,
               background: (layer.Opacity == 100) ? layer.GetRender(creator, image, canvasToVirtualMatrix) : new OpacityEffect
               {
                   Source = layer.GetRender(creator, image, canvasToVirtualMatrix),
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

        

        //Delegate
        public delegate void RemoveHandler(Layer layer);
        public static event RemoveHandler Remove = null;
        public void RemoveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Layer.Remove?.Invoke(this);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
