using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Control of <see cref="Brush">Show.
    /// </summary>
    public sealed partial class BrushShowControl : UserControl
    {
        Vector2 CanvasCenter;

        float CanvasWidth;
        float CanvasHeight;
        CanvasBitmap Bitmap;


        #region DependencyProperty


        /// <summary> Sets or Gets brush type. </summary>
        public BrushType Type
        {
            get { return (BrushType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary> Identifies the <see cref = "BrushShowControl.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(BrushType), typeof(BrushShowControl), new PropertyMetadata(BrushType.None, (sender, e) =>
        {
            BrushShowControl con = (BrushShowControl)sender;

            con.CanvasControl.Invalidate();
        }));


        /// <summary> Sets or Gets brush color. </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BrushShowControl.Color" /> dependency property. </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(BrushShowControl), new PropertyMetadata(Colors.Transparent, (sender, e) =>
        {
            BrushShowControl con = (BrushShowControl)sender;

            con.CanvasControl.Invalidate();
        }));


        /// <summary> Sets or Gets gradient colors. </summary>
        public CanvasGradientStop[] Array
        {
            get { return (CanvasGradientStop[])GetValue(ArrayProperty); }
            set { SetValue(ArrayProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BrushShowControl.Array" /> dependency property. </summary>
        public static readonly DependencyProperty ArrayProperty = DependencyProperty.Register(nameof(Array), typeof(CanvasGradientStop[]), typeof(BrushShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            BrushShowControl con = (BrushShowControl)sender;

            con.CanvasControl.Invalidate();
        }));


        #endregion


        //@Construct
        public BrushShowControl()
        {
            this.InitializeComponent();

            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.CanvasWidth = (float)e.NewSize.Width;
                this.CanvasHeight = (float)e.NewSize.Height;
                this.CanvasCenter = new Vector2(this.CanvasWidth / 2, this.CanvasHeight / 2);
            };
            this.CanvasControl.CreateResources += (sender, args) =>
            {
                Color[] colors = new Color[]
                {
                     Windows.UI.Colors.LightGray, Windows.UI.Colors.White,
                     Windows.UI.Colors.White, Windows.UI.Colors.LightGray
                };
                this.Bitmap = CanvasBitmap.CreateFromColors(sender, colors, 2, 2);
            };
            this.CanvasControl.Draw += (s, args) =>
            {
                switch (this.Type)
                {
                    case BrushType.None:
                        this.DrawNone(args.DrawingSession, this.CanvasWidth, this.CanvasHeight);
                        break;

                    case BrushType.Color:
                        this.DrawColor(args.DrawingSession, this.Color);
                        break;

                    case BrushType.LinearGradient:
                        this.DrawLinearGradient(args.DrawingSession, this.CanvasControl, this.Array, this.CanvasWidth, this.CanvasHeight, this.CanvasCenter.Y);
                        break;

                    case BrushType.RadialGradient:
                        this.DrawRadialGradient(args.DrawingSession, this.CanvasControl, this.Array, this.CanvasWidth, this.CanvasHeight, this.CanvasCenter);
                        break;

                    case BrushType.EllipticalGradient:
                        this.DrawEllipticalGradient(args.DrawingSession, this.CanvasControl, this.Array, this.CanvasWidth, this.CanvasHeight, this.CanvasCenter);
                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }

            };
        }


        private void DrawGrayAndWhite(CanvasDrawingSession ds)
        {
            ds.DrawImage(new DpiCompensationEffect
            {
                Source = new ScaleEffect
                {
                    Scale = new Vector2(this.CanvasHeight / 4),
                    InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                    Source = new BorderEffect
                    {
                        ExtendX = CanvasEdgeBehavior.Wrap,
                        ExtendY = CanvasEdgeBehavior.Wrap,
                        Source = this.Bitmap
                    }
                }
            });
        }

        private void DrawNone(CanvasDrawingSession ds, float width, float height)
        {
            ds.Clear(Colors.White);
            ds.DrawLine(0, 0, width, height, Colors.DodgerBlue);
            ds.DrawLine(0, height, width, 0, Colors.DodgerBlue);
        }

        private void DrawColor(CanvasDrawingSession ds, Color color)
        {
            if (color.A < 200) this.DrawGrayAndWhite(ds);

            ds.Clear(color);
        }

        private void DrawLinearGradient(CanvasDrawingSession ds, ICanvasResourceCreator creator, CanvasGradientStop[] stops, float width, float height, float centerY)
        {
            this.DrawGrayAndWhite(ds);

            ds.FillRectangle(0, 0, width, height, new CanvasLinearGradientBrush(creator, stops)
            {
                StartPoint = new Vector2(0, centerY),
                EndPoint = new Vector2(width, centerY)
            });
        }

        private void DrawRadialGradient(CanvasDrawingSession ds, ICanvasResourceCreator creator, CanvasGradientStop[] stops, float width, float height, Vector2 center)
        {
            this.DrawGrayAndWhite(ds);

            float radius = Math.Min(width, height) / 2;
            ds.FillRectangle(0, 0, width, height, new CanvasRadialGradientBrush(creator, stops)
            {
                Center = center,
                RadiusX = radius,
                RadiusY = radius
            });
        }

        private void DrawEllipticalGradient(CanvasDrawingSession ds, ICanvasResourceCreator creator, CanvasGradientStop[] stops, float width, float height, Vector2 center)
        {
            this.DrawGrayAndWhite(ds);

            ds.FillRectangle(0, 0, width, height, new CanvasRadialGradientBrush(creator, stops)
            {
                Center = center,
                RadiusX = width / 2,
                RadiusY = height / 2
            });
        }

    }
}