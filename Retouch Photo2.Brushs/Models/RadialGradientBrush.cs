using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;

namespace Retouch_Photo2.Brushs.Models
{
    /// <summary>
    /// <see cref="IBrush"/>'s RadialGradientBrush.
    /// </summary>
    public class RadialGradientBrush : IBrush
    {
        //@Content
        public BrushType Type => BrushType.RadialGradient;

        public CanvasGradientStop[] Array { get; set; } = GreyWhiteMeshHelpher.GetGradientStopArray();
        public Color Color { get; set; }
        public Transformer Destination { set { } }
        public Photocopier Photocopier { get => new Photocopier(); }
        public CanvasEdgeBehavior Extend { get => CanvasEdgeBehavior.Clamp; set { } }


        /// <summary> <see cref="RadialGradientBrush"/>'s center point. </summary>
        public Vector2 Center;
        Vector2 _startingCenter;

        /// <summary> <see cref="RadialGradientBrush"/>'s control point. </summary>
        public Vector2 Point;
        Vector2 _startingPoint;


        //@Construct
        /// <summary>
        /// Initializes a RadialGradientBrush.
        /// </summary>
        public RadialGradientBrush() { }
        /// <summary>
        /// Initializes a RadialGradientBrush.
        /// </summary>
        public RadialGradientBrush(Transformer transformer)
        {
            Vector2 center = transformer.Center;
            Vector2 point = transformer.CenterBottom;

            this.Center = center;
            this.Point = point;
        }


        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator)
        {
            Vector2 center = this.Center;
            Vector2 point = this.Point;

            return this._getRadialGradientBrush(resourceCreator, center, point);
        }
        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 point = Vector2.Transform(this.Point, matrix);

            return this._getRadialGradientBrush(resourceCreator, center, point);
        }
        private CanvasRadialGradientBrush _getRadialGradientBrush(ICanvasResourceCreator resourceCreator, Vector2 center, Vector2 point)
        {
            float radius = Vector2.Distance(center, point);

            return new CanvasRadialGradientBrush(resourceCreator, this.Array)
            {
                RadiusX = radius,
                RadiusY = radius,
                Center = center
            };
        }


        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
        {
            Vector2 point2 = Vector2.Transform(this.Point, matrix);
            if (FanKit.Math.InNodeRadius(point, point2))
            {
                return BrushOperateMode.RadialPoint;
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (FanKit.Math.InNodeRadius(point, center))
            {
                this._startingPoint = this.Point;
                this._startingCenter = this.Center;
                return BrushOperateMode.RadialCenter;
            }

            return BrushOperateMode.None;
        }
        public void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point)
        {
            switch (mode)
            {
                case BrushOperateMode.RadialCenter:
                    {
                        Vector2 center = point;
                        Vector2 point2 = center + this._startingPoint - this._startingCenter;
                        this.Center = center;
                        this.Point = point2;
                    }
                    break;

                case BrushOperateMode.RadialPoint:
                    this.Point = point;
                    break;
            }
        }

        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 point2 = Vector2.Transform(this.Point, matrix);

            //Line: white
            drawingSession.DrawLine(center, point2, Windows.UI.Colors.White, 4);

            //Circle: white
            drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
            drawingSession.FillCircle(point2, 10, Windows.UI.Colors.White);

            //Line: accent
            drawingSession.DrawLine(center, point2, accentColor, 2);

            foreach (CanvasGradientStop stop in this.Array)
            {
                Vector2 position = center * (1.0f - stop.Position) + point2 * stop.Position;

                //Circle: stop
                drawingSession.FillCircle(position, 8, accentColor);
                drawingSession.FillCircle(position, 6, stop.Color);
            }
        }


        public IBrush Clone()
        {
            return new RadialGradientBrush
            {
                Array = (CanvasGradientStop[])this.Array.Clone(),

                Center = this.Center,
                _startingCenter = this._startingCenter,

                Point = this.Point,
                _startingPoint = this._startingPoint,
            };
        }

        public void SaveWith(XElement element)
        {
            element.Add(XML.SaveCanvasGradientStopArray("Array", this.Array));
            element.Add(FanKit.Transformers.XML.SaveVector2("Center", this.Center));
            element.Add(FanKit.Transformers.XML.SaveVector2("Point", this.Point));
        }
        public void Load(XElement element)
        {
            if (element.Element("Array") is XElement array) this.Array = XML.LoadCanvasGradientStopArray(array);
            if (element.Element("Center") is XElement center) this.Center = FanKit.Transformers.XML.LoadVector2(center);
            if (element.Element("Point") is XElement point) this.Point = FanKit.Transformers.XML.LoadVector2(point);
        }


        //@Interface
        public void CacheTransform()
        {
            this._startingCenter = this.Center;
            this._startingPoint = this.Point;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Center = Vector2.Transform(this._startingCenter, matrix);
            this.Point = Vector2.Transform(this._startingPoint, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Center = Vector2.Add(this._startingCenter, vector);
            this.Point = Vector2.Add(this._startingPoint, vector);
        }

    }
}