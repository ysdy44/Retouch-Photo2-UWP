using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers;
using System;
using System.Numerics;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainCanvasControl" />. 
    /// </summary>
    public partial class MainCanvasControl : UserControl
    {
        

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainCanvasControl" />'s accent color. </summary>
        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MainCanvasControl.AccentColor" /> dependency property. </summary>
        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(nameof(AccentColor), typeof(Color), typeof(MainCanvasControl), new PropertyMetadata(Colors.DodgerBlue, (sender, e) =>
        {
            MainCanvasControl con = (MainCanvasControl)sender;

            if (e.NewValue is Color value)
            {
                con.ViewModel.AccentColor = value;
                con.ViewModel.Invalidate();//Invalidate
            }
        }));


        /// <summary> Gets or sets <see cref = "MainCanvasControl" />'s shadow color. </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MainCanvasControl.ShadowColor" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(MainCanvasControl), new PropertyMetadata(Colors.Black, (sender, e) =>
        {
            MainCanvasControl con = (MainCanvasControl)sender;

            if (e.NewValue is Color value)
            {
                con.ViewModel.Invalidate();//Invalidate
            }
        }));


        #endregion


        /// <summary>
        /// Render.
        /// </summary>
        /// <returns> The render image. </returns>
        public ICanvasImage Render()
        {
            return LayerBase.Render(this.ViewModel.CanvasDevice, this.ViewModel.LayerageCollection.RootLayerages);
        }


        private void _drawRenderAndCrad(CanvasDrawingSession drawingSession)
        {
            drawingSession.DrawCard(new ColorSourceEffect { Color = Colors.White }, this.ViewModel.CanvasTransformer, this.ShadowColor);

            ICanvasImage canvasImage = this.Render();
            if (canvasImage == null) return;

            drawingSession.DrawImage(new Transform2DEffect
            {
                Source = canvasImage,
                TransformMatrix = this.ViewModel.CanvasTransformer.GetMatrix()
            });
        }

        private void _drawToolAndBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            
            //Bound
            foreach (Layerage layerage in this.ViewModel.LayerageCollection.RootLayerages)
            {
                ILayer layer = layerage.Self;

                if (layer.IsSelected == true)
                {
                    layer.DrawBound(resourceCreator, drawingSession, matrix, layerage.Children, this.ViewModel.AccentColor);
                }
            }


            //Tool
            this.TipViewModel.Tool.Draw(drawingSession);
        }


        /// <summary>
        /// Render image.
        /// </summary>
        /// <param name="fileWidth"> The file width.</param>
        /// <param name="fileHeight"> The file height.</param>
        /// <returns> The file image. </returns>
        public CanvasRenderTarget Render( int fileWidth = 256, int fileHeight = 256, bool isClearWhite = true)
        {
            ICanvasImage canvasImage = this.Render();
            
            int canvasWidth = this.ViewModel.CanvasTransformer.Width;
            int canvasHeight = this.ViewModel.CanvasTransformer.Height;
            
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(this.ViewModel.CanvasDevice, fileWidth, fileHeight, 96);
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
        /// <param name="resourceCreator"> The resource-creator. </param>
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

            CanvasRenderTarget renderTarget = new CanvasRenderTarget(this.ViewModel.CanvasDevice, fileWidth, fileHeight, dpi);
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