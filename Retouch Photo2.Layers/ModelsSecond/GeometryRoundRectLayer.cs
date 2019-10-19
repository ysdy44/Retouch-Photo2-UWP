using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryRoundRectLayer .
    /// </summary>
    public class GeometryRoundRectLayer : IGeometryLayer, ILayer
    {
        //@Content       
        public string Type => "GeometryRoundRectLayer";

        public float Corner = 0.25f;

        //@Construct
        public GeometryRoundRectLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryRoundRectIcon(),
                Text = "RoundRect",
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            //LTRB
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


            //Control
            float minLength = System.Math.Min(horizontalLength, verticalLength);
            float minLength2 = this.Corner * minLength;

            Vector2 horizontal2 = minLength2 * horizontalUnit;
            Vector2 horizontal448 = horizontal2 *  0.448f;// vector / (1 - 4 * 0.552f)
            Vector2 vertical2 = minLength2 * verticalUnit;
            Vector2 vertical448 = vertical2 *  0.448f;// vector /  (1 - 4 * 0.552f)


            Vector2 leftTop_Left = leftTop + vertical2;
            Vector2 leftTop_Left2= leftTop + vertical448;
            Vector2 leftTop_Top = leftTop + horizontal2;
            Vector2 leftTop_Top1 = leftTop + horizontal448;

            Vector2 rightTop_Top = rightTop - horizontal2;
            Vector2 rightTop_Top2 = rightTop - horizontal448;
            Vector2 rightTop_Right = rightTop + vertical2;
            Vector2 rightTop_Right1 = rightTop + vertical448;

            Vector2 rightBottom_Right = rightBottom - vertical2;
            Vector2 rightBottom_Right2 = rightBottom - vertical448;
            Vector2 rightBottom_Bottom = rightBottom - horizontal2;
            Vector2 rightBottom_Bottom1 = rightBottom - horizontal448;

            Vector2 leftBottom_Bottom = leftBottom + horizontal2;
            Vector2 leftBottom_Bottom2 = leftBottom + horizontal448;
            Vector2 leftBottom_Left = leftBottom - vertical2;
            Vector2 leftBottom_Left1 = leftBottom - vertical448;
                         

            //Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(leftTop_Left);

            pathBuilder.AddCubicBezier(leftTop_Left2, leftTop_Top1, leftTop_Top);
            pathBuilder.AddLine(rightTop_Top);

            pathBuilder.AddCubicBezier(rightTop_Top2, rightTop_Right1, rightTop_Right);
            pathBuilder.AddLine(rightBottom_Right);

            pathBuilder.AddCubicBezier(rightBottom_Right2, rightBottom_Bottom1, rightBottom_Bottom);
            pathBuilder.AddLine(leftBottom_Bottom);

            pathBuilder.AddCubicBezier(leftBottom_Bottom2, leftBottom_Left1, leftBottom_Left);
            pathBuilder.AddLine(leftBottom_Left);

            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRoundRectLayer RoundRectLayer = new GeometryRoundRectLayer();

            LayerBase.CopyWith(resourceCreator, RoundRectLayer, this);
            return RoundRectLayer;
        }

        public XElement Save()
        {
            XElement element = new XElement("GeometryRoundRectLayer");
            
            element.Add(new XElement("Corner", this.Corner));

            LayerBase.SaveWidth(element, this);
            return element;
        }

    }
}