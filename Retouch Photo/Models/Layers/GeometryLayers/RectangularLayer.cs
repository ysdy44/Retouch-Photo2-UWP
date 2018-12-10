using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer:GeometryLayer
    {
        public static string ID = "RectangularLayer";

        public Rect Rect;

        public override ICanvasImage GetRender(ICanvasResourceCreator creator)
        {
            CanvasCommandList command = new CanvasCommandList(creator);

            using (CanvasDrawingSession ds= command.CreateDrawingSession())
            {
                if (this.IsFill)ds.FillRectangle(this.Rect,this.FillBrush);
                if (this.IsStroke) ds.DrawRectangle(this.Rect,this.StrokeBrush,this.StrokeWidth);
            }

            return command;
        }

        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, Rect rect, Color color)
        {
            return new RectangularLayer
            {
                Rect = rect,
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }

    }
}
