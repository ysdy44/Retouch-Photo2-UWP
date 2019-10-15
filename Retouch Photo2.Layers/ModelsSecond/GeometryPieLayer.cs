using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryPieLayer .
    /// </summary>
    public class GeometryPieLayer : IGeometryLayer
    {
        public float InnerRadius = 0.0f;
        public float SweepAngle = FanKit.Math.Pi / 2f;
        
        //@Construct
        public GeometryPieLayer()
        {
            base.Control.Icon = new GeometryPieIcon();
            base.Control.Text = "Pie";
        }
        public static float sadasdasd = FanKit.Math.Pi * 2f;

        //@Override       
        public override string Type => "Pie";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {                       
            Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
            Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;
            
            CanvasPathBuilder pathBuilder = (this.InnerRadius == 0.0f) ?              
                this._getCirle(resourceCreator) :           
                this._getPie(resourceCreator);

            CanvasGeometry pie = CanvasGeometry.CreatePath(pathBuilder);
            return pie.Transform(matrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPieLayer PieLayer = new GeometryPieLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, PieLayer, this);
            return PieLayer;
        }

        private CanvasPathBuilder _getCirle(ICanvasResourceCreator resourceCreator)
        {
            //start tooth
            Vector2 startTooth = new Vector2(1, 0);

            //end tooth
            Vector2 endTooth = new Vector2((float)System.Math.Cos(this.SweepAngle), (float)System.Math.Sin(this.SweepAngle));

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            {
                pathBuilder.BeginFigure(Vector2.Zero);

                //end notch point
                pathBuilder.AddLine(endTooth);

                CanvasArcSize canvasArcSize = (this.SweepAngle < System.Math.PI) ? CanvasArcSize.Large : CanvasArcSize.Small;
                //end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, this.SweepAngle, CanvasSweepDirection.Clockwise, canvasArcSize);
            }
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            return pathBuilder;
        }
        private CanvasPathBuilder _getPie(ICanvasResourceCreator resourceCreator)
        {
            //start tooth
            Vector2 startTooth = new Vector2(1, 0);
            //start notch
            Vector2 startNotch = startTooth * this.InnerRadius;

            //end tooth
            Vector2 endTooth = new Vector2((float)System.Math.Cos(this.SweepAngle), (float)System.Math.Sin(this.SweepAngle));
            //end notch
            Vector2 endNotch = endTooth * this.InnerRadius;

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            {
                CanvasArcSize canvasArcSize = (this.SweepAngle < System.Math.PI) ? CanvasArcSize.Large : CanvasArcSize.Small;

                //start tooth point
                pathBuilder.BeginFigure(startNotch);
                //start notch point
                pathBuilder.AddArc(endNotch, this.InnerRadius, this.InnerRadius, this.SweepAngle, CanvasSweepDirection.CounterClockwise, canvasArcSize);

                //end notch point
                pathBuilder.AddLine(endTooth);
                //end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, this.SweepAngle, CanvasSweepDirection.Clockwise, canvasArcSize);
            }
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            return pathBuilder;
        }
    }
}