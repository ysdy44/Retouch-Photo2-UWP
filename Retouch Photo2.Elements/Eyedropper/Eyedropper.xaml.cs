using Microsoft.Graphics.Canvas;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// The <see cref="Eyedropper"/> control can pick up a color from anywhere in your application.
    /// </summary>
    internal sealed partial class Eyedropper : UserControl
    {

        //@Converter
        private string ColorToStringConverter(Color color) => $"#{color.R:x2}{color.G:x2}{color.B:x2}".ToUpper();
        private double RadiusToDiameterConverter(double radius) => radius * 2;
        private CornerRadius RadiusToCornerRadiusConverter(double radius) => new CornerRadius(radius);
        private double RadiusToInnerDiameterConverter(double radius) => radius * 2 - 2;
        private CornerRadius RadiusToInnerCornerRadiusConverter(double radius) => new CornerRadius(radius - 1);
        private double FactorToSquareConverter(double factor) => factor + 2;

        //@Content
        private readonly Popup popup = new Popup();
        private readonly CanvasDevice canvasDevice = new CanvasDevice();
        private CanvasBitmap screenShot;
        private TaskCompletionSource<Color> taskSource;

        #region DependencyProperty


        /// <summary> Canvas control's width. </summary>
        public double CanvasWidth
        {
            get => this.canvasWidth;
            set
            {
                this.RootGrid.Width = this.Canvas.Width = value;

                this.canvasWidth = value;
            }
        }
        private double canvasWidth;


        /// <summary> Canvas control's Height. </summary>
        public double CanvasHeight
        {
            get => this.canvasHeight;
            set
            {
                this.RootGrid.Height = this.Canvas.Height = value;

                this.canvasHeight = value;
            }
        }
        private double canvasHeight;


        /// <summary> Gets or sets the postion of glass. </summary>
        public Vector2 Postion
        {
            get => (Vector2)base.GetValue(PostionProperty);
            set => base.SetValue(PostionProperty, value);
        }
        /// Using a DependencyProperty as the backing store for <see cref="Eyedropper.Postion"/>.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PostionProperty = DependencyProperty.Register(nameof(Postion), typeof(Vector2), typeof(Eyedropper), new PropertyMetadata(Vector2.Zero, (sender, e) =>
        {
            Eyedropper control = (Eyedropper)sender;

            if (e.NewValue is Vector2 value)
            {
                Canvas.SetLeft(control.GlassBorder, value.X - control.Radius);
                Canvas.SetTop(control.GlassBorder, value.Y - control.Radius);

                if (control.screenShot != null)
                {
                    Color color = control.GetPixelColor();
                    control.Color = color;
                    control.CanvasControl.Invalidate();
                }
            }
        }));


        /// <summary> Gets or sets the radius of glass. </summary>
        public double Radius
        {
            get => (double)base.GetValue(RadiusProperty);
            set => base.SetValue(RadiusProperty, value);
        }
        /// Using a DependencyProperty as the backing store for <see cref="Eyedropper.Radius"/>.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Eyedropper), new PropertyMetadata(50.0d));


        /// <summary> Gets or sets the factor of glass. </summary>
        public double Factor
        {
            get => (double)base.GetValue(FactorProperty);
            set => base.SetValue(FactorProperty, value);
        }
        /// Using a DependencyProperty as the backing store for <see cref="Eyedropper.Factor"/>.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FactorProperty = DependencyProperty.Register(nameof(Factor), typeof(double), typeof(Eyedropper), new PropertyMetadata(10.0d));


        /// <summary> Gets or sets the color. </summary>
        public Color Color
        {
            get => (Color)base.GetValue(ColorProperty);
            set => base.SetValue(ColorProperty, value);
        }
        /// Using a DependencyProperty as the backing store for <see cref="Eyedropper.Color"/>.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(Eyedropper), new PropertyMetadata(Colors.Transparent));


        #endregion

        //@Construct
        /// <summary>
        /// Construct a StrawPicker.
        /// </summary>
        /// <param name="hide"> Occur when popup hiding. </param>
        public Eyedropper()
        {
            this.InitializeComponent();
            this.popup.Child = this;

            this.CanvasControl.Draw += (sender, args) =>
            {
                if (this.popup.IsOpen == false) return;
                if (this.screenShot == null) return;

                this.DrawGlass(sender, args.DrawingSession);
            };

            this.RootGrid.Tapped += (s, e) =>
            {
                if (this.popup.IsOpen == false) return;
                if (this.screenShot == null) return;

                this.Close();
            };
            this.RootGrid.PointerMoved += (s, e) =>
            {
                if (this.popup.IsOpen == false) return;
                if (this.screenShot == null) return;

                this.Updata(e);
            };
        }

        /// <summary>
        /// Performs the task defined by the application associated with releasing or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.taskSource?.TrySetCanceled();
            this.taskSource = null;

            this.screenShot?.Dispose();
            this.screenShot = null;
        }

    }
}