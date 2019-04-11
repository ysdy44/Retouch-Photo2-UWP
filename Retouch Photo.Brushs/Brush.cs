using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo.Brushs.EllipticalGradient;
using Retouch_Photo.Brushs.LinearGradient;
using Retouch_Photo.Brushs.RadialGradient;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.Brushs
{
    public class Brush
    {
        public BrushType Type = BrushType.Color;
        public Color Color = Colors.Gray;
        public CanvasGradientStop[] Array = new CanvasGradientStop[]
        {
             new CanvasGradientStop{Color= Colors.White, Position=0.0f },
             new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };

        public CanvasImageBrush ImageBrush;
        public LinearGradientManager LinearGradientManager = new LinearGradientManager();
        public RadialGradientManager RadialGradientManager = new RadialGradientManager();
        public EllipticalGradientManager EllipticalGradientManager = new EllipticalGradientManager();
        
    }
}
