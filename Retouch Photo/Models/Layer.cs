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
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Microsoft.Graphics.Canvas.UI;
using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.Models.Blends;

namespace Retouch_Photo.Models
{
    public abstract class Layer: INotifyPropertyChanged
    {

        #region Property

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

        public List<Adjustment> Adjustments = new List<Adjustment>();

        #endregion
                     

        #region Thumbnail


        //@override
        public abstract void ThumbnailDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Size controlSize);


        CanvasControl sender;
        public void CanvasControl_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args) => this.sender = sender;
        public void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args) => this.ThumbnailDraw(sender, args.DrawingSession, sender.Size);
        public void Invalidate()
        {
            if (sender == null) return;

            this.sender.Invalidate();
        }


        public static Rect GetThumbnailSize(float width, float height, Size controlSize)
        {
            double widthScale = controlSize.Width / width;
            double heightScale = controlSize.Height / height;

            double scale = Math.Min(widthScale, heightScale);
            double w = width * scale;
            double h = height * scale;

            double x = (controlSize.Width - w) / 2;
            double y = (controlSize.Height - h) / 2;

            return new Rect(x, y, w, h);
        }
        public static Matrix3x2 GetThumbnailMatrix(float width, float height, Size controlSize)
        {
            double widthScale = controlSize.Width / width;
            double heightScale = controlSize.Height / height;

            double scale = Math.Min(widthScale, heightScale);
            double w = width * scale;
            double h = height * scale;

            double x = (controlSize.Width - w) / 2;
            double y = (controlSize.Height - h) / 2;

            return Matrix3x2.CreateScale((float)scale) *
              Matrix3x2.CreateTranslation((float)x, (float)y);
        }


        #endregion


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
            layer.BlendIndex =(int)element.Element("LayerBlendIndex");

            return layer;
        }


        //@override
        protected abstract ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix);
    
        //@static
        /// <summary> LayerRender </summary>
        /// <param name="layer">当前图层</param>
        /// <param name="image">从当前图层上面 传下来的 图像</param>
        /// <returns>新的 向下传递的 图像</returns>
        public static ICanvasImage LayerRender(ICanvasResourceCreator creator, Layer layer, ICanvasImage image,Matrix3x2 canvasToVirtualMatrix)
        {
            if (layer.IsVisual == false || layer.Opacity == 0) return image;

            var adjustment = Adjustment.Render
            (
                adjustments: layer.Adjustments,
                image: layer.GetRender(creator, image, canvasToVirtualMatrix)
            );

            return Blend.Render
            (
               type: (BlendType)layer.BlendIndex,
               foreground: image,
               background: (layer.Opacity == 100) ? adjustment : new OpacityEffect
               {
                   Source = adjustment,
                   Opacity = (float)(layer.Opacity / 100)
               }
            );
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
