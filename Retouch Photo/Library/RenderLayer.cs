using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Retouch_Photo.Library
{
   public class RenderLayer
    {
        /// <summary>重新加载RenderLayer，可以多次调用</summary>
        /// <param name="project">Project类型</param>
        public void LoadFromProject(ICanvasResourceCreatorWithDpi creator, Project project)
        {
            this.Layers.Clear();
            foreach (Layer layer in project.Layers) this.Layers.Add(layer);

            this.GrayWhiteGrid = this.GrayWhiteGridDraw(creator, project.Width, project.Height);
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

        public void SetIndex(Layer layer)
        {
            if (this.Layers == null || this.Layers.Count == 0)
            {
                this.Index = -1;
                return;
            }
            if (this.Layers.Count == 1 || this.Layers.Contains(layer) == false)
            {
                this.Index = 0;
                return;
            }
            this.Index = this.Layers.IndexOf(layer);
        }


        /// <summary>所有图层</summary>  
        public ObservableCollection<Layer> Layers = new ObservableCollection<Layer>();    
        public Layer CurrentLayer()
        {
            if (this.Layers.Count == 0) return null;

            if (this.Layers.Count == 1) return this.Layers.First();

            if (this.Index >= 0 && this.Index < this.Layers.Count()) return this.Layers[this.Index];

            return null;
        }
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
        }


        /// <summary>灰白网格</summary>
        public ICanvasImage GrayWhiteGrid;
        private ICanvasEffect GrayWhiteGridDraw(ICanvasResourceCreator creator,int width,int height)
        {
            return new CropEffect
            {
                SourceRectangle = new Windows.Foundation.Rect(0, 0, width, height),
                Source = new DpiCompensationEffect//根据DPI适配
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
                }

            };
        }


        /// <summary>生成渲染</summary>   
        public ICanvasImage RenderTarget;
        public ICanvasImage GetRender(ICanvasResourceCreator creator,Matrix3x2 canvasToVirtualMatrix)
        {
            ICanvasImage image = new Transform2DEffect
            {
                TransformMatrix = canvasToVirtualMatrix,
                Source = this.GrayWhiteGrid
            };

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.Render(creator, this.Layers[i], image, canvasToVirtualMatrix);
            }

            return image;
        }    
        public ICanvasImage GetRenderWithJumpedQueueLayer(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix, Layer jumpedQueueLayer)
        {
            ICanvasImage image = new Transform2DEffect
            {
                TransformMatrix = canvasToVirtualMatrix,
                Source = this.GrayWhiteGrid
            };

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.Render(creator, this.Layers[i], image, canvasToVirtualMatrix);

                //Layer: jumped the Queue (Index: 0~n)
                if (this.Index == i) image = Layer.Render(creator, jumpedQueueLayer, image, canvasToVirtualMatrix);
            }

            //Layer: jumped the Queue  (Index: -1)
            if (this.Index == -1) image = Layer.Render(creator, jumpedQueueLayer, image, canvasToVirtualMatrix);

            return image;
        }



        /// <summary>Draw</summary>   
        public void Draw(ICanvasImage renderImage, CanvasDrawingSession ds, Matrix3x2 virtualToControlMatrix)
        {
            if (renderImage == null) return;
           
            /*
             
            ICanvasImage shadow = new OpacityEffect
            {
                Opacity = 0.2f,
                Source = new ShadowEffect
                {
                    ShadowColor = Colors.Black,
                    Source = renderImage
                }
            };

            ds.DrawImage(shadow, 5.0f, 5.0f);
             */


            ds.DrawImage(new Transform2DEffect
            {
                Source=renderImage,
                TransformMatrix=virtualToControlMatrix
            });
        }



    }
}
