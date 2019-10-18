using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    internal enum PieType
    {
        Cirle,
        Donut,//圆环之理
        Pie,
        DonutAndPie,
    }

    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryPieLayer .
    /// </summary>
    public partial class GeometryPieLayer : IGeometryLayer
    {
        private PieType GetPieType(bool isZeroInnerRadius, bool isZeroSweepAngle)
        {
            if (isZeroInnerRadius && isZeroSweepAngle)
                return PieType.Cirle;
            else if (isZeroInnerRadius == false && isZeroSweepAngle)
                return PieType.Donut;
            else if (isZeroInnerRadius && isZeroSweepAngle == false)
                return PieType.Pie;
            else
                return PieType.DonutAndPie;
        }


        private CanvasGeometry _getDonut(ICanvasResourceCreator resourceCreator, float innerRadius, Matrix3x2 canvasToVirtualMatrix)
        {
            //Outter
            Transformer transformer = base.TransformManager.Destination;
            CanvasGeometry outter = transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
            //Inner
            Vector2 center = transformer.Center;
            Matrix3x2 innerMatrix = Matrix3x2.CreateTranslation(-center) * Matrix3x2.CreateScale(innerRadius) * Matrix3x2.CreateTranslation(center);

            return outter.CombineWith(outter, innerMatrix, CanvasGeometryCombine.Exclude);
        }

        private CanvasPathBuilder _getPie(ICanvasResourceCreator resourceCreator, float sweepAngle)
        {
            //start tooth
            Vector2 startTooth = new Vector2(1, 0);

            //end tooth
            Vector2 endTooth = new Vector2((float)System.Math.Cos(sweepAngle), (float)System.Math.Sin(sweepAngle));

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            {
                pathBuilder.BeginFigure(Vector2.Zero);

                //end notch point
                pathBuilder.AddLine(endTooth);

                CanvasArcSize canvasArcSize = (sweepAngle < System.Math.PI) ? CanvasArcSize.Large : CanvasArcSize.Small;
                //end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, sweepAngle, CanvasSweepDirection.Clockwise, canvasArcSize);
            }
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            return pathBuilder;
        }

        private CanvasPathBuilder _getDonutAndPie(ICanvasResourceCreator resourceCreator, float innerRadius, float sweepAngle)
        {
            //start tooth
            Vector2 startTooth = new Vector2(1, 0);
            //start notch
            Vector2 startNotch = startTooth * innerRadius;

            //end tooth
            Vector2 endTooth = new Vector2((float)System.Math.Cos(sweepAngle), (float)System.Math.Sin(sweepAngle));
            //end notch
            Vector2 endNotch = endTooth * innerRadius;

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            {
                CanvasArcSize canvasArcSize = (sweepAngle < System.Math.PI) ? CanvasArcSize.Large : CanvasArcSize.Small;

                //start tooth point
                pathBuilder.BeginFigure(startNotch);
                //start notch point
                pathBuilder.AddArc(endNotch, innerRadius, innerRadius, sweepAngle, CanvasSweepDirection.CounterClockwise, canvasArcSize);

                //end notch point
                pathBuilder.AddLine(endTooth);
                //end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, sweepAngle, CanvasSweepDirection.Clockwise, canvasArcSize);
            }
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            return pathBuilder;
        }
    }
}