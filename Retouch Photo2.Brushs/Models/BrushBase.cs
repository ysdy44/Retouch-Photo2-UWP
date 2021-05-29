// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★★★★
// Only:              ★★★★
// Complete:      ★★★★
using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Photos;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
    public partial class BrushBase : IBrush
    {

        /// <summary> Gets the type. </summary>
        public BrushType Type { get; internal set; }

        /// <summary> Gets of sets the color. </summary>
        public Color Color { get; set; }

        /// <summary> Gets of sets the extend. </summary>
        public CanvasEdgeBehavior Extend { get; set; }
        /// <summary> Gets of sets the stops. </summary>
        public CanvasGradientStop[] Stops { get; set; }
        /// <summary> Gets of sets the photocopier. </summary>
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


        /// <summary> Gets of sets the center point. (LinearGradientBrush.StartPoint) (RadialGradientBrush.Center) </summary>
        public Vector2 Center { get; set; }
        /// <summary> Cache of <see cref="IBrush.Center"/>. </summary>
        public Vector2 StartingCenter { get; private set; }
        /// <summary> Gets of sets the x-point. </summary>
        public Vector2 XPoint { get; set; }
        /// <summary> Cache of <see cref="IBrush.XPoint"/>. </summary>
        public Vector2 StartingXPoint { get; private set; }
        /// <summary> Gets of sets the y-point. (LinearGradientBrush.EndPoint) (RadialGradientBrush.Point) </summary>
        public Vector2 YPoint { get; set; }
        /// <summary> Cache of <see cref="IBrush.YPoint"/>. </summary>
        public Vector2 StartingYPoint { get; private set; }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned <see cref="IBrush"/>. </returns>
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
                    if (this.Stops!=null) brush.Stops = this.Stops.CloneArray();
                    break;

                case BrushType.Image:
                    brush.Photocopier = this.Photocopier;
                    break;

                default:
                    break;
            }

            return brush;
        }


        /// <summary>
        /// Cache the class's transformer. Ex: _oldTransformer = Transformer.
        /// </summary>
        public void CacheTransform()
        {
            this.StartingCenter = this.Center;
            this.StartingXPoint = this.XPoint;
            this.StartingYPoint = this.YPoint;
        }
        /// <summary>
        /// Transforms the class by the given vector. Ex: Transformer.Add()
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector)
        {
            this.Center = Vector2.Add(this.StartingCenter, vector);
            this.XPoint = Vector2.Add(this.StartingXPoint, vector);
            this.YPoint = Vector2.Add(this.StartingYPoint, vector);
        }
        /// <summary>
        /// Transforms the class by the given matrix. Ex: Transformer.Multiplies()
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Center = Vector2.Transform(this.StartingCenter, matrix);
            this.XPoint = Vector2.Transform(this.StartingXPoint, matrix);
            this.YPoint = Vector2.Transform(this.StartingYPoint, matrix);
        }


        //@Static
        // Keep both handles at right angles
        // X-Point to Y-Point
        private static Vector2 XToY(Vector2 xPoint, Vector2 center, float radiusY)
        {
            Vector2 normalize = Vector2.Normalize(xPoint - center);
            Vector2 reflect = new Vector2(-normalize.Y, normalize.X);

            Vector2 yPoint = radiusY * reflect + center;
            return yPoint;
        }

        // Keep both handles at right angles
        // Y-Point to X-Point
        private static Vector2 YToX(Vector2 yPoint, Vector2 center, float radiusX)
        {
            Vector2 normalize = Vector2.Normalize(yPoint - center);
            Vector2 reflect = new Vector2(normalize.Y, -normalize.X);

            Vector2 xPoint = radiusX * reflect + center;
            return xPoint;
        }
        private static Vector2 YToX(Vector2 yPoint, Vector2 center)
        {
            float radiusY = Vector2.Distance(yPoint, center);
            float radiusX = radiusY; // If radiusX = radiusY ? Yes !

            return BrushBase.YToX(yPoint, center, radiusX);
        }
        private static Vector2 YToX(Vector2 yPoint, Vector2 center, float width, float height)
        {
            float radiusY = Vector2.Distance(yPoint, center);
            float radiusX = radiusY / height * width; // Ratio

            return BrushBase.YToX(yPoint, center, radiusX);
        }

    }
}