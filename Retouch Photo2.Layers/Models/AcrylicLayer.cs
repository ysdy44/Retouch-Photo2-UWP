using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Icons;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s AcrylicLayer .
    /// </summary>
    public class AcrylicLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Acrylic;

        //@Content       
        public float TintOpacity = 0.5f;
        public float BlurAmount = 12.0f;

        //@Construct
        /// <summary>
        /// Construct a acrylic-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public AcrylicLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a acrylic-layer.
        /// </summary>
        public AcrylicLayer()
        {
            base.StyleManager.FillBrush.Color = Color.FromArgb(255, 255, 255, 255);
            base.Control = new LayerControl(this)
            {
                Icon = new AcrylicIcon(),
                Text = "Acrylic",
            };
        }   


        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, canvasToVirtualMatrix);

            return new CropEffect
            {
                SourceRectangle = new Rect(leftTop.ToPoint(), rightBottom.ToPoint()),
                Source = new CompositeEffect
                {
                    Sources =
                    {
                        new GaussianBlurEffect
                        {
                             BlurAmount = this.BlurAmount,
                             Source = previousImage
                        },
                        new OpacityEffect
                        {
                            Opacity = this.TintOpacity,
                            Source = new ColorSourceEffect
                            {
                                Color = base.StyleManager.FillBrush.Color
                             }
                         }
                    }
                }
            };
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            AcrylicLayer acrylicLayer = new AcrylicLayer
            {
                TintOpacity = this.TintOpacity,
                BlurAmount = this.BlurAmount,
            };

            LayerBase.CopyWith(resourceCreator, acrylicLayer, this);
            return acrylicLayer;
        }
        

        public void SaveWith(XElement element)
        {
            element.Add(new XElement("TintOpacity", this.TintOpacity));
            element.Add(new XElement("BlurAmount", this.BlurAmount));
        }
        public void Load(XElement element)
        {
            this.TintOpacity = (float)element.Element("TintOpacity");
            this.BlurAmount = (float)element.Element("BlurAmount");
        }

    }
}