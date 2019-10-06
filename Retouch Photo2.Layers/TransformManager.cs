using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// <see cref = "Transformer" />'s manager. 
    /// </summary>
    public class TransformManager : ICacheTransform
    {
        
        /// <summary> The source transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination transformer. </summary>
        public Transformer Destination { get; set; }
        Transformer _startingDestination;
        /// <summary> Is disable rotate radian? Defult **false**. </summary>
        public bool DisabledRadian { get; set; }
        

        /// <summary> Is cropped? </summary>
        public bool IsCrop { get; set; }
        /// <summary> The cropped destination transformer. </summary>
        public Transformer CropDestination { get; set; }
        Transformer _startingCropDestination;
      

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
                DisabledRadian = this.DisabledRadian,

                IsCrop = this.IsCrop,
                CropDestination = this.CropDestination,
            };
        }


        //@Abstract
        public void CacheTransform()
        {
            this._startingDestination = this.Destination;
            this._startingCropDestination = this.CropDestination;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Destination = this._startingDestination * matrix;
            this.CropDestination = this._startingCropDestination * matrix;
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Destination = this._startingDestination + vector;
            this.CropDestination = this._startingCropDestination + vector;
        }


        //@Static
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