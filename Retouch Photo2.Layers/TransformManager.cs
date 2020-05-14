using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// <see cref = "Transformer" />'s manager. 
    /// </summary>
    public partial class TransformManager : ICacheTransform
    {

        /// <summary> The source transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination transformer. </summary>
        public Transformer Destination { get; set; }
        /// <summary> The cache of <see cref="TransformManager.Destination"/>. </summary>
        public Transformer StartingDestination { get; private set; }


        /// <summary> Is cropped? </summary>
        public bool IsCrop { get; set; }
        /// <summary> The cropped destination transformer. </summary>
        public Transformer CropDestination { get; set; }
        /// <summary> The cache of <see cref="TransformManager.CropDestination"/>. </summary>
        public Transformer StartingCropDestination { get; private set; }


        //@Construct
        /// <summary>
        /// Constructs a <see cref = "TransformManager" />.
        /// </summary>
        public TransformManager() { }
        /// <summary>
        /// Constructs a <see cref = "TransformManager" />.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        public TransformManager(Transformer transformer)
        {
            this.Source = transformer;
            this.Destination = transformer;
        }
        /// <summary>
        /// Constructs a <see cref = "TransformManager" />.
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        public TransformManager(Transformer source, Transformer destination)
        {
            this.Source = source;
            this.Destination = destination;
        }
        

        /// <summary>
        /// Gets transformer-matrix's resulting matrix.
        /// </summary>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);
 
        /// <summary>
        /// Get TransformManager own copy.
        /// </summary>
        /// <returns> The cloned TransformManager. </returns>
        public TransformManager Clone()
        {
            return new TransformManager
            {
                Source = this.Source,
                Destination = this.Destination,

                IsCrop = this.IsCrop,
                CropDestination = this.CropDestination,
            };
        }


        //@Abstract
        public void CacheTransform()
        {
            this.StartingDestination = this.Destination;
            this.StartingCropDestination = this.CropDestination;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Destination = this.StartingDestination * matrix;
            this.CropDestination = this.StartingCropDestination * matrix;
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Destination = this.StartingDestination + vector;
            this.CropDestination = this.StartingCropDestination + vector;
        }
                       

        public static ICanvasImage Render(TransformManager transformManager, ICanvasResourceCreator resourceCreator, ICanvasImage image, Matrix3x2 matrix)
        {
            if (transformManager.IsCrop == false) return image;

            
            CanvasGeometry canvasGeometry = transformManager.CropDestination.ToRectangle(resourceCreator, matrix);
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