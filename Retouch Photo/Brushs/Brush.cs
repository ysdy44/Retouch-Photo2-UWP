using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
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
        public RadialGradientManager RadialGradientManager;

        #region LinearBrush


        //Manager

        //Initialize
        public void InitializeLinearBrush(Vector2 StartPoint, Vector2 EndPoint)
        {
            if (this.Type == BrushType.None || this.Type == BrushType.Color)
            {
                this.Type = BrushType.LinearGradient;
                this.LinearGradientManager.StartPoint = StartPoint;
                this.LinearGradientManager.EndPoint = EndPoint;
            }
        }
         


        #endregion


        public CanvasRadialGradientBrush RadialBrush(ICanvasResourceCreator creator, Matrix3x2 matrix)
        {
            return new CanvasRadialGradientBrush(creator, this.Array)
            {
                Center = Vector2.Transform(this.RadialGradientManager.Center, matrix),
                RadiusX = this.RadialGradientManager.RadiusX,
                RadiusY = this.RadialGradientManager.RadiusY,
            };
        }


    }

}
