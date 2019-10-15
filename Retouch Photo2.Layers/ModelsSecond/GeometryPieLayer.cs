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
        public float InnerRadius = 0.5f;
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


            //start tooth
            Vector2 startTooth = new Vector2(1, 0);
            //start notch
            Vector2 startNotch = startTooth * this.InnerRadius;

            //end tooth
            Vector2 endTooth = new Vector2((float)System.Math.Cos(this.SweepAngle), (float)System.Math.Sin(this.SweepAngle));
            //end notch
            Vector2 endNotch = endTooth * this.InnerRadius;


            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            if (this.SweepAngle < System.Math.PI)
            {
                //start tooth point
                pathBuilder.BeginFigure(startNotch);
                //start notch point
                pathBuilder.AddArc(endNotch, this.InnerRadius, this.InnerRadius, this.SweepAngle, CanvasSweepDirection.CounterClockwise, CanvasArcSize.Large);

                //end notch point
                pathBuilder.AddLine(endTooth);
                //end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, this.SweepAngle, CanvasSweepDirection.Clockwise, CanvasArcSize.Large);
            }
            else
            {
                //start tooth point
                pathBuilder.BeginFigure(startNotch);
                //start notch point
                pathBuilder.AddArc(endNotch, this.InnerRadius, this.InnerRadius, this.SweepAngle, CanvasSweepDirection.CounterClockwise, CanvasArcSize.Small);

                //end notch point
                pathBuilder.AddLine(endTooth);
                //end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, this.SweepAngle, CanvasSweepDirection.Clockwise, CanvasArcSize.Small);
            }
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);


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
    }
}