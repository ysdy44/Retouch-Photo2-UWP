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
        
         
        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            Rect rect = new Rect(0, 0, this.Transformer.Width, this.Transformer.Height);

            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                if (this.IsFill) ds.FillRectangle(rect, this.FillBrush);
                if (this.IsStroke) ds.DrawRectangle(rect, this.StrokeBrush, this.StrokeWidth);
            }

            return new Transform2DEffect
            {
                Source = command,
                TransformMatrix = this.Transformer.Matrix* canvasToVirtualMatrix
            };
        }
         


        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator,Rect rect, Color color)
        {
            return new RectangularLayer
            {
                Transformer = Transformer.CreateFromRect(rect),
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }

    

    }
}
