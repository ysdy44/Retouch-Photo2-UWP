using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;

namespace Retouch_Photo2.Brushs.Models
{
    /// <summary>
    /// <see cref="IBrush"/>'s NoneBrush.
    /// </summary>
    public class NoneBrush : IBrush
    {
        //@Content
        public BrushType Type => BrushType.None;

        public CanvasGradientStop[] Array { get => null; set { } }
        public Color Color { get; set; }
        public Transformer Destination { set { } }
        public Photocopier Photocopier { get => new Photocopier(); }
        public CanvasEdgeBehavior Extend { get => CanvasEdgeBehavior.Clamp; set { } }


        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator) => null;
        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => null;


        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix) => BrushOperateMode.InitializeController;
        public void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point) { }
        public void InitializeController(Vector2 startingPoint, Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor) { }


        public IBrush Clone()
        {
            return new NoneBrush();
        }

        public void SaveWith(XElement element) { }
        public void Load(XElement element) { }


        //@Interface
        public void CacheTransform() { }
        public void TransformMultiplies(Matrix3x2 matrix) { }
        public void TransformAdd(Vector2 vector) { }

    }
}