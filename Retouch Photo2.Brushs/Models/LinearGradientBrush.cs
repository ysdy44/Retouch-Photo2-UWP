using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;

namespace Retouch_Photo2.Brushs.Models
{
    /// <summary>
    /// <see cref="IBrush"/>'s LinearGradientBrush.
    /// </summary>
    public class LinearGradientBrush : IBrush
    {
        //@Content
        public BrushType Type => BrushType.LinearGradient;
                
        public CanvasGradientStop[] Array { get; set; } = GreyWhiteMeshHelpher.GetGradientStopArray();
        public Color Color { get; set; }
        public Transformer Destination { set { } }
        public Photocopier Photocopier { get => new Photocopier(); }
        public CanvasEdgeBehavior Extend { get => CanvasEdgeBehavior.Clamp; set { } }


        /// <summary> <see cref="LinearGradientBrush"/>'s start point. </summary>
        public Vector2 StartPoint;
        Vector2 _startingStartPoint;

        /// <summary> <see cref="LinearGradientBrush"/>'s end point. </summary>
        public Vector2 EndPoint;
        Vector2 _startingEndPoint;


        //@Construct
        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>
        public LinearGradientBrush() { }
        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>
        /// <param name="startPoint"> The start point. </param>
        /// <param name="endPoint"> The end point. </param>
        public LinearGradientBrush(Vector2 startPoint, Vector2 endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>       
        /// <param name="transformer"> The transformer. </param>
        public LinearGradientBrush(Transformer transformer)
        {
            Vector2 startPoint = transformer.CenterTop;
            Vector2 endPoint = transformer.CenterBottom;

            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>
        /// <param name="startPoint"> The starting point. </param>
        /// <param name="endPoint"> The point. </param>   
        /// <param name="inverseMatrix"> The inverse matrix. </param>
        public LinearGradientBrush(Vector2 startingPoint, Vector2 point, Matrix3x2 inverseMatrix)
        {
            Vector2 startPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 endPoint = Vector2.Transform(point, inverseMatrix);

            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }


        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator)
        {
            Vector2 startPoint = this.StartPoint;
            Vector2 endPoint = this.EndPoint;

            return new CanvasLinearGradientBrush(resourceCreator, this.Array)
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
            };
        }
        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);

            return new CanvasLinearGradientBrush(resourceCreator, this.Array)
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
            };
        }


        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, startPoint))
            {
                return BrushOperateMode.LinearStartPoint;
            }

            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, endPoint))
            {
                return BrushOperateMode.LinearEndPoint;
            }

            return BrushOperateMode.None;
        }
        public void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point)
        {
            switch (mode)
            {
                case BrushOperateMode.LinearStartPoint:
                    this.StartPoint = point;
                    break;

                case BrushOperateMode.LinearEndPoint:
                    this.EndPoint = point;
                    break;
            }
        }

        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor)
        {
            Vector2 startPoint = Vector2.Transform(this.StartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(this.EndPoint, matrix);

            //Line: white
            drawingSession.DrawLine(startPoint, endPoint, Windows.UI.Colors.White, 4);

            //Circle: white
            drawingSession.FillCircle(startPoint, 10, Windows.UI.Colors.White);
            drawingSession.FillCircle(endPoint, 10, Windows.UI.Colors.White);

            //Line: accent
            drawingSession.DrawLine(startPoint, endPoint, accentColor, 2);

            foreach (CanvasGradientStop stop in this.Array)
            {
                Vector2 position = startPoint * (1.0f - stop.Position) + endPoint * stop.Position;

                //Circle: stop
                drawingSession.FillCircle(position, 8, accentColor);
                drawingSession.FillCircle(position, 6, stop.Color);
            }
        }


        public IBrush Clone()
        {
            return new LinearGradientBrush
            {
                Array = (CanvasGradientStop[])this.Array.Clone(),

                StartPoint = this.StartPoint,
                _startingStartPoint = this._startingStartPoint,

                EndPoint = this.EndPoint,
                _startingEndPoint = this._startingEndPoint,
            };
        }

        public void SaveWith(XElement element)
        {
            element.Add(XML.SaveCanvasGradientStopArray("Array", this.Array));
            element.Add(FanKit.Transformers.XML.SaveVector2("StartPoint", this.StartPoint));
            element.Add(FanKit.Transformers.XML.SaveVector2("EndPoint", this.EndPoint));
        }
        public void Load(XElement element)
        {
            if (element.Element("Array") is XElement array) this.Array = XML.LoadCanvasGradientStopArray(array);
            if (element.Element("StartPoint") is XElement startPoint) this.StartPoint = FanKit.Transformers.XML.LoadVector2(startPoint);
            if (element.Element("EndPoint") is XElement endPoint) this.EndPoint = FanKit.Transformers.XML.LoadVector2(endPoint);
        }


        //@Interface
        public void CacheTransform()
        {
            this._startingStartPoint = this.StartPoint;
            this._startingEndPoint = this.EndPoint;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.StartPoint = Vector2.Transform(this._startingStartPoint, matrix);
            this.EndPoint = Vector2.Transform(this._startingEndPoint, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.StartPoint = Vector2.Add(this._startingStartPoint, vector);
            this.EndPoint = Vector2.Add(this._startingEndPoint, vector);
        }

    }
}