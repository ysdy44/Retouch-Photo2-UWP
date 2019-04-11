using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Controls.LayerControls.GeometryControls;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;
using Retouch_Photo.Brushs;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer : GeometryLayer
    {

        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public static readonly string Type = "Rectangular";
        protected RectangularLayer()
        {
            base.Name = RectangularLayer.Type;
            base.Icon = new RectangularControl();
        }




        private CanvasGeometry GetGeometry(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix)
        {
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(this.Transformer.DstRightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(this.Transformer.DstLeftBottom, canvasToVirtualMatrix);

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(creator, new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            });
            return geometry;
        }

        public override void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            CanvasGeometry geometry = this.GetGeometry(creator, matrix);

            ds.DrawGeometry(geometry, Windows.UI.Colors.DodgerBlue);
        }
        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.GetGeometry(creator, canvasToVirtualMatrix);
                Matrix3x2 matrix = base.Transformer.Matrix * canvasToVirtualMatrix;

                switch (base.FillBrush.Type)
                {
                    case BrushType.None:
                        break;

                    case BrushType.Color:
                        ds.FillGeometry(geometry, base.FillBrush.Color);
                        break;

                    case BrushType.LinearGradient:                         
                        ds.FillGeometry(geometry, new CanvasLinearGradientBrush(this.ViewModel.CanvasDevice, base.FillBrush.Array)
                        {
                            StartPoint = Vector2.Transform(base.FillBrush.LinearGradientManager.StartPoint, matrix),
                            EndPoint = Vector2.Transform(base.FillBrush.LinearGradientManager.EndPoint, matrix)
                        });
                        break;

                    case BrushType.RadialGradient:
                        {
                            Vector2 center = Vector2.Transform(base.FillBrush.RadialGradientManager.Center, matrix);
                            Vector2 point = Vector2.Transform(base.FillBrush.RadialGradientManager.Point, matrix);
                            float radius = Vector2.Distance(center, point);
                            ds.FillGeometry(geometry, new CanvasRadialGradientBrush(this.ViewModel.CanvasDevice, base.FillBrush.Array)
                            {
                                RadiusX = radius,
                                RadiusY = radius,
                                Center = center
                            });
                        }
                        break;

                    case BrushType.EllipticalGradient:
                        {
                            Vector2 center = Vector2.Transform(base.FillBrush.EllipticalGradientManager.Center, matrix);
                            Vector2 xPoint = Vector2.Transform(base.FillBrush.EllipticalGradientManager.XPoint, matrix);
                            Vector2 yPoint = Vector2.Transform(base.FillBrush.EllipticalGradientManager.YPoint, matrix);
                            ds.FillGeometry(geometry, new CanvasRadialGradientBrush(this.ViewModel.CanvasDevice, base.FillBrush.Array)
                            {
                                Transform = base.FillBrush.EllipticalGradientManager.GetTransform(center, xPoint),
                                RadiusX = base.FillBrush.EllipticalGradientManager.GetRadiusX(center, xPoint),
                                RadiusY = base.FillBrush.EllipticalGradientManager.GetRadiusY(center, yPoint),
                                Center = center
                            });
                        }
                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }

                // if (this.IsStroke) ds.DrawGeometry(geometry, base.StrokeBrush, base.StrokeWidth);
            }
            return command;
        }


        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new RectangularLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y)),
                FillBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = color
                }
            };
        }

    }
}
