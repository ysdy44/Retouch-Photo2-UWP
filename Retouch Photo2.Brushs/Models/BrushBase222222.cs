using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushBase454545454 : IBrush
    {
        public BrushType Type { get; set; }

        public Color Color { get; set; }

        public CanvasGradientStop[] Stops { get; set; }

        public Photocopier Photocopier { get; set; }
        public CanvasEdgeBehavior Extend { get; set; }

        public Vector2 Center { get; set; }
        Vector2 _startingCenter;
        public Vector2 XPoint { get; set; }
        Vector2 _startingXPoint;
        public Vector2 YPoint { get; set; }
        Vector2 _startingYPoint;



        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            if (this.Type == BrushType.None) return null;
            if (this.Type == BrushType.Color) return new CanvasSolidColorBrush(resourceCreator, this.Color);


            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

            if (this.Type == BrushType.LinearGradient)
            {
                return new CanvasLinearGradientBrush(resourceCreator, this.Stops)
                {
                    StartPoint = center,
                    EndPoint = yPoint,
                };
            }


            float radiusX = Vector2.Distance(center, xPoint);
            float radiusY = Vector2.Distance(center, yPoint);

            if (this.Type == BrushType.RadialGradient)
            {
                return new CanvasRadialGradientBrush(resourceCreator, this.Stops)
                {
                    RadiusX = radiusY,
                    RadiusY = radiusY,
                    Center = center
                };
            }

            if (this.Type == BrushType.EllipticalGradient)
            {
                float xRadians = FanKit.Math.VectorToRadians(xPoint - center);

                Matrix3x2 transformMatrix =
                    Matrix3x2.CreateTranslation(-center) *
                    Matrix3x2.CreateRotation(xRadians) *
                    Matrix3x2.CreateTranslation(center);

                return new CanvasRadialGradientBrush(resourceCreator, this.Stops)
                {
                    Transform = transformMatrix,
                    RadiusX = radiusX,
                    RadiusY = radiusY,
                    Center = center
                };
            }


            if (this.Type == BrushType.Image)
            {
                Photocopier photocopier = this.Photocopier;
                if (photocopier.Name == null) return null;

                Photo photo = Photo.FindFirstPhoto(photocopier);
                CanvasBitmap bitmap = photo.Source;

                float radian = FanKit.Math.Pi + FanKit.Math.VectorToRadians(this.Center - this.XPoint);
                float xScale = Vector2.Distance(this.XPoint, this.Center) / (float)bitmap.Size.Width;
                float yScale = Vector2.Distance(this.YPoint, this.Center) / (float)bitmap.Size.Height;

                Matrix3x2 matrix2 =
                    Matrix3x2.CreateTranslation(-this.Center) *
                    Matrix3x2.CreateScale(xScale, yScale) *
                    Matrix3x2.CreateRotation(radian) *
                    Matrix3x2.CreateTranslation(this.Center);

                return new CanvasImageBrush(resourceCreator, bitmap)
                {
                    Transform = matrix2 * matrix,
                    ExtendX = this.Extend,
                    ExtendY = this.Extend,
                };
            }

            return null;
        }



        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
        {
            if (this.Type == BrushType.None) return BrushOperateMode.None;
            if (this.Type == BrushType.Color) return BrushOperateMode.None;

            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, yPoint))
            {
                return BrushOperateMode.YPoint;
            }

            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, xPoint))
            {
                this._startingCenter = this.Center;
                this._startingXPoint = this.XPoint;
                this._startingYPoint = this.YPoint;
                return BrushOperateMode.XPoint;
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (FanKit.Math.InNodeRadius(point, center))
            {
                return BrushOperateMode.Center;
            }

            return BrushOperateMode.None;
        }
        public void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    break;

                case BrushType.LinearGradient:
                    {
                        switch (mode)
                        {
                            case BrushOperateMode.Center:
                                this.Center = point;
                                break;

                            case BrushOperateMode.YPoint:
                                this.YPoint = point;
                                break;
                        }
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        switch (mode)
                        {
                            case BrushOperateMode.Center:
                                {
                                    Vector2 center = point;
                                    Vector2 point2 = center + this._startingXPoint - this._startingCenter;
                                    this.Center = center;
                                    this.XPoint = point2;
                                }
                                break;

                            case BrushOperateMode.YPoint:
                                this.YPoint = point;
                                break;
                        }
                    }
                    break;


                case BrushType.EllipticalGradient:
                    {
                        switch (mode)
                        {
                            case BrushOperateMode.Center:
                                {
                                    Vector2 center = point;
                                    Vector2 xPoint = center + this._startingXPoint - this._startingCenter;
                                    Vector2 yPoint = center + this._startingYPoint - this._startingCenter;

                                    this.Center = center;
                                    this.XPoint = xPoint;
                                    this.YPoint = yPoint;
                                }
                                break;
                            case BrushOperateMode.XPoint:
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
                            case BrushOperateMode.YPoint:
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
                    break;
                case BrushType.Image:
                    {
                        switch (mode)
                        {
                            case BrushOperateMode.Center:
                                Vector2 offset = point - startingPoint;
                                this.Center = point;
                                this.XPoint = offset + this._startingXPoint;
                                this.YPoint = offset + this._startingYPoint;
                                break;
                            case BrushOperateMode.XPoint:
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
                            case BrushOperateMode.YPoint:
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
                    break;
                default:
                    break;
            }
        }
        public void InitializeController(Vector2 startingPoint, Vector2 point)
        {
            switch (this.Type)
            {
                case BrushType.None: break;
                case BrushType.Color: break;
                case BrushType.LinearGradient:
                    {
                        this.Center = startingPoint;
                        this.YPoint = point;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.Center = startingPoint;
                        this.YPoint = point;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.Center = startingPoint;
                        this.YPoint = point;
                        this.XPoint = this._controllerAPoint(startingPoint, point, point);
                    }
                    break;
                case BrushType.Image:
                    {
                        this.Center = startingPoint;
                        this.YPoint = point;
                        this.XPoint = this._controllerAPoint(startingPoint, point, point);
                    }
                    break;
                default:
                    break;
            }
        }


        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor)
        {
            if (this.Type == BrushType.None) return;
            if (this.Type == BrushType.Color) return;

            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, yPoint, Windows.UI.Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
                        drawingSession.FillCircle(yPoint, 10, Windows.UI.Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, yPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in this.Stops)
                        {
                            Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, yPoint, Windows.UI.Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
                        drawingSession.FillCircle(yPoint, 10, Windows.UI.Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, yPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in this.Stops)
                        {
                            Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, xPoint, Colors.White, 4);
                        drawingSession.DrawLine(center, yPoint, Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Colors.White);
                        drawingSession.FillCircle(yPoint, 10, Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, xPoint, accentColor, 2);
                        drawingSession.DrawLine(center, yPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in this.Stops)
                        {
                            Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }

                        //Circle: node
                        drawingSession.DrawNode2(xPoint);
                    }
                    break;
                case BrushType.Image:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, xPoint, Colors.White, 4);
                        drawingSession.DrawLine(center, yPoint, Colors.White, 4);

                        //Line: accent
                        drawingSession.DrawLine(center, xPoint, Colors.Red, 2);
                        drawingSession.DrawLine(center, yPoint, Colors.LimeGreen, 2);

                        //Circle: node
                        drawingSession.DrawNode2(center);
                        drawingSession.DrawNode2(xPoint);
                        drawingSession.DrawNode2(yPoint);
                    }
                    break;
            }
        }


        public IBrush Clone()
        {
            BrushBase454545454 brush = new BrushBase454545454();


            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    brush.Type = BrushType.Color;
                    brush.Color = this.Color;
                    break;

                case BrushType.LinearGradient:
                    brush.Type = BrushType.LinearGradient;
                    brush.Stops = (CanvasGradientStop[])this.Stops.Clone();
                    break;

                case BrushType.RadialGradient:
                    brush.Type = BrushType.RadialGradient;
                    brush.Stops = (CanvasGradientStop[])this.Stops.Clone();
                    break;

                case BrushType.EllipticalGradient:
                    brush.Type = BrushType.EllipticalGradient;
                    brush.Stops = (CanvasGradientStop[])this.Stops.Clone();
                    break;

                case BrushType.Image:
                    brush.Photocopier = this.Photocopier;
                    brush.Extend = this.Extend;
                    break;

                default:
                    return new BrushBase454545454();
            }


            brush.Center = this.Center;
            brush._startingCenter = this._startingCenter;

            brush.XPoint = this.XPoint;
            brush._startingXPoint = this._startingXPoint;

            brush.YPoint = this.YPoint;
            brush._startingYPoint = this._startingYPoint;


            return brush;
        }





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





        /// <summary>
        /// Initializes a ColorBrush.
        /// </summary>
        /// <param name="color"> The color. </param>
        public static IBrush ColorBrush(Color color)
        {
            return new BrushBase454545454
            {
                Type = BrushType.Color,

                Color = color,
            };
        }

        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>
        /// <param name="startPoint"> The start point. </param>
        /// <param name="endPoint"> The end point. </param>
        public static IBrush LinearGradientBrush(Vector2 startPoint, Vector2 endPoint)
        {
            return new BrushBase454545454
            {
                Type = BrushType.LinearGradient,

                Stops = GreyWhiteMeshHelpher.GetGradientStopArray(),

                Center = startPoint,
                YPoint = endPoint,
            };
        }
        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>       
        /// <param name="transformer"> The transformer. </param>
        public static IBrush LinearGradientBrush(Transformer transformer, CanvasGradientStop[] stops)
        {
            Vector2 center = transformer.CenterTop;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase454545454
            {
                Type = BrushType.LinearGradient,

                Stops = stops,

                Center = center,
                YPoint = yPoint,
            };
        }




        /// <summary>
        /// Initializes a RadialGradientBrush.
        /// </summary>
        public static IBrush RadialGradientBrush(Transformer transformer, CanvasGradientStop[] stops)
        {
            Vector2 center = transformer.Center;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase454545454
            {
                Type = BrushType.RadialGradient,

                Stops = stops,

                Center = center,
                YPoint = yPoint,
            };
        }



        /// <summary>
        /// Initializes a EllipticalGradientBrush.
        /// </summary>
        public static IBrush EllipticalGradientBrush(Transformer transformer, CanvasGradientStop[] stops)
        {
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase454545454
            {
                Type = BrushType.EllipticalGradient,

                Stops = stops,

                Center = center,
                XPoint = xPoint,
                YPoint = yPoint,
            };
        }








        private Vector2 _controllerAPoint(Vector2 center, Vector2 distancePoint, Vector2 bPoint)
        {
            Vector2 normalize = Vector2.Normalize(bPoint - center);
            float radius = Vector2.Distance(distancePoint, center);
            Vector2 reflect = new Vector2(normalize.Y, -normalize.X);
            Vector2 aPoint = radius * reflect + center;

            return aPoint;
        }



        /// <summary>
        /// Initializes a ImageBrush.
        /// </summary>
        public static IBrush ImageBrush(Transformer transformer, Photocopier photocopier)
        {
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase454545454
            {
                Type = BrushType.Image,

                Photocopier = photocopier,

                Center = center,
                XPoint = xPoint,
                YPoint = yPoint,
            };
        }


    }
}