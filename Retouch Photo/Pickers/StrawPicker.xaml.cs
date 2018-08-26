using Microsoft.Graphics.Canvas;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Retouch_Photo.Pickers
{
    public sealed partial class StrawPicker : UserControl
    {
        //Delegate
        public delegate void ColorChangeHandler(object sender, Color value);
        public event ColorChangeHandler ColorChangeStarted = null;
        public event ColorChangeHandler ColorChangeDelta = null;
        public event ColorChangeHandler ColorChangeCompleted = null;


        private Popup popup = new Popup();
        private CanvasDevice device = new CanvasDevice();
        private CanvasBitmap bitmap;


        #region DependencyProperty


        public Color NewColor
        {
            get { return (Color)GetValue(NewColorProperty); }
            set { SetValue(NewColorProperty, value); }
        }
        public static readonly DependencyProperty NewColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(StrawPicker), new PropertyMetadata(Windows.UI.Colors.White, (sender, e) =>
        {
            StrawPicker con = (StrawPicker)sender;

            if (e.NewValue is Color value)
            {
                con.ColorChangeDelta?.Invoke(con, value);
                con.SolidColorBrushName.Color = value;
            }
        }));


        public Color OldColor
        {
            get { return (Color)GetValue(OldColorProperty); }
            set { SetValue(OldColorProperty, value); }
        }
        public static readonly DependencyProperty OldColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(StrawPicker), new PropertyMetadata(Windows.UI.Colors.White, (sender, e) =>
        {
            StrawPicker con = (StrawPicker)sender;

            if (e.NewValue is Color value)
            {
                con.ColorChangeCompleted?.Invoke(con, value);
                con.SolidColorBrushNameOld.Color = value;
            }
        }));


        #endregion


        public StrawPicker()
        {
            this.InitializeComponent();

            //Popup
            UIElement element = this._popup.Child;
            this._popup.Child = null;
            popup.Child = element;
        }



        Vector2 v;
        private void Border_PointerPressed(object sender, PointerRoutedEventArgs e) => v = e.GetCurrentPoint(Window.Current.Content).Position.ToVector2();
        private async void Border_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.ColorChangeStarted?.Invoke(this, this.OldColor);

            this.bitmap = await this.GetRenderTargetBitmap(Window.Current.Content);
            this.NewColor = this.GetColor(this.bitmap, this.v);

            //Popup
            this.popup.HorizontalOffset = v.X - 50;
            this.popup.VerticalOffset = v.Y - 50;
            this.popup.IsOpen = true;
        }
        private void Border_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            v += e.Delta.Translation.ToVector2();
            this.NewColor = this.GetColor(this.bitmap, this.v);

            //Popup
            this.popup.HorizontalOffset = v.X - 50;
            this.popup.VerticalOffset = v.Y - 50;
        }
        private void Border_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.OldColor = this.GetColor(this.bitmap, this.v);
            this.bitmap.Dispose();
            this.bitmap = null;

            //Popup         
            this.popup.IsOpen = false;
        }



        private async Task<CanvasBitmap> GetRenderTargetBitmap(UIElement element)
        {
            RenderTargetBitmap render = new RenderTargetBitmap();
            await render.RenderAsync(element);
            return CanvasBitmap.CreateFromBytes(this.device, await render.GetPixelsAsync(), render.PixelWidth, render.PixelHeight, DirectXPixelFormat.B8G8R8A8UIntNormalized);
        }
        private Color GetColor(CanvasBitmap bitmap, Vector2 v)
        {
            if (bitmap != null)
            {
                int left = GetLeft((int)bitmap.SizeInPixels.Width, v.X, Window.Current.Bounds.Width);
                int top = GetLeft((int)bitmap.SizeInPixels.Height, v.Y, Window.Current.Bounds.Height);

                return bitmap.GetPixelColors(left, top, 1, 1).Single();
            }
            else return Windows.UI.Colors.White;
        }
        private int GetLeft(int bitmapWidth, float x, double windowWidth)
        {
            int left = (int)(bitmapWidth * x / windowWidth);

            if (left < 0) return 0;
            else if (left >= bitmapWidth) return bitmapWidth - 1;
            return left;
        }


    }
}
