using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
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
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        bool _isSingleStarted;
        Vector2 _singleStartingPoint;
        InputDevice _inputDevice = InputDevice.None;


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
            ICanvasImage previousImage = new ColorSourceEffect { Color = Colors.White };

            for (int i = this.ViewModel.LayerageCollection.RootLayerages.Count - 1; i >= 0; i--)
            {
                Layerage currentLayer = this.ViewModel.LayerageCollection.RootLayerages[i];
                ILayer currentLayer2 = currentLayer.Self;

                previousImage = LayerBase.Render(this.ViewModel.CanvasDevice, currentLayer2, previousImage, currentLayer.Children);
            }

            return previousImage;
        }


        private void _drawRenderAndCrad(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 canvasToVirtualMatrix = this.ViewModel.CanvasTransformer.GetMatrix(MatrixTransformerMode.CanvasToVirtual);
            ICanvasImage previousImage = new Transform2DEffect
            {
                TransformMatrix = canvasToVirtualMatrix,
                Source = this.Render()
            };

            //Card
            drawingSession.DrawCard(previousImage, this.ViewModel.CanvasTransformer, this.ShadowColor);
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
        /// Render thumbnail image.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="width"> The thumbnail width.</param>
        /// <param name="height"> The thumbnail height.</param>
        /// <returns> The thumbnail image. </returns>
        public CanvasRenderTarget RenderThumbnail(ICanvasResourceCreator resourceCreator, int width = 256, int height = 256)
        {
            float scale = 1;
            int thumbnailWidth = 256;
            int thumbnailHeight = 256;
            if (width > height)
            {
                scale = 256.0f / width;
                thumbnailHeight = (int)(scale * height);
            }
            else
            {
                scale = 256.0f / height;
                thumbnailWidth = (int)(scale * width);
            }

            //Thumbnail
            CanvasRenderTarget thumbnail = new CanvasRenderTarget(resourceCreator, thumbnailWidth, thumbnailHeight, 96);
            {
                Matrix3x2 matrix = Matrix3x2.CreateScale(scale);
                ICanvasImage previousImage = new Transform2DEffect
                {
                    TransformMatrix = matrix,
                    Source = this.Render()
                };

                using (CanvasDrawingSession drawingSession = thumbnail.CreateDrawingSession())
                {
                    drawingSession.DrawImage(previousImage);
                }
            }
            return thumbnail;
        }
        
    }
}