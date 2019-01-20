using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Library
{
    public class RenderLayer
    {
        /// <summary>重新加载RenderLayer，可以多次调用</summary>
        /// <param name="project">Project类型</param>
        public void LoadFromProject(ICanvasResourceCreator creator, Project project)
        {
            this.Layers.Clear();
            foreach (Layer layer in project.Layers) this.Layers.Add(layer);

            this.GrayWhiteGrid = this.GrayWhiteGridDraw(creator);
        }
        

        /// <summary>索引</summary>      
        public int Index
        {
            set => index = value;
            get
            {
                if (this.Layers == null || this.Layers.Count == 0) return -1;
                if (this.Layers.Count == 1 ) return 0;
                return index;
            }
        }
        private int index=-1;
        /// <summary>所有图层</summary>  
        public ObservableCollection<Layer> Layers = new ObservableCollection<Layer>();    
    

        public void Insert(Layer layer)
        {
            if (this.Layers.Count==0)
            {
                this.Layers.Add(layer);
                this.Index = 0;
                return;
            }

            if (this.Index == -1) this.Index = 0;
            if (this.Index >= this.Layers.Count) this.Index = this.Layers.Count - 1;

            this.Layers.Insert(this.Index , layer);
        }
        public void Remove(Layer layer)
        {
            this.Layers.Remove(layer);
            layer = null;
        }


        /// <summary>灰白网格</summary>
        public ICanvasImage GrayWhiteGrid;
        private ICanvasEffect GrayWhiteGridDraw(ICanvasResourceCreator creator)
        {
            return new DpiCompensationEffect//根据DPI适配
            {
                Source = new ScaleEffect//缩放
                {
                    Scale = new Vector2(8, 8),
                    InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                    Source = new BorderEffect//无限延伸图片
                    {
                        ExtendX = CanvasEdgeBehavior.Wrap,
                        ExtendY = CanvasEdgeBehavior.Wrap,
                        Source = CanvasBitmap.CreateFromColors
                             (
                                resourceCreator: creator,
                                widthInPixels: 2,
                                heightInPixels: 2,
                                colors: new Color[] //从数组创建2x2图片
                                {
                                   Color.FromArgb(255, 233, 233, 233),Colors.White,
                                   Colors.White,Color.FromArgb(255, 233, 233, 233),
                                }
                             )
                    }
                }
            };
        }


        /// <summary>生成渲染</summary>   
        public ICanvasImage RenderTarget;
        public ICanvasImage GetRender(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix, int width, int height, float scale)
        {
            ICanvasImage image = new ScaleEffect
            {
                Scale = new Vector2(scale),
                Source = this.GrayWhiteGrid
            };

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.LayerRender(creator, this.Layers[i], image, canvasToVirtualMatrix);
            }

            return new CropEffect
            {
                Source = image,
                SourceRectangle = new Rect(-width / 2 * scale, -height / 2 * scale, width * scale, height * scale),
            };
        }    
        public ICanvasImage GetRenderWithJumpedQueueLayer(ICanvasResourceCreator creator, Layer jumpedQueueLayer, Matrix3x2 canvasToVirtualMatrix, int width, int height, float scale)
        {
            ICanvasImage image = new ScaleEffect
            {
                Scale = new Vector2(scale),
                Source = this.GrayWhiteGrid
            };

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.LayerRender(creator, this.Layers[i], image, canvasToVirtualMatrix);
                if (this.Index == i) image = Layer.LayerRender(creator, jumpedQueueLayer, image, canvasToVirtualMatrix);//Layer: jumped the Queue (Index: 0~n)
            }
            if (this.Index == -1) image = Layer.LayerRender(creator, jumpedQueueLayer, image, canvasToVirtualMatrix); //Layer: jumped the Queue  (Index: -1)

            return new CropEffect
            {
                Source = image,
                SourceRectangle = new Rect(-width / 2 * scale, -height / 2 * scale, width * scale, height * scale),
            };
        }


        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="virtualToControlMatrix"></param>
        public void Draw(CanvasDrawingSession ds, Matrix3x2 virtualToControlMatrix)
        {
            if (this.RenderTarget == null) return;
            

            if (this.RenderTarget == null) return;

            ICanvasImage image = new Transform2DEffect
            {
                Source = this.RenderTarget,
                TransformMatrix = virtualToControlMatrix
            };
           // ICanvasImage shadow = new ShadowEffect
           // {
            //    Source = image,
                //ShadowColor=Color.FromArgb(64,0,0,0),
                 //BlurAmount=4.0f
            //};

            //ds.DrawImage(shadow, 5.0f, 5.0f);
            ds.DrawImage(image);

        }



     }
}
