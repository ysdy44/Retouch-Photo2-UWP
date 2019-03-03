using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo.Models;
using System;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Library
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

            if (project.Layers!=null)
            {
                foreach (Layer layer in project.Layers)
                {
                    this.Layers.Add(layer);
                }
            }

            this.GrayWhiteGrid = this.GrayWhiteGridDraw(creator);
        }


        #region Index & Layers


        /// <summary>Index of layers.</summary>      
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

        /// <summary> All layers. </summary>  
        public ObservableCollection<Layer> Layers = new ObservableCollection<Layer>();


        /// <summary>
        /// Insert in layers.
        /// </summary>
        /// <param name="layer"> Which insert</param>
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

        /// <summary>
        /// Remove form layers.
        /// </summary>
        /// <param name="layer"> Which remove</param>
        public void Remove(Layer layer)
        {
            this.Layers.Remove(layer);
            layer = null;
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


        /// <summary>生成渲染</summary>   
        public ICanvasImage RenderTarget;
        public ICanvasImage GetRender(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix, int width, int height, float scale)
        {
            ICanvasImage image = new ScaleEffect
            {
                Scale = new Vector2(scale),
                //Source = this.GrayWhiteGrid
                Source = new ColorSourceEffect{Color=Colors.White}
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
                //Source = this.GrayWhiteGrid
                Source = new ColorSourceEffect { Color = Colors.White }
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
        /// Draw.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="virtualToControlMatrix"></param>
        public void Draw(CanvasDrawingSession ds, Matrix3x2 virtualToControlMatrix)
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
                ShadowColor = Color.FromArgb(64, 0, 0, 0),
                BlurAmount = 4.0f
            };

            ds.DrawImage(shadow, 5.0f, 5.0f);
            ds.DrawImage(image);
        }


        #endregion


        #region Ruler & Text


        /// <summary>  Draw rulers line or no. </summary>
        public bool IsRuler;


        readonly CanvasTextFormat RulerTextFormat = new CanvasTextFormat() { FontSize = 12, HorizontalAlignment = CanvasHorizontalAlignment.Center, VerticalAlignment = CanvasVerticalAlignment.Center, };
        readonly float RulerSpace = 20;

        /// <summary>
        /// Draw rulers line.
        /// </summary>
        /// <param name="ds"> Drawing sessions are used to issue graphics drawing commands. This is the main way to draw things onto a canvas. </param>
        public void RulerDraw(CanvasDrawingSession ds, MatrixTransformer transformer)
        {
            if (this.IsRuler == false) return;


            //line
            ds.FillRectangle(0, 0, transformer.ControlWidth, this.RulerSpace, Windows.UI.Color.FromArgb(64, 127, 127, 127));//Horizontal
            ds.FillRectangle(0, 0, this.RulerSpace, transformer.ControlHeight, Windows.UI.Color.FromArgb(64, 127, 127, 127));//Vertical
            ds.DrawLine(0, this.RulerSpace, transformer.ControlWidth, this.RulerSpace, Windows.UI.Colors.Gray);//Horizontal
            ds.DrawLine(this.RulerSpace, 0, this.RulerSpace, transformer.ControlHeight, Windows.UI.Colors.Gray);//Vertical

            //space
            float space = (10 * transformer.Scale);
            while (space < 10) space *= 5;
            while (space > 100) space /= 5;
            float spaceFive = space * 5;

            //Horizontal
            for (float X = (float)transformer.Position.X; X < transformer.ControlWidth; X += space) ds.DrawLine(X, 10, X, this.RulerSpace, Windows.UI.Colors.Gray);
            for (float X = (float)transformer.Position.X; X > this.RulerSpace; X -= space) ds.DrawLine(X, 10, X, this.RulerSpace, Windows.UI.Colors.Gray);
            //Vertical
            for (float Y = (float)transformer.Position.Y; Y < transformer.ControlHeight; Y += space) ds.DrawLine(10, Y, this.RulerSpace, Y, Windows.UI.Colors.Gray);
            for (float Y = (float)transformer.Position.Y; Y > this.RulerSpace; Y -= space) ds.DrawLine(10, Y, this.RulerSpace, Y, Windows.UI.Colors.Gray);

            //Horizontal
            for (float X = (float)transformer.Position.X; X < transformer.ControlWidth; X += spaceFive) ds.DrawLine(X, 10, X, this.RulerSpace, Windows.UI.Colors.Gray);
            for (float X = (float)transformer.Position.X; X > this.RulerSpace; X -= spaceFive) ds.DrawLine(X, 10, X, this.RulerSpace, Windows.UI.Colors.Gray);
            //Vertical
            for (float Y = (float)transformer.Position.Y; Y < transformer.ControlHeight; Y += spaceFive) ds.DrawLine(10, Y, this.RulerSpace, Y, Windows.UI.Colors.Gray);
            for (float Y = (float)transformer.Position.Y; Y > this.RulerSpace; Y -= spaceFive) ds.DrawLine(10, Y, this.RulerSpace, Y, Windows.UI.Colors.Gray);

            //Horizontal
            for (float X = (float)transformer.Position.X; X < transformer.ControlWidth; X += spaceFive) ds.DrawText(((int)(Math.Round((X - transformer.Position.X) / transformer.Scale))).ToString(), X, 10, Windows.UI.Colors.Gray, RulerTextFormat);
            for (float X = (float)transformer.Position.X; X > this.RulerSpace; X -= spaceFive) ds.DrawText(((int)(Math.Round((X - transformer.Position.X) / transformer.Scale))).ToString(), X, 10, Windows.UI.Colors.Gray, RulerTextFormat);
            //Vertical
            for (float Y = (float)transformer.Position.Y; Y < transformer.ControlHeight; Y += spaceFive) ds.DrawText(((int)(Math.Round((Y - transformer.Position.Y) / transformer.Scale))).ToString(), 10, Y, Windows.UI.Colors.Gray, RulerTextFormat);
            for (float Y = (float)transformer.Position.Y; Y > this.RulerSpace; Y -= spaceFive) ds.DrawText(((int)(Math.Round((Y - transformer.Position.Y) / transformer.Scale))).ToString(), 10, Y, Windows.UI.Colors.Gray, RulerTextFormat);
        }


        #endregion

    }
}
