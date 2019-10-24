using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    internal static class _heartUtil
    {
        public static Vector2 _topSpread(float spread)
        {
            //Rang
            //   x: 0~1
            //   y: 1.0~-0.8
            //  y=1-1.8x
            float topSpread = 1f - spread * 1.8f;
            return new Vector2(0, topSpread);
        }

        public static Vector2 _bottom = new Vector2(0, 1);

        public static Vector2 _leftBottom = new Vector2(-0.84f, 0.178f);
        public static Vector2 _leftBottom1 = _leftBottom + new Vector2(-0.2f, -0.2f);

        public static Vector2 _leftTop = new Vector2(-0.84f, -0.6f);
        public static Vector2 _leftTop1 = _leftTop + new Vector2(0.2f, -0.2f);
        public static Vector2 _leftTop2 = _leftTop + new Vector2(-0.2f, 0.2f);

        public static Vector2 _top1 = new Vector2(0.2f, -0.8f);
        public static Vector2 _top2 = new Vector2(-0.2f, -0.8f);

        public static Vector2 _rightTop = new Vector2(0.84f, -0.6f);
        public static Vector2 _rightTop1 = _rightTop + new Vector2(0.2f, 0.2f);
        public static Vector2 _rightTop2 = _rightTop + new Vector2(-0.2f, -0.2f);

        public static Vector2 _rightBottom = new Vector2(0.84f, 0.178f);
        public static Vector2 _rightBottom2 = _rightBottom + new Vector2(0.2f, -0.2f);
    }
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryHeartLayer";
         
        //@Content
        public float Spread = 0.8f;

        //@Construct  
        /// <summary>
        /// Construct a heart-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryHeartLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a heart-layer.
        /// </summary>
        public GeometryHeartLayer()
        {
            base.Type = GeometryHeartLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryHeartIcon(),
                Text = "Heart",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
            Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;
            
            //Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(_heartUtil._bottom);
            {
                pathBuilder.AddLine(_heartUtil._leftBottom);

                pathBuilder.AddCubicBezier(_heartUtil._leftBottom1, _heartUtil._leftTop2, _heartUtil._leftTop);

                pathBuilder.AddCubicBezier(_heartUtil._leftTop1, _heartUtil._top2, _heartUtil._topSpread(this.Spread));
                pathBuilder.AddCubicBezier(_heartUtil._top1, _heartUtil._rightTop2, _heartUtil._rightTop);

                pathBuilder.AddCubicBezier(_heartUtil._rightTop1, _heartUtil._rightBottom2, _heartUtil._rightBottom);
            }

            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder).Transform(matrix);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryHeartLayer HeartLayer = new GeometryHeartLayer();

            LayerBase.CopyWith(resourceCreator, HeartLayer, this);
            return HeartLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GeometryHeartLayer");
            
            element.Add(new XElement("Spread", this.Spread));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            this.Spread = (float)element.Element("Spread");
            LayerBase.LoadWith(element, this);
        }

    }
}