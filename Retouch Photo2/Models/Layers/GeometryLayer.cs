using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo2.Models.Layers
{
    public abstract class GeometryLayer : Layer
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //@override
        protected abstract CanvasGeometry GetGeometry(ICanvasResourceCreator creator,  Matrix3x2 canvasToVirtualMatrix);

        //Fill
        public Brush FillBrush = new Brush();
        //Stroke
        public float StrokeWidth = 1.0f;
        public Brush StrokeBrush;
        public CanvasStrokeStyle StrokeStyle;


        public override void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Matrix3x2 matrix) => ds.DrawGeometry(this.GetGeometry(creator, matrix), Windows.UI.Colors.DodgerBlue);
        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.GetGeometry(creator, canvasToVirtualMatrix);

                this.FillBrush.DrawGeometry(this.ViewModel.CanvasDevice, ds, geometry, canvasToVirtualMatrix);
            }
            return command;
        }

        public override void ColorChanged(Color color, bool fillOrStroke)
        {
            this.FillBrush.Color = color;
            if (this.FillBrush.Type != BrushType.Color)
            {
                this.FillBrush.Type = BrushType.Color;
            }
        }
    }
}
