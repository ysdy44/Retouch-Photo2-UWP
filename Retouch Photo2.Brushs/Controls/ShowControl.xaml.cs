using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs.Controls
{
    /// <summary>
    /// A control used to show a brush.
    /// </summary>
    public sealed partial class ShowControl : UserControl
    {
        //Size
        float SizeWidth;
        float SizeHeight;
        Vector2 Center;

        //Background
        CanvasRenderTarget GrayAndWhiteBackground;
        
             
        #region DependencyProperty
         

        /// <summary> Sets or Gets brush type. </summary>
        public BrushType BrushType
        {
            get => this.brushType;
            set
            {
                this.brushType = value;
                this.CanvasControl.Invalidate();//Invalidate
            }
        }
        private BrushType brushType;
                

        /// <summary> Sets or Gets brush color. </summary>
        public Color Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.CanvasControl.Invalidate();//Invalidate
            }
        }
        private Color color;
        

        /// <summary> Sets or Gets gradient colors. </summary>
        public CanvasGradientStop[] BrushArray
        {
            get => this.brushArray;
            set
            {
                this.brushArray = value;
                this.CanvasControl.Invalidate();//Invalidate
            }
        }
        private CanvasGradientStop[] brushArray;
              

        #endregion


        //@Construct
        public ShowControl()
        {
            this.InitializeComponent();

            //Canvas
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SizeWidth = (float)e.NewSize.Width;
                this.SizeHeight = (float)e.NewSize.Height;
                this.Center = new Vector2(this.SizeWidth / 2, this.SizeHeight / 2);
            };
            this.CanvasControl.CreateResources += (sender, args) => this.GrayAndWhiteBackground = ShowControl.CreateGrayAndWhiteBackground(sender, (float)sender.ActualWidth, (float)sender.ActualHeight);
            this.CanvasControl.Draw += (s, args) =>
            {
                switch (this.brushType)
                {
                    case BrushType.None:
                        {
                            args.DrawingSession.Clear(Colors.White);//ClearColor
                            args.DrawingSession.DrawLine(0, 0, this.SizeWidth, this.SizeHeight, Colors.DodgerBlue);
                            args.DrawingSession.DrawLine(0, this.SizeHeight, this.SizeWidth, 0, Colors.DodgerBlue);
                        }
                        break;
                    case BrushType.Color:
                        {                            
                            args.DrawingSession.DrawImage(this.GrayAndWhiteBackground);//Background
                            args.DrawingSession.Clear(this.color);//ClearColor
                        }
                        break;
                    case BrushType.LinearGradient:
                        {
                            args.DrawingSession.DrawImage(this.GrayAndWhiteBackground);//Background
                            args.DrawingSession.FillRectangle(0, 0, this.SizeWidth, this.SizeHeight, new CanvasLinearGradientBrush(this.CanvasControl, this.brushArray)
                            {
                                StartPoint = new Vector2(0, this.Center.Y),
                                EndPoint = new Vector2(this.SizeWidth, this.Center.Y),
                            });
                        }
                        break;
                    case BrushType.RadialGradient:
                        {
                            args.DrawingSession.DrawImage(this.GrayAndWhiteBackground);//Background
                            args.DrawingSession.FillRectangle(0, 0, this.SizeWidth, this.SizeHeight, new CanvasRadialGradientBrush(this.CanvasControl, this.brushArray)
                            {
                                Center = this.Center,
                                RadiusX = this.Center.Y,
                                RadiusY = this.Center.Y,
                            });
                        }
                        break;
                    case BrushType.EllipticalGradient:
                        {
                            args.DrawingSession.DrawImage(this.GrayAndWhiteBackground);//Background
                            args.DrawingSession.FillRectangle(0, 0, this.SizeWidth, this.SizeHeight, new CanvasRadialGradientBrush(this.CanvasControl, this.brushArray)
                            {
                                Center = this.Center,
                                RadiusX = this.Center.X,
                                RadiusY = this.Center.Y,
                            });
                        }
                        break;
                    case BrushType.Image:
                        break;
                }

            };
        }        


        //@Static
        /// <summary>
        /// Create a gray-and-white bitmap.
        /// </summary>
        /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="width"> The bitmap width. </param>
        /// <param name="height"> The bitmap height. </param>
        /// <returns> CanvasRenderTarget </returns>
        public static CanvasRenderTarget CreateGrayAndWhiteBackground(ICanvasResourceCreatorWithDpi resourceCreator, float width, float height)
        {
            CanvasRenderTarget background = new CanvasRenderTarget(resourceCreator, width, height);

            Color[] colors = new Color[]
            {
                  Windows.UI.Colors.LightGray,
                  Windows.UI.Colors.White,
                  Windows.UI.Colors.White,
                  Windows.UI.Colors.LightGray
            };

            CanvasBitmap bitmap = CanvasBitmap.CreateFromColors(resourceCreator, colors, 2, 2);

            using (CanvasDrawingSession ds = background.CreateDrawingSession())
            {
                ds.DrawImage(new DpiCompensationEffect
                {
                    Source = new ScaleEffect
                    {
                        Scale = new Vector2(height / 4),
                        InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                        Source = new BorderEffect
                        {
                            ExtendX = CanvasEdgeBehavior.Wrap,
                            ExtendY = CanvasEdgeBehavior.Wrap,
                            Source = bitmap
                        }
                    }
                });
            }

            return background;
        }
    }
}