using FanKit.Transformers;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary> 
    /// <see cref="Brush">'s points. 
    /// </summary>
    public struct BrushPoints
    {
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
    }
}