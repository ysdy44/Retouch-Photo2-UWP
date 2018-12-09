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
            this.Render();
        }



        /// <summary>所有图层</summary>      
        public ObservableCollection<Layer> Layers = new ObservableCollection<Layer>();
        


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
        public void Render()
        {
            ICanvasImage image = this.GrayWhiteGrid;

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.Render(this.Layers[i], image);
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
