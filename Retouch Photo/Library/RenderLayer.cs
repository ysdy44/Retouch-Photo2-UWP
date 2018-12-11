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

            this.GrayWhiteGrid = new CanvasRenderTarget(creator, project.Width, project.Height);
            using (CanvasDrawingSession ds = this.GrayWhiteGrid.CreateDrawingSession())
            {
                ds.DrawImage(this.GrayWhiteGridDraw(creator));
            }

            this.RenderTarget = new CanvasRenderTarget(creator, project.Width, project.Height);
            this.Render(creator);
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
                return;
            }

            if (this.Index == -1) this.Index = 0;
            this.Layers.Insert(this.Index , layer);
        }
        public void Remove(Layer layer)
        {
            this.Layers.Remove(layer);
        }


        /// <summary>灰白网格</summary>
        public CanvasRenderTarget GrayWhiteGrid;
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
        private CanvasRenderTarget RenderTarget;
        public void Render(ICanvasResourceCreator creator)
        {
            ICanvasImage image = this.GrayWhiteGrid;

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.Render(creator, this.Layers[i], image);
            }

            using (CanvasDrawingSession ds = this.RenderTarget.CreateDrawingSession())
            {
                ds.DrawImage(image);
            }
        }
        public void RenderWithJumpedQueueLayer(ICanvasResourceCreator creator, Layer jumpedQueueLayer)
        {
            ICanvasImage image = this.GrayWhiteGrid;

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.Render(creator, this.Layers[i], image);

                //Layer: jumped the Queue 
                if (i == this.Index) image = Layer.Render(creator, jumpedQueueLayer, image);
            }

            using (CanvasDrawingSession ds = this.RenderTarget.CreateDrawingSession())
            {
                ds.DrawImage(image);
            }
        }
        

        /// <summary>Draw</summary>   
        public void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            if (this.RenderTarget == null) return;

            ICanvasImage image = new Transform2DEffect
            {
                Source = this.RenderTarget,
                TransformMatrix = matrix
            };
            ICanvasImage shadow = new OpacityEffect
            {
                Opacity = 0.2f,
                Source = new ShadowEffect
                {
                    Source = image,
                    ShadowColor = Colors.Black,
                }
            };

            ds.DrawImage(shadow, 5.0f, 5.0f);
            ds.DrawImage(image);
        }

    }
}
