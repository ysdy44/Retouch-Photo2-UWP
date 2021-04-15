using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Retouch_Photo2.Elements
{
    internal sealed partial class Eyedropper : UserControl
    {

        /// <summary>
        /// Open the eyedropper.
        /// </summary>
        /// <returns>The picked color.</returns>
        public async Task<Color> OpenAsync()
        {
            await this.OpenCore();

            this.popup.IsOpen = true;

            Color resultcolor = await this.taskSource.Task;
            this.taskSource = null;
            return resultcolor;
        }
        /// <summary>
        /// Open the eyedropper.
        /// </summary>
        /// <param name="pointer">The initial eyedropper pointer</param>
        /// <returns>The picked color.</returns>
        public async Task<Color> OpenAsync(PointerRoutedEventArgs pointer)
        {
            await this.OpenCore();

            this.Visibility = Visibility.Collapsed;
            this.popup.IsOpen = true;
            this.Postion = pointer.GetCurrentPoint(this.RootGrid).Position.ToVector2();
            this.Visibility = Visibility.Visible;

            Color resultcolor = await this.taskSource.Task;
            this.taskSource = null;
            return resultcolor;
        }
        private async Task OpenCore()
        {
            this.taskSource = new TaskCompletionSource<Color>();

            Window.Current.CoreWindow.PointerCursor = null;
            this.CanvasWidth = Window.Current.Bounds.Width;
            this.CanvasHeight = Window.Current.Bounds.Height;

            var result = await this.RenderScreenshotAsync();
            this.screenShot = result.Item1;
            this.ImageBrush.ImageSource = result.Item2;
        }


        /// <summary>
        /// Updata the position and color.
        /// </summary>
        /// <param name="pointer">The initial eyedropper pointer</param>
        public void Updata(PointerRoutedEventArgs pointer)
        {
            this.Postion = pointer.GetCurrentPoint(this.RootGrid).Position.ToVector2();
        }


        /// <summary>
        /// Close the eyedropper.
        /// </summary>
        public void Close()
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            this.Color = this.GetPixelColor();

            this.popup.IsOpen = false;

            if (this.taskSource != null && this.taskSource.Task.IsCanceled == false)
            {
                this.taskSource.TrySetResult(this.Color);
            }
            this.Dispose();
        }



        private void DrawGlass(CanvasControl sender, CanvasDrawingSession drawingSession)
        {
            // Radius
            float radius = (float)this.Radius;

            // Factor and Pixel
            float factor = (float)this.Factor;
            float halfPixel = factor / 2;

            // Offset = - the Postion + the Radius + half of Pixel
            Vector2 offset = new Vector2(-this.Postion.X + radius + halfPixel, -this.Postion.Y + radius + halfPixel);

            // Draw: ScaleEffect + Transform2DEffect + DpiCompensationEffect
            drawingSession.DrawImage(new DpiCompensationEffect
            {
                SourceDpi = new Vector2(sender.Dpi),
                Source = new Transform2DEffect
                {
                    TransformMatrix = Matrix3x2.CreateTranslation(offset),
                    Source = new ScaleEffect
                    {
                        Scale = new Vector2(factor),
                        CenterPoint = this.Postion,
                        InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                        BorderMode = EffectBorderMode.Hard,
                        Source = this.screenShot
                    }
                }
            });
        }


        private Color GetPixelColor()
        {
            if (this.screenShot == null) return Colors.White;
            else
            {
                int left = getLeft((int)this.screenShot.SizeInPixels.Width, this.Postion.X, this.CanvasWidth);
                int top = getLeft((int)this.screenShot.SizeInPixels.Height, this.Postion.Y, this.CanvasHeight);

                return this.screenShot.GetPixelColors(left, top, 1, 1).Single();
            }

            int getLeft(int bitmapWidth, float x, double windowWidth)
            {
                int left = (int)(bitmapWidth * (x / windowWidth));

                if (left < 0) return 0;
                else if (left >= bitmapWidth) return bitmapWidth - 1;
                return left;
            }
        }


        private async Task<(CanvasBitmap, RenderTargetBitmap)> RenderScreenshotAsync()
        {
            try
            {
                RenderTargetBitmap imageSource = new RenderTargetBitmap();
                await imageSource.RenderAsync(getUIElement());

                CanvasBitmap screenshot = CanvasBitmap.CreateFromBytes(this.canvasDevice, await imageSource.GetPixelsAsync(), imageSource.PixelWidth, imageSource.PixelHeight, DirectXPixelFormat.B8G8R8A8UIntNormalized);

                return (screenshot, imageSource);
            }
            catch (OutOfMemoryException)
            {
                return default;
            }

            UIElement getUIElement()
            {
                if (Window.Current.Content is FrameworkElement frame)
                {
                    if (frame.Parent is FrameworkElement border)
                    {
                        if (border.Parent is FrameworkElement rootScrollViewer)
                            return rootScrollViewer;
                        else
                            return border;
                    }
                    else
                        return frame;
                }
                else return Window.Current.Content;
            }
        }

    }
}
