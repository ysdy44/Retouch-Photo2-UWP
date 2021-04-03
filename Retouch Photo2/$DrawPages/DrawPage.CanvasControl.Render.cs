using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {


        //CanvasControl
        private void CanvasControlInvalidate(InvalidateMode mode)
        {
            //High-Display screen
            if (this.LayerRenderCanvasControl.Dpi > 96.0f)
            {
                switch (mode)
                {
                    case InvalidateMode.Thumbnail:
                        float dpiScale = 96.0f / this.LayerRenderCanvasControl.Dpi;
                        if (dpiScale < 0.4f) dpiScale = 0.4f;
                        if (dpiScale > 1.0f) dpiScale = 1.0f;

                        this.LayerRenderCanvasControl.DpiScale = dpiScale;
                        break;
                    case InvalidateMode.HD:
                        this.LayerRenderCanvasControl.DpiScale = 1.0f;
                        break;
                }
            }

            this.LayerRenderCanvasControl.Invalidate();
            this.ToolDrawCanvasControl.Invalidate();
        }


        /// <summary>
        /// Render.
        /// </summary>
        /// <returns> The render image. </returns>
        public ICanvasImage Render()
        {
            return LayerBase.Render(LayerManager.CanvasDevice, LayerManager.RootLayerage);
        }


        private void DrawRenderAndCrad(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawCard(new ColorSourceEffect
            {
                Color = Colors.White
            },
            this.ViewModel.CanvasTransformer,
            this.ShadowColor);

            ICanvasImage canvasImage = this.Render();
            if (canvasImage == null) return;

            drawingSession.DrawImage(new Transform2DEffect
            {
                Source = canvasImage,
                TransformMatrix = this.ViewModel.CanvasTransformer.GetMatrix()
            });
        }


        /// <summary>
        /// Render image.
        /// </summary>
        /// <param name="fileWidth"> The file width.</param>
        /// <param name="fileHeight"> The file height.</param>
        /// <param name="isClearWhite"> Clears to the white color. </param>
        /// <returns> The file image. </returns>
        public CanvasRenderTarget Render(int fileWidth = 256, int fileHeight = 256, bool isClearWhite = true)
        {
            ICanvasImage canvasImage = this.Render();

            int canvasWidth = this.ViewModel.CanvasTransformer.Width;
            int canvasHeight = this.ViewModel.CanvasTransformer.Height;

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(LayerManager.CanvasDevice, fileWidth, fileHeight, 96);
            if (canvasImage == null) return renderTarget;


            float scaleX = (float)fileWidth / (float)canvasWidth;
            float scaleY = (float)fileHeight / (float)canvasHeight;
            Matrix3x2 matrix =
                Matrix3x2.CreateTranslation(-canvasWidth / 2, -canvasHeight / 2) *
                Matrix3x2.CreateScale(Math.Min(scaleX, scaleY)) *
                Matrix3x2.CreateTranslation(fileWidth / 2, fileHeight / 2);

            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                if (isClearWhite) drawingSession.Clear(Colors.White);

                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = matrix,
                    Source = canvasImage
                });
            }
            return renderTarget;
        }


        /// <summary>
        /// Render image.
        /// </summary>
        /// <param name="size"> The size. </param>
        /// <param name="dpi"> The dpi. </param>
        /// <param name="isClearWhite"> Clears to the white color. </param>
        /// <returns> The image. </returns>
        public CanvasRenderTarget Render(BitmapSize size, float dpi, bool isClearWhite = true)
        {
            ICanvasImage canvasImage = this.Render();

            int canvasWidth = this.ViewModel.CanvasTransformer.Width;
            int canvasHeight = this.ViewModel.CanvasTransformer.Height;

            int fileWidth = (int)size.Width;
            int fileHeight = (int)size.Height;

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(LayerManager.CanvasDevice, fileWidth, fileHeight, dpi);
            if (canvasImage == null) return renderTarget;


            if (canvasWidth == fileWidth && canvasHeight == fileHeight)
            {
                using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
                {
                    if (isClearWhite) drawingSession.Clear(Colors.White);

                    drawingSession.DrawImage(canvasImage);
                }
                return renderTarget;
            }


            float scaleX = (float)fileWidth / (float)canvasWidth;
            float scaleY = (float)fileHeight / (float)canvasHeight;
            Matrix3x2 matrix = Matrix3x2.CreateScale(scaleX, scaleY);

            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                drawingSession.DrawImage(new Transform2DEffect
                {
                    TransformMatrix = matrix,
                    Source = canvasImage
                });
            }
            return renderTarget;
        }

    }
}