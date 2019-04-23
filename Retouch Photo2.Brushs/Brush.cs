using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs.EllipticalGradient;
using Retouch_Photo2.Brushs.LinearGradient;
using Retouch_Photo2.Brushs.RadialGradient;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    public class Brush
    {
        public BrushType Type;

        public Color Color = Colors.Gray;
        public CanvasGradientStop[] Array = new CanvasGradientStop[]
        {
             new CanvasGradientStop{Color= Colors.White, Position=0.0f },
             new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };


        public LinearGradientManager LinearGradientManager = new LinearGradientManager();
        public RadialGradientManager RadialGradientManager = new RadialGradientManager();
        public EllipticalGradientManager EllipticalGradientManager = new EllipticalGradientManager();

        public CanvasImageBrush ImageBrush;


        public void DrawGeometry(ICanvasResourceCreator creator, CanvasDrawingSession ds, CanvasGeometry geometry, Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    ds.FillGeometry(geometry, this.Color);
                    break;

                case BrushType.LinearGradient:
                    ds.FillGeometry(geometry, this.LinearGradientManager.GetBrush(creator, matrix, this.Array));
                    break;

                case BrushType.RadialGradient:
                    ds.FillGeometry(geometry, this.RadialGradientManager.GetBrush(creator, matrix, this.Array));
                    break;

                case BrushType.EllipticalGradient:
                    ds.FillGeometry(geometry, this.EllipticalGradientManager.GetBrush(creator, matrix, this.Array));
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }

        }
    }
}
