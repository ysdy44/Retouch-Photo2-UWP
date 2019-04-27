using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    public interface IGradientManager
    {
        ICanvasBrush GetBrush(ICanvasResourceCreator creator, Matrix3x2 matrix, CanvasGradientStop[] array);

        void Start(Vector2 point, Matrix3x2 matrix);
        void Delta(Vector2 point, Matrix3x2 inverseMatrix);
        void Complete();

        void Draw(CanvasDrawingSession ds, Matrix3x2 matrix);
    }
}
