using Microsoft.Graphics.Canvas.Brushes;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Brush Classes.
    /// </summary>
    public class Brush
    {
        /// <summary> Gets new CanvasGradientStop array. </summary>
        public static CanvasGradientStop[] GetNewArray() => new CanvasGradientStop[]
        {
            new CanvasGradientStop{Color= Colors.White, Position=0.0f },
            new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };

        /// <summary> <see cref="Brush">'s type. </summary>
        public BrushType Type;
        
        /// <summary> <see cref="Brush">'s color. </summary>
        public Color Color = Colors.Gray;

        /// <summary> <see cref="Brush">'s gradient colors. </summary>
        public CanvasGradientStop[] Array = new CanvasGradientStop[]
        {
             new CanvasGradientStop{Color= Colors.White, Position=0.0f },
             new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };

        /// <summary> <see cref="Brush">'s points. </summary>
        public BrushPoints Points;
        /// <summary> <see cref = "Brush.Points" />'s old cache. </summary>
        public BrushPoints OldPoints;

        /// <summary> <see cref="Brush">'s CanvasImageBrush. </summary>
        public CanvasImageBrush ImageBrush;
    }
}