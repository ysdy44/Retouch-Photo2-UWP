using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Models.Layers
{
    public abstract class GeometryLayer : Layer
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //@override
        protected abstract CanvasGeometry GetGeometry(Matrix3x2 canvasToVirtualMatrix);


        //Fill
        public Brush FillBrush = new Brush();
        //Stroke
        public float StrokeWidth = 1.0f;
        public Brush StrokeBrush = new Brush();
        public CanvasStrokeStyle StrokeStyle;


        public override void TransformStart()
        {
            if (this.FillBrush.IsFollowTransform == false) return;
            this.FillBrush.TransformStart();
        }
        public override void TransformDelta()
        {
            if (this.FillBrush.IsFollowTransform == false) return;

            Matrix3x2 matrix = Transformer.DivideMatrix(base.OldTransformer, base.Transformer);

            this.FillBrush.TransformDelta(matrix);
        }
        public override void TransformComplete()
        {
            this.TransformDelta();
        }


        public override void Draw(CanvasDrawingSession ds, Matrix3x2 matrix) => ds.DrawGeometry(this.GetGeometry(matrix), Windows.UI.Colors.DodgerBlue);
        protected override ICanvasImage GetRender(IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(this.ViewModel.CanvasDevice);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.GetGeometry(canvasToVirtualMatrix);

                this.FillBrush.FillGeometry(this.ViewModel.CanvasDevice, ds, geometry, canvasToVirtualMatrix);
                this.StrokeBrush.DrawGeometry(this.ViewModel.CanvasDevice, ds, geometry, canvasToVirtualMatrix,this.StrokeWidth);
            }
            return command;
        }

        public override void ColorChanged(Color color, bool fillOrStroke)
        {
            if (fillOrStroke)
            {
                this.FillBrush.Color = color;
                if (this.FillBrush.Type != BrushType.Color)
                {
                    this.FillBrush.Type = BrushType.Color;
                }
            }
            else
            {
                this.StrokeBrush.Color = color;
                if (this.StrokeBrush.Type != BrushType.Color)
                {
                    this.StrokeBrush.Type = BrushType.Color;
                }
            }
        }
    }
}
