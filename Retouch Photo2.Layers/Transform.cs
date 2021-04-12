// Core:              ★★★★★
// Referenced:   ★★★★
// Difficult:         ★★
// Only:              ★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a transform that provides an <see cref="FanKit.Transformers.Transformer"/>.
    /// </summary>
    public class Transform : ICacheTransform
    {

        /// <summary> The transformer. </summary>
        public Transformer Transformer { get; set; }
        /// <summary> The cache of <see cref="Transform.Transformer"/>. </summary>
        public Transformer StartingTransformer { get; private set; }


        /// <summary> Is cropped? </summary>
        public bool IsCrop { get; set; }
        /// <summary> The cache of <see cref="Transform.IsCrop"/>. </summary>
        public bool StartingIsCrop { get; set; }

        /// <summary> The cropped transformer. </summary>
        public Transformer CropTransformer { get; set; }
        /// <summary> The cache of <see cref="Transform.CropTransformer"/>. </summary>
        public Transformer StartingCropTransformer { get; private set; }


        //@Construct
        /// <summary>
        /// Initialize a <see cref = "Transform" />.
        /// </summary>
        public Transform() { }
        /// <summary>
        /// Initialize a <see cref = "Transform" />.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        public Transform(Transformer transformer)
        {
            this.Transformer = transformer;
        }

        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned transform. </returns>
        public Transform Clone()
        {
            return new Transform
            {
                Transformer = this.Transformer,
                StartingTransformer = this.StartingTransformer,

                IsCrop = this.IsCrop,
                StartingIsCrop = this.StartingIsCrop,

                CropTransformer = this.CropTransformer,
                StartingCropTransformer = this.StartingCropTransformer,
            };
        }


        internal Transformer GetActualTransformer() => this.IsCrop ? this.CropTransformer : this.Transformer;


        /// <summary>
        /// Cache the class's transformer. Ex: _oldTransformer = Transformer.
        /// </summary>
        public virtual void CacheTransform()
        {
            this.StartingTransformer = this.Transformer;
            this.StartingIsCrop = this.IsCrop;
            this.StartingCropTransformer = this.GetActualTransformer();
        }
        /// <summary>
        /// Transforms the class by the given vector. Ex: Transformer.Add()
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public virtual void TransformAdd(Vector2 vector)
        {
            this.Transformer = this.StartingTransformer + vector;
            this.CropTransformer = this.StartingCropTransformer + vector;
        }
        /// <summary>
        /// Transforms the class by the given vector. Ex: Transformer.Add()
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public virtual void CropTransformAdd(Vector2 vector)
        {
            this.CropTransformer = this.StartingCropTransformer + vector;
        }
        /// <summary>
        /// Transforms the class by the given matrix. Ex: Transformer.Multiplies()
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public virtual void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Transformer = this.StartingTransformer * matrix;
            this.CropTransformer = this.StartingCropTransformer * matrix;
        }


        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public void DrawCrop(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            if (this.IsCrop)
            {
                Transformer transformer = this.Transformer;
                drawingSession.DrawBound(transformer, matrix, accentColor);

                Transformer cropTransformer = this.CropTransformer;
                drawingSession.DrawCrop(cropTransformer, matrix, Colors.BlueViolet);
            }
            else
            {
                Transformer transformer = this.Transformer;
                drawingSession.DrawCrop(transformer, matrix, accentColor);
            }
        }


        //@Static
        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="transform"> The transform. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="image"> The source image. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The rendered image. </returns>
        public static ICanvasImage Render(Transform transform, ICanvasResourceCreator resourceCreator, ICanvasImage image, Matrix3x2 matrix)
        {
            if (transform.IsCrop == false) return image;


            CanvasGeometry canvasGeometry = transform.CropTransformer.ToRectangle(resourceCreator, matrix);
            CanvasCommandList canvasCommandList = new CanvasCommandList(resourceCreator);

            using (CanvasDrawingSession drawingSession = canvasCommandList.CreateDrawingSession())
            {
                drawingSession.FillGeometry(canvasGeometry, Colors.Gray);

                Rect cropRect = image.GetBounds(resourceCreator);
                drawingSession.DrawImage(image, (float)cropRect.X, (float)cropRect.Y, cropRect, 1.0f, CanvasImageInterpolation.NearestNeighbor, CanvasComposite.SourceIn);
            }
            return canvasCommandList;
        }

    }
}