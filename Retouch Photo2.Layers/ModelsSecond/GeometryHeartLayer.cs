using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : IGeometryLayer
    {
        //@Static

        static Vector2 _bottom = new Vector2(0, 1);

        static Vector2 _leftBottom = new Vector2(-0.84f, 0.178f);
        static Vector2 _leftBottom1 = _leftBottom + new Vector2(-0.2f, -0.2f);

        static Vector2 _leftTop = new Vector2(-0.84f, -0.6f);
        static Vector2 _leftTop1 = _leftTop + new Vector2(0.2f, -0.2f);
        static Vector2 _leftTop2 = _leftTop + new Vector2(-0.2f, 0.2f);

        static Vector2 _top1 = new Vector2(0.2f, -0.8f);
        static Vector2 _top2 = new Vector2(-0.2f, -0.8f);

        static Vector2 _rightTop = new Vector2(0.84f, -0.6f);
        static Vector2 _rightTop1 = _rightTop + new Vector2(0.2f, 0.2f);
        static Vector2 _rightTop2 = _rightTop + new Vector2(-0.2f, -0.2f);

        static Vector2 _rightBottom = new Vector2(0.84f, 0.178f);
        static Vector2 _rightBottom2 = _rightBottom + new Vector2(0.2f, -0.2f);


        public float Spread = 0.8f;

        //@Construct
        public GeometryHeartLayer()
        {
            base.Control.Icon = new GeometryHeartIcon();
            base.Control.Text = "Heart";
        }

        //@Override       
        public override string Type => "Heart";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
            Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;


            //Rang
            //   x: 0~1
            //   y: 1.0~-0.8
            //  y=1-1.8x
            float spread = 1f - this.Spread * 1.8f;
            Vector2 topSpread = new Vector2(0, spread);


            //Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(_bottom);
            {
                pathBuilder.AddLine(_leftBottom);

                pathBuilder.AddCubicBezier(GeometryHeartLayer._leftBottom1, GeometryHeartLayer._leftTop2, GeometryHeartLayer._leftTop);

                pathBuilder.AddCubicBezier(GeometryHeartLayer._leftTop1, GeometryHeartLayer._top2, topSpread);
                pathBuilder.AddCubicBezier(GeometryHeartLayer._top1, GeometryHeartLayer._rightTop2, GeometryHeartLayer._rightTop);

                pathBuilder.AddCubicBezier(GeometryHeartLayer._rightTop1, GeometryHeartLayer._rightBottom2, GeometryHeartLayer._rightBottom);
            }

            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder).Transform(matrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryHeartLayer HeartLayer = new GeometryHeartLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, HeartLayer, this);
            return HeartLayer;
        }
    }
}