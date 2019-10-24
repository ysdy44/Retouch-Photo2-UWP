using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCapsuleLayer .
    /// </summary>
    public class GeometryCapsuleLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryCapsuleLayer";

        //@Construct   
        /// <summary>
        /// Construct a capsule-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryCapsuleLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a capsule-layer.
        /// </summary>
        public GeometryCapsuleLayer()
        {
            base.Type = GeometryCapsuleLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCapsuleIcon(),
                Text = "Capsule",
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;


            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, canvasToVirtualMatrix);

            Vector2 center = Vector2.Transform(transformer.Center, canvasToVirtualMatrix);
            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, canvasToVirtualMatrix);
            Vector2 centerTop = Vector2.Transform(transformer.CenterTop, canvasToVirtualMatrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, canvasToVirtualMatrix);
            Vector2 centerBottom = Vector2.Transform(transformer.CenterBottom, canvasToVirtualMatrix);


            //HV
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;

            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();
            Vector2 verticalUnit = vertical / verticalLength;

            if (horizontalLength<verticalLength)
            {
                return transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
            }
            else
            {
                //HV
                Vector2 horizontal2 = 0.5f * verticalLength * horizontalUnit;
                Vector2 horizontal448 = horizontal2 * 0.448f;// vector / (1 - 0.552f)

                Vector2 vertical276 = (centerBottom - centerTop) * 0.276f;// vector / 2 * 0.552f
                

                //Control
                Vector2 left2 = centerLeft - vertical276;
                Vector2 leftTop_Top = leftTop + horizontal2;
                Vector2 leftTop_Top1 = leftTop + horizontal448;

                Vector2 rightTop_Top = rightTop - horizontal2;
                Vector2 rightTop_Top2 = rightTop - horizontal448;
                Vector2 right1 = centerRight - vertical276;

                Vector2 right2 = centerRight + vertical276;
                Vector2 rightBottom_Bottom = rightBottom - horizontal2;
                Vector2 rightBottom_Bottom1 = rightBottom - horizontal448;

                Vector2 leftBottom_Bottom = leftBottom + horizontal2;
                Vector2 leftBottom_Bottom2 = leftBottom + horizontal448;
                Vector2 left1 = centerLeft + vertical276;

                
                //Path
                CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
                pathBuilder.BeginFigure(centerLeft);

                pathBuilder.AddCubicBezier(left2, leftTop_Top1, leftTop_Top);
                pathBuilder.AddLine(rightTop_Top);

                pathBuilder.AddCubicBezier(rightTop_Top2, right1, centerRight);

                pathBuilder.AddCubicBezier(right2, rightBottom_Bottom1, rightBottom_Bottom);
                pathBuilder.AddLine(leftBottom_Bottom);

                pathBuilder.AddCubicBezier(leftBottom_Bottom2, left1, centerLeft);

                pathBuilder.EndFigure(CanvasFigureLoop.Closed);

                //Geometry
                return CanvasGeometry.CreatePath(pathBuilder);
            }
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCapsuleLayer CapsuleLayer = new GeometryCapsuleLayer();

            LayerBase.CopyWith(resourceCreator, CapsuleLayer, this);
            return CapsuleLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GeometryCapsuleLayer");
            
            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            LayerBase.LoadWith(element, this);
        }

    }
}