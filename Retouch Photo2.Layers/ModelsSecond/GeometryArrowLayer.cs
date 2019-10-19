using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    public enum GeometryArrowTailType
    {
        None,
        Arrow,
        //   Round,
    }
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s ArrowLayer .
    /// </summary>
    public class GeometryArrowLayer : IGeometryLayer, ILayer
    {
        //@Content       
        public string Type => "GeometryArrowLayer";

        public bool IsAbsolute = false;
        public float Width = 10;
        public float Value = 0.5f;

        public GeometryArrowTailType LeftTail = GeometryArrowTailType.None;
        public GeometryArrowTailType RightTail = GeometryArrowTailType.Arrow;

        //@Construct
        public GeometryArrowLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryArrowIcon(),
                Text = "Arrow",
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
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, canvasToVirtualMatrix);


            //horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            //vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();


            float width = this.IsAbsolute ? this.Width : this.Value * verticalLength;
            Vector2 widthVector = vertical * (width / verticalLength) / 2;
            Vector2 widthVectorTransform = Vector2.Transform(widthVector + base.TransformManager.Destination.Center, canvasToVirtualMatrix) - center;

            Vector2 focusVector = (verticalLength < horizontalLength) ?
              0.5f * (verticalLength / horizontalLength) * horizontal :
              0.5f * horizontal;

            Vector2[] points;


            if (this.LeftTail == GeometryArrowTailType.Arrow && this.RightTail == GeometryArrowTailType.Arrow)
            {
                Vector2 leftFocusTransform = Vector2.Transform(base.TransformManager.Destination.CenterLeft + focusVector, canvasToVirtualMatrix);
                Vector2 leftVector = leftFocusTransform - centerLeft;

                Vector2 rightFocusTransform = Vector2.Transform(base.TransformManager.Destination.CenterRight - focusVector, canvasToVirtualMatrix);
                Vector2 rightVector = rightFocusTransform - centerRight;

                points = new Vector2[10]
                {
                    centerLeft,//L

                    leftTop+leftVector,//LT
                    leftFocusTransform-widthVectorTransform,//C LT

                    rightFocusTransform-widthVectorTransform,//C RT
                    rightTop+rightVector,//RT

                    centerRight,//R

                    rightBottom+rightVector,//RB
                    rightFocusTransform+widthVectorTransform,//C RB

                    leftFocusTransform+widthVectorTransform,//C LB
                    leftBottom+leftVector,//LB
                };
            }
            else if (this.LeftTail == GeometryArrowTailType.Arrow && this.RightTail == GeometryArrowTailType.None)
            {
                Vector2 leftFocusTransform = Vector2.Transform(base.TransformManager.Destination.CenterLeft + focusVector, canvasToVirtualMatrix);
                Vector2 leftVector = leftFocusTransform - centerLeft;

                points = new Vector2[7]
                {
                    centerLeft,//L

                    leftTop+leftVector,//LT
                    leftFocusTransform-widthVectorTransform,//C LT
                    
                    centerRight-widthVectorTransform,//RT
                    centerRight+widthVectorTransform,//RB

                    leftFocusTransform+widthVectorTransform,//C LB
                    leftBottom+leftVector,//LB
                };
            }
            else if (this.LeftTail == GeometryArrowTailType.None && this.RightTail == GeometryArrowTailType.Arrow)
            {
                Vector2 rightFocusTransform = Vector2.Transform(base.TransformManager.Destination.CenterRight - focusVector, canvasToVirtualMatrix);
                Vector2 rightVector = rightFocusTransform - centerRight;

                points = new Vector2[7]
                {
                    centerRight,//R

                    rightTop+rightVector,//RT
                    rightFocusTransform-widthVectorTransform,//C RT

                    centerLeft-widthVectorTransform,//LT
                    centerLeft+widthVectorTransform,//LB

                    rightFocusTransform+widthVectorTransform,//C RB
                    rightBottom+rightVector,//RB
                };
            }
            else
            {
                points = new Vector2[4]
                {
                    centerLeft+widthVectorTransform,//LB
                    centerLeft-widthVectorTransform,//LT
                    centerRight-widthVectorTransform,//RT
                    centerRight+widthVectorTransform,//RB
                };
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryArrowLayer ArrowLayer = new GeometryArrowLayer();

            LayerBase.CopyWith(resourceCreator, ArrowLayer, this);
            return ArrowLayer;
        }

        public XElement Save()
        {
            XElement element = new XElement("GeometryArrowLayer");
            
            element.Add(new XElement("IsAbsolute", this.IsAbsolute));
            element.Add(new XElement("Width", this.Width));
            element.Add(new XElement("Value", this.Value));

            LayerBase.SaveWidth(element, this);
            return element;
        }

    }
}