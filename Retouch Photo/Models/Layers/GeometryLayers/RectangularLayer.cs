using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer:GeometryLayer
    {

        public static string Type = "RectangularLayer";
        protected RectangularLayer() => base.Name = RectangularLayer.Type;
        
        public VectorRect Rect;
         
        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 matrix)
        {
            Rect r = this.Rect.Transform(matrix).ToRect();

            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                if (this.IsFill) ds.FillRectangle(r, this.FillBrush);
                if (this.IsStroke) ds.DrawRectangle(r, this.StrokeBrush, this.StrokeWidth);
            }

            return command;
        }
         
        public override VectorRect GetBoundRect(ICanvasResourceCreator creator)
        {
            return this.Rect;
        }

        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, VectorRect rect, Color color)
        {
            return new RectangularLayer
            {
                Rect = rect,
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }

    

    }
}
