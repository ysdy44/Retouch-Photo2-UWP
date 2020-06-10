using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
    public partial class BrushBase : IBrush
    {
        public BrushType Type { get; internal set; }

        public Color Color { get; set; }

        public CanvasEdgeBehavior Extend { get; set; }
        public CanvasGradientStop[] Stops { get; set; }
        public Photocopier Photocopier
        {
            get => this.photocopier;
            set
            {
                this.photocopier = value;

                Photocopier photocopier = this.Photocopier;
                if (photocopier.Name == null) return;

                Photo photo = Photo.FindFirstPhoto(photocopier);
                CanvasBitmap bitmap = photo.Source;
                this.bitmap = bitmap;

                float canvasWidth = (float)bitmap.Size.Width;
                float canvasHeight = (float)bitmap.Size.Height;

                TransformerRect transformerRect = new TransformerRect(canvasWidth, canvasHeight, Vector2.Zero);
                this.transformerRect = transformerRect;
            }
        }
        private Photocopier photocopier;
        private CanvasBitmap bitmap;
        private TransformerRect transformerRect;


        public Vector2 Center { get; set; }
        public Vector2 StartingCenter { get; private set; }
        public Vector2 XPoint { get; set; }
        public Vector2 StartingXPoint { get; private set; }
        public Vector2 YPoint { get; set; }
        public Vector2 StartingYPoint { get; private set; }


        public IBrush Clone()
        {
            BrushBase brush = new BrushBase()
            {
                Type = this.Type
            };

            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    brush.Color = this.Color;
                    break;
            }


            brush.Extend = this.Extend;

            brush.Center = this.Center;
            brush.StartingCenter = this.StartingCenter;

            brush.XPoint = this.XPoint;
            brush.StartingXPoint = this.StartingXPoint;

            brush.YPoint = this.YPoint;
            brush.StartingYPoint = this.StartingYPoint;

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    if (this.Stops!=null) brush.Stops = (CanvasGradientStop[])this.Stops.Clone();
                    break;

                case BrushType.Image:
                    brush.Photocopier = this.Photocopier;
                    break;

                default:
                    break;
            }

            return brush;
        }


        public void CacheTransform()
        {
            this.StartingCenter = this.Center;
            this.StartingXPoint = this.XPoint;
            this.StartingYPoint = this.YPoint;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Center = Vector2.Transform(this.StartingCenter, matrix);
            this.XPoint = Vector2.Transform(this.StartingXPoint, matrix);
            this.YPoint = Vector2.Transform(this.StartingYPoint, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Center = Vector2.Add(this.StartingCenter, vector);
            this.XPoint = Vector2.Add(this.StartingXPoint, vector);
            this.YPoint = Vector2.Add(this.StartingYPoint, vector);
        }


        //@Static
        // Keep both handles at right angles
        // X-Point to Y-Point
        public static Vector2 XToY(Vector2 xPoint, Vector2 center, float radiusY)
        {
            Vector2 normalize = Vector2.Normalize(xPoint - center);
            Vector2 reflect = new Vector2(-normalize.Y, normalize.X);

            Vector2 yPoint = radiusY * reflect + center;
            return yPoint;
        }

        // Keep both handles at right angles
        // Y-Point to X-Point
        public static Vector2 YToX(Vector2 yPoint, Vector2 center, float radiusX)
        {
            Vector2 normalize = Vector2.Normalize(yPoint - center);
            Vector2 reflect = new Vector2(normalize.Y, -normalize.X);

            Vector2 xPoint = radiusX * reflect + center;
            return xPoint;
        }
        public static Vector2 YToX(Vector2 yPoint, Vector2 center)
        {
            float radiusY = Vector2.Distance(yPoint, center);
            float radiusX = radiusY;// If radiusX = radiusY ? Yes !

            return BrushBase.YToX(yPoint, center, radiusX);
        }
        public static Vector2 YToX(Vector2 yPoint, Vector2 center, float width, float height)
        {
            float radiusY = Vector2.Distance(yPoint, center);
            float radiusX = radiusY / height * width;//Ratio

            return BrushBase.YToX(yPoint, center, radiusX);
        }

    }
}