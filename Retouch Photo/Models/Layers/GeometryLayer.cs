using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers
{
    public abstract class GeometryLayer : Layer
    {
        public bool IsFill = true;
        public ICanvasBrush FillBrush;
        
        public bool IsStroke = false;
        public float StrokeWidth = 1.0f;
        public ICanvasBrush StrokeBrush;
        public CanvasStrokeStyle StrokeStyle;

    }
}
