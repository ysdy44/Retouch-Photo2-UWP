using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Models;
using System;
using System.Numerics;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel
    {

        /// <summary> Gets or sets the canvas transformer. </summary>
        public CanvasTransformer CanvasTransformer { get; } = new CanvasTransformer();


        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s radian. </summary>
        public float CanvasTransformerRadian => this.CanvasTransformer.Radian;
        /// <summary> Notify <see cref="CanvasTransformerRadian"/>. </summary>
        public void NotifyCanvasTransformerRadian() => this.OnPropertyChanged(nameof(CanvasTransformerRadian));//Notify 
        /// <summary> Set <see cref="CanvasTransformerRadian"/>. </summary>
        public void SetCanvasTransformerRadian(float radian)
        {
            this.CanvasTransformer.Radian = radian;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerRadian();//Notify
        }
        /// <summary> Left rotate. </summary>
        public void CanvasTransformerLeftRotate(float sweep = 0.1f)
        {
            float radian = this.CanvasTransformer.Radian;
            radian -= sweep;
            if (radian < ViewRadianConverter.MinNumber) radian = ViewRadianConverter.MinNumber;
            this.SetCanvasTransformerRadian(radian);
        }
        /// <summary> Right rotate. </summary>
        public void CanvasTransformerRightRotate(float sweep = 0.1f)
        {
            float radian = this.CanvasTransformer.Radian;
            radian += sweep;
            if (radian > ViewRadianConverter.MaxNumber) radian = ViewRadianConverter.MaxNumber;
            this.SetCanvasTransformerRadian(radian);
        }



        /// <summary> <see cref = "ViewModel.CanvasTransformer" />'s scale. </summary>
        public float CanvasTransformerScale => this.CanvasTransformer.Scale;
        /// <summary> Notify <see cref="CanvasTransformerScale"/>. </summary>
        public void NotifyCanvasTransformerScale() => this.OnPropertyChanged(nameof(CanvasTransformerScale));//Notify 
        /// <summary> Set <see cref="CanvasTransformerScale"/>. </summary>
        public void SetCanvasTransformerScale(float scale)
        {
            this.CanvasTransformer.Scale = scale;
            this.CanvasTransformer.ReloadMatrix();

            this.NotifyCanvasTransformerScale();//Notify
        }



        /// <summary>
        /// Draw crad.
        /// </summary>
        public void DrawRenderAndCrad(CanvasDrawingSession drawingSession, Color shadowColor)
        {
            drawingSession.DrawCard(new ColorSourceEffect
            {
                Color = Colors.White
            },
            this.CanvasTransformer, shadowColor);

            ICanvasImage canvasImage = LayerBase.Render(LayerManager.CanvasDevice, LayerManager.RootLayerage);
            if (canvasImage == null) return;

            drawingSession.DrawImage(new Transform2DEffect
            {
                Source = canvasImage,
                TransformMatrix = this.CanvasTransformer.GetMatrix()
            });
        }


        /// <summary>
        /// Render all layers with size.
        /// </summary>
        /// <param name="fileWidth"> The file width.</param>
        /// <param name="fileHeight"> The file height.</param>
        /// <param name="isClearWhite"> Clears to the white color. </param>
        /// <returns> The file image. </returns>
        public CanvasRenderTarget Render(int fileWidth = 256, int fileHeight = 256, bool isClearWhite = true)
        {
            ICanvasImage canvasImage = LayerBase.Render(LayerManager.CanvasDevice, LayerManager.RootLayerage);

            int canvasWidth = this.CanvasTransformer.Width;
            int canvasHeight = this.CanvasTransformer.Height;

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(LayerManager.CanvasDevice, fileWidth, fileHeight, 96);
            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                if (isClearWhite) drawingSession.Clear(Colors.White);

                if (canvasImage != null)
                {
                    float scaleX = fileWidth / canvasWidth;
                    float scaleY = fileHeight / canvasHeight;

                    Matrix3x2 matrix =
                        Matrix3x2.CreateTranslation(-canvasWidth / 2, -canvasHeight / 2) *
                        Matrix3x2.CreateScale(Math.Min(scaleX, scaleY)) *
                        Matrix3x2.CreateTranslation(fileWidth / 2, fileHeight / 2);

                    drawingSession.DrawImage(new Transform2DEffect
                    {
                        TransformMatrix = matrix,
                        Source = canvasImage
                    });
                }
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
        public CanvasRenderTarget Render(float width, float height , float dpi, bool isClearWhite = true)
        {
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(LayerManager.CanvasDevice, width, height, dpi);

            using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
            {
                if (isClearWhite) drawingSession.Clear(Colors.White);

                ICanvasImage canvasImage = LayerBase.Render(LayerManager.CanvasDevice, LayerManager.RootLayerage);
                if (canvasImage != null)
                {
                    int canvasWidth = this.CanvasTransformer.Width;
                    int canvasHeight = this.CanvasTransformer.Height;

                    if (canvasWidth == width && canvasHeight == height)
                    {
                        drawingSession.DrawImage(canvasImage);
                    }
                    else
                    {
                        float scaleX = width / canvasWidth;
                        float scaleY = height / canvasHeight;
                        Matrix3x2 matrix = Matrix3x2.CreateScale(scaleX, scaleY);

                        drawingSession.DrawImage(new Transform2DEffect
                        {
                            TransformMatrix = matrix,
                            Source = canvasImage
                        });
                    }                        
                }
            }

            return renderTarget;
        }

    }
}