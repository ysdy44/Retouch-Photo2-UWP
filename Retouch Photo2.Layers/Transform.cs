using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a brush that provides an <see cref="Matrix3x2"/>.
    /// </summary>
    public class Transform : ICacheTransform, IGetActualTransformer
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
        /// Initialize a <see cref = "Transform" />.
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        public Transform(Transformer source, Transformer destination)
        {
            this.Transformer = destination;
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


        //@Abstract      
        public Transformer GetActualTransformer() => this.IsCrop ? this.CropTransformer : this.Transformer;

        public void CropTransformAdd(Vector2 vector)
        {
            this.CropTransformer = this.StartingCropTransformer + vector;
        }

        public void CacheTransform()
        {
            this.StartingTransformer = this.Transformer;
            this.StartingIsCrop = this.IsCrop;
            this.StartingCropTransformer = this.GetActualTransformer();
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Transformer = this.StartingTransformer * matrix;
            this.CropTransformer = this.StartingCropTransformer * matrix;
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Transformer = this.StartingTransformer + vector;
            this.CropTransformer = this.StartingCropTransformer + vector;
        }


        //@Static
        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="filter"> The filter. </param>
        /// <param name="image"> The source image. </param>
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