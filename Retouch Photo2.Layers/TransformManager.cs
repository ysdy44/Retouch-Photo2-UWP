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
    public struct TransformManager
    {
        /// <summary>
        /// Gets transformer-matrix's resulting matrix.
        /// </summary>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);


        /// <summary> The source transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination transformer. </summary>
        public Transformer Destination { get; set; }
        public Transformer _oldDestination;
        /// <summary> Is disable rotate radian? Defult **false**. </summary>
        public bool DisabledRadian { get; set; }


        /// <summary> Is cropped? </summary>
        public bool IsCrop { get; set; }
        /// <summary> The cropped destination transformer. </summary>
        public Transformer CropDestination { get; set; }
        public Transformer _oldCropDestination;


        /// <summary>
        /// Gets showed destination transformer.
        /// </summary>
        /// <returns> Return **CropDestination** if the **IsCrop** is true, otherwise **Destination**. </returns>
        public Transformer GetShowDestination()
        {
            return this.IsCrop ? this.CropDestination : this.Destination;
        }


        //@Static
        public static TransformManager Set_oldDestination(TransformManager transformManager, Transformer value)
        {
            transformManager._oldDestination = value;
            return transformManager;
        }
        public static TransformManager Set_oldCropDestination(TransformManager transformManager, Transformer value)
        {
            transformManager._oldCropDestination = value;
            return transformManager;
        }
        public static TransformManager SetSource(TransformManager transformManager, Transformer value)
        {
            transformManager.Source = value;
            return transformManager;
        }
        public static TransformManager SetDestination(TransformManager transformManager, Transformer value)
        {
            transformManager.Destination = value;
            return transformManager;
        }
        public static TransformManager SetCropDestination(TransformManager transformManager, Transformer value)
        {
            transformManager.CropDestination = value;
            return transformManager;
        }
        public static TransformManager SetIsCrop(TransformManager transformManager, bool value)
        {
            transformManager.IsCrop = value;
            return transformManager;
        }
        public static TransformManager SetDisabledRadian(TransformManager transformManager, bool value)
        {
            transformManager.DisabledRadian = value;
            return transformManager;
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