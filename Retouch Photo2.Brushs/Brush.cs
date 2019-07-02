using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
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

        /// <summary> <see cref="Brush">'s IsFollowTransform. </summary>
        public bool IsFollowTransform = true;

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


        /// <summary> <see cref="Brush">'s linear gradient start point. </summary>
        public Vector2 LinearGradientStartPoint;
        /// <summary> <see cref="Brush">'s linear gradient end point. </summary>
        public Vector2 LinearGradientEndPoint;

        /// <summary> <see cref="Brush">'s radial gradient center point. </summary>
        public Vector2 RadialGradientCenter;
        /// <summary> <see cref="Brush">'s radial gradient control point. </summary>
        public Vector2 RadialGradientPoint;

        /// <summary> <see cref="Brush">'s elliptical gradient center point. </summary>
        public Vector2 EllipticalGradientCenter;
        /// <summary> <see cref="Brush">'s elliptical gradient x-point. </summary>
        public Vector2 EllipticalGradientXPoint;
        /// <summary> <see cref="Brush">'s elliptical gradient y-point. </summary>
        public Vector2 EllipticalGradientYPoint;


        /// <summary> <see cref="Brush">'s CanvasImageBrush. </summary>
        public CanvasImageBrush ImageBrush;
    }
}