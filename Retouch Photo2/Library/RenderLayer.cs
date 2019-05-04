using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Models;
using System;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Library
{
    public class RenderLayer
    {
        /// <summary>
        /// Reload Renderlayer, which can be called multiple times
        /// </summary>
        /// <param name="project">Project类型</param>
        public void LoadFromProject(ICanvasResourceCreator creator, Project project)
        {
            this.Layers.Clear();

            if (project.Layers != null)
            {
                foreach (Layer layer in project.Layers)
                {
                    this.Layers.Add(layer);
                }
            }

            this.GrayWhiteGrid = this.GrayWhiteGridDraw(creator);
        }


        #region Index & Layers


        /// <summary>Current layer.</summary>      
        public Layer Layer
        {
            get
            {
                if (this.Index ==-1) return null;
                if (this.Layers.Count == 0) return null;

                if (this.Index >= 0 && this.Index < this.Layers.Count) return this.Layers[this.Index];

                return null;
            }
            set
            {
                if (value == null || this.Layers == null || this.Layers.Count == 0)
                {
                    this.Index = -1;
                    return;
                }

                if (this.Layers.Contains(value))
                {
                    this.Index = this.Layers.IndexOf(value);
                    return;
                }

                this.Index = -1;
                return;
            }
        }


        /// <summary>Index of layers.</summary>      
        public int Index =-1;

        /// <summary> All layers. </summary>  
        public ObservableCollection<Layer> Layers = new ObservableCollection<Layer>();


        /// <summary>
        /// Insert in layers.
        /// </summary>
        /// <param name="layer"> Which insert</param>
        public void Insert(Layer layer)
        {
            if (this.Layers.Count == 0)
            {
                this.Layers.Add(layer);
                this.Index = 0;
                return;
            }

            if (this.Index == -1) this.Index = 0;
            if (this.Index >= this.Layers.Count) this.Index = this.Layers.Count - 1;

            this.Layers.Insert(this.Index, layer);
        }

        /// <summary>
        /// Remove form layers.
        /// </summary>
        /// <param name="layer"> Which remove</param>
        public void Remove(Layer layer)
        {
            this.Layers.Remove(layer);
            layer = null;
        }

        /// <summary>
        /// Click on the layer
        /// </summary>
        /// <param name="point"> Click point</param>
        /// <param name="matrix">matrix </param>
        /// <returns></returns>
        public Layer GetClickedLayer(Vector2 point, Matrix3x2 matrix)
        {
            foreach (Layer layer in this.Layers)
            {
                if (layer.IsVisual == false || layer.Opacity == 0) continue;

                Vector2 leftTop = Vector2.Transform(layer.Transformer.DstLeftTop, matrix);
                Vector2 rightTop = Vector2.Transform(layer.Transformer.DstRightTop, matrix);
                Vector2 rightBottom = Vector2.Transform(layer.Transformer.DstRightBottom, matrix);
                Vector2 leftBottom = Vector2.Transform(layer.Transformer.DstLeftBottom, matrix);

                if (HomographyController.Transformer.InQuadrangle(point, leftTop, rightTop, rightBottom, leftBottom))
                {
                    return layer;
                }
            }

            return null;
        }


        #endregion


        #region Draw & Render


        /// <summary>Gray and white grid. </summary>
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


        /// <summary> 
        /// Get Render :
        ///   [Canvas] To [Virtual] on MatrixTransformer
        /// </summary>   
        public ICanvasImage RenderTarget;
        public ICanvasImage GetRender(Matrix3x2 canvasToVirtualMatrix, int width, int height, float scale)
        {
            ICanvasImage image = new ColorSourceEffect { Color = Colors.White };

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.LayerRender(this.Layers[i], image, canvasToVirtualMatrix);
            }


            Vector2 rightBottom = new Vector2(width, height) * scale / 2;
            Vector2 leftTop = -rightBottom;

            return new CropEffect
            {
                Source = image,
                SourceRectangle = new Rect(leftTop.ToPoint(), rightBottom.ToPoint()),
            };
        }
        public ICanvasImage GetRenderWithJumpedQueueLayer(Layer jumpedQueueLayer, Matrix3x2 canvasToVirtualMatrix, int width, int height, float scale)
        {
            ICanvasImage image = new ColorSourceEffect { Color = Colors.White };

            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                image = Layer.LayerRender(this.Layers[i], image, canvasToVirtualMatrix);

                /// Layer: 
                ///    jumped the Queue (Index: 0~n)     
                if (this.Index == i)
                {
                    image = Layer.LayerRender(jumpedQueueLayer, image, canvasToVirtualMatrix);
                }
            }

            /// Layer: 
            ///    jumped the Queue  (Index: -1)
            if (this.Index == -1)
            {
                image = Layer.LayerRender(jumpedQueueLayer, image, canvasToVirtualMatrix);
            }


            Vector2 rightBottom = new Vector2(width, height) * scale / 2;
            Vector2 leftTop = -rightBottom;

            return new CropEffect
            {
                Source = image,
                SourceRectangle = new Rect(leftTop.ToPoint(), rightBottom.ToPoint()),
            };
        }


        /// <summary> 
        /// Draw
        ///   [Virtual] To [Control] on MatrixTransformer
        /// </summary>
        public void Draw(CanvasDrawingSession ds, Matrix3x2 virtualToControlMatrix, Color shadowColor)
        {
            if (this.RenderTarget == null) return;

            ICanvasImage image = new Transform2DEffect
            {
                Source = this.RenderTarget,
                TransformMatrix = virtualToControlMatrix
            };
            ICanvasImage shadow = new ShadowEffect
            {
                Source = image,
                ShadowColor = shadowColor,
                BlurAmount = 4.0f
            };

            ds.DrawImage(shadow, 5.0f, 5.0f);
            ds.DrawImage(image);
        }


        #endregion

    }
}
