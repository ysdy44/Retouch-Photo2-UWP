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
    /// <see cref="IBrush"/>'s EllipticalGradientBrush.
    /// </summary>
    public class EllipticalGradientBrush : IBrush
    {
        //@Content
        public BrushType Type => BrushType.EllipticalGradient;

        public CanvasGradientStop[] Array { get; set; } = GreyWhiteMeshHelpher.GetGradientStopArray();
        public Color Color { get; set; }
        public Transformer Destination { set { } }
        public Photocopier Photocopier { get => new Photocopier(); }
        public CanvasEdgeBehavior Extend { get => CanvasEdgeBehavior.Clamp; set { } }


        /// <summary> <see cref="EllipticalGradientBrush"/>'s center point. </summary>
        public Vector2 Center;
        Vector2 _startingCenter;

        /// <summary> <see cref="EllipticalGradientBrush"/>'s x-point. </summary>
        public Vector2 XPoint;
        Vector2 _startingXPoint;

        /// <summary> <see cref="EllipticalGradientBrush"/>'s y-point. </summary>
        public Vector2 YPoint;
        Vector2 _startingYPoint;


        //@Construct
        /// <summary>
        /// Initializes a EllipticalGradientBrush.
        /// </summary>
        public EllipticalGradientBrush() { }
        /// <summary>
        /// Initializes a EllipticalGradientBrush.
        /// </summary>
        public EllipticalGradientBrush(Transformer transformer)
        {
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;
                       
            this.Center = center;
            this.XPoint = xPoint;
            this.YPoint = yPoint;
        }


        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator)
        {
            Vector2 center = this.Center;
            Vector2 xPoint = this.XPoint;
            Vector2 yPoint = this.YPoint;

            return this._getEllipticalGradientBrush(resourceCreator, center, xPoint, yPoint);
        }
        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

            return this._getEllipticalGradientBrush(resourceCreator, center, xPoint, yPoint);
        }
        private CanvasRadialGradientBrush _getEllipticalGradientBrush(ICanvasResourceCreator resourceCreator, Vector2 center, Vector2 xPoint, Vector2 yPoint)
        {
            float radiusX = Vector2.Distance(center, xPoint);
            float radiusY = Vector2.Distance(center, yPoint);
            Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                * Matrix3x2.CreateRotation(FanKit.Math.VectorToRadians(xPoint - center))
                * Matrix3x2.CreateTranslation(center);

            return new CanvasRadialGradientBrush(resourceCreator, this.Array)
            {
                Transform = transformMatrix,
                RadiusX = radiusX,
                RadiusY = radiusY,
                Center = center
            };
        }


        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
        {
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, xPoint))
            {
                this._startingCenter = this.Center;
                this._startingXPoint = this.XPoint;
                this._startingYPoint = this.YPoint;
                return BrushOperateMode.EllipticalXPoint;
            }

            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, yPoint))
            {
                this._startingCenter = this.Center;
                this._startingXPoint = this.XPoint;
                this._startingYPoint = this.YPoint;
                return BrushOperateMode.EllipticalYPoint;
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (FanKit.Math.InNodeRadius(point, center))
            {
                this._startingCenter = this.Center;
                this._startingXPoint = this.XPoint;
                this._startingYPoint = this.YPoint;
                return BrushOperateMode.EllipticalCenter;
            }

            return BrushOperateMode.None;
        }
        public void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point)
        {
            switch (mode)
            {
                case BrushOperateMode.EllipticalCenter:
                    {
                        Vector2 center = point;
                        Vector2 xPoint = center + this._startingXPoint - this._startingCenter;
                        Vector2 yPoint = center + this._startingYPoint - this._startingCenter;

                        this.Center = center;
                        this.XPoint = xPoint;
                        this.YPoint = yPoint;
                    }
                    break;

                case BrushOperateMode.EllipticalXPoint:
                    {
                        Vector2 xPoint = point;

                        Vector2 normalize = Vector2.Normalize(xPoint - this._startingCenter);
                        float radiusY = Vector2.Distance(this._startingYPoint, this._startingCenter);
                        Vector2 reflect = new Vector2(-normalize.Y, normalize.X);
                        Vector2 yPoint = radiusY * reflect + this._startingCenter;

                        this.XPoint = xPoint;
                        this.YPoint = yPoint;
                    }
                    break;

                case BrushOperateMode.EllipticalYPoint:
                    {
                        Vector2 yPoint = point;

                        Vector2 normalize = Vector2.Normalize(yPoint - this._startingCenter);
                        float radiusX = Vector2.Distance(this._startingXPoint, this._startingCenter);
                        Vector2 reflect = new Vector2(normalize.Y, -normalize.X);
                        Vector2 xPoint = radiusX * reflect + this._startingCenter;

                        this.XPoint = xPoint;
                        this.YPoint = yPoint;
                    }
                    break;
            }
        }

        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

            //Line: white
            drawingSession.DrawLine(center, xPoint, Windows.UI.Colors.White, 4);
            drawingSession.DrawLine(center, yPoint, Windows.UI.Colors.White, 4);

            //Circle: white
            drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
            drawingSession.FillCircle(yPoint, 10, Windows.UI.Colors.White);

            //Line: accent
            drawingSession.DrawLine(center, xPoint, accentColor, 2);
            drawingSession.DrawLine(center, yPoint, accentColor, 2);

            foreach (CanvasGradientStop stop in this.Array)
            {
                Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                //Circle: stop
                drawingSession.FillCircle(position, 8, accentColor);
                drawingSession.FillCircle(position, 6, stop.Color);
            }

            //Circle: node
            drawingSession.DrawNode2(xPoint);
        }


        public IBrush Clone()
        {
            return new EllipticalGradientBrush
            {
                Array = (CanvasGradientStop[])this.Array.Clone(),

                Center = this.Center,
                _startingCenter= this._startingCenter,

                XPoint = this.XPoint,
                _startingXPoint = this._startingXPoint,

                YPoint = this.YPoint,
                _startingYPoint = this._startingYPoint,
            };
        }

        public void SaveWith(XElement element)
        {
            element.Add(XML.SaveCanvasGradientStopArray("Array", this.Array));
            element.Add(FanKit.Transformers.XML.SaveVector2("Center", this.Center));
            element.Add(FanKit.Transformers.XML.SaveVector2("XPoint", this.XPoint));
            element.Add(FanKit.Transformers.XML.SaveVector2("YPoint", this.YPoint));
        }
        public void Load(XElement element)
        {
            if (element.Element("Array") is XElement array) this.Array = XML.LoadCanvasGradientStopArray(array);
            if (element.Element("Center") is XElement center) this.Center = FanKit.Transformers.XML.LoadVector2(center);
            if (element.Element("XPoint") is XElement xPoint) this.XPoint = FanKit.Transformers.XML.LoadVector2(xPoint);
            if (element.Element("YPoint") is XElement yPoint) this.YPoint = FanKit.Transformers.XML.LoadVector2(yPoint);
        }


        public void OneBrushPoints(Transformer transformer)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(transformer, Transformer.One);

            this._startingCenter = Vector2.Transform(this.Center, oneMatrix);
            this._startingXPoint = Vector2.Transform(this.XPoint, oneMatrix);
            this._startingYPoint = Vector2.Transform(this.YPoint, oneMatrix);
        }
        public void DeliverBrushPoints(Transformer transformer)
        {
            Matrix3x2 matrix = Transformer.FindHomography(Transformer.One, transformer);

            this.Center = Vector2.Transform(this._startingCenter, matrix);
            this.XPoint = Vector2.Transform(this._startingXPoint, matrix);
            this.YPoint = Vector2.Transform(this._startingYPoint, matrix);
        }


        //@Interface
        public void CacheTransform()
        {
            this._startingCenter = this.Center;
            this._startingXPoint = this.XPoint;
            this._startingYPoint = this.YPoint;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Center = Vector2.Transform(this._startingCenter, matrix);
            this.XPoint = Vector2.Transform(this._startingXPoint, matrix);
            this.YPoint = Vector2.Transform(this._startingYPoint, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Center = Vector2.Add(this._startingCenter, vector);
            this.XPoint = Vector2.Add(this._startingXPoint, vector);
            this.YPoint = Vector2.Add(this._startingYPoint, vector);
        }

    }
}