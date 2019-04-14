using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Brushs;
using Retouch_Photo.Controls.LayerControls.GeometryControls;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

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

        protected override CanvasGeometry GetGeometry(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(this.Transformer.DstRightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(this.Transformer.DstLeftBottom, canvasToVirtualMatrix);

            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(creator, points);
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
