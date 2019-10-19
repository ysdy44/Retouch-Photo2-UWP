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



        #region XElement


        //@Static
        /// <summary>
        /// Saves the entire TransformManager to a XElement.
        /// </summary>
        /// <returns> The saved XElement. </returns>
        public static XElement Save(TransformManager transformManager)
        {
            XElement element = new XElement("TransformManager");
            {
                //Source
                XElement source = new XElement("Source");
                TransformManager._save(source, transformManager.Source);
                element.Add(source);

                //Destination
                XElement destination = new XElement("Destination");
                TransformManager._save(destination, transformManager.Destination);
                element.Add(destination);

                //DisabledRadian
                element.Add(new XElement("DisabledRadian", transformManager.DisabledRadian));

                //IsCrop
                element.Add(new XElement("IsCrop", transformManager.IsCrop));

                //CropDestination
                XElement cropDestination = new XElement("CropDestination");
                TransformManager._save(cropDestination, transformManager.CropDestination);
                element.Add(cropDestination);
            }
            return element;
        }
        /// <summary>
        ///  Loads a TransformManager from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded TransformManager. </returns>
        public static TransformManager Load(XElement element)
        {
            return new TransformManager
            {
                Source = TransformManager._load(element.Element("Source")),
                Destination = TransformManager._load(element.Element("Destination")),
                DisabledRadian = (bool)element.Element("DisabledRadian"),

                IsCrop = (bool)element.Element("IsCrop"),
                CropDestination = TransformManager._load(element.Element("CropDestination")),
            };
        }
               

        private static void _save(XElement element, Transformer transformer)
        {
            //LeftTop
            XElement leftTop = new XElement("LeftTop");
            leftTop.Add(new XElement("X", transformer.LeftTop.X));
            leftTop.Add(new XElement("Y", transformer.LeftTop.Y));
            element.Add(leftTop);

            //RightTop
            XElement rightTop = new XElement("RightTop");
            rightTop.Add(new XElement("X", transformer.RightTop.X));
            rightTop.Add(new XElement("Y", transformer.RightTop.Y));
            element.Add(rightTop);

            //RightBottom
            XElement rightBottom = new XElement("RightBottom");
            rightBottom.Add(new XElement("X", transformer.RightBottom.X));
            rightBottom.Add(new XElement("Y", transformer.RightBottom.Y));
            element.Add(rightBottom);

            //LeftBottom
            XElement leftBottom = new XElement("LeftBottom");
            leftBottom.Add(new XElement("X", transformer.LeftBottom.X));
            leftBottom.Add(new XElement("Y", transformer.LeftBottom.Y));
            element.Add(leftBottom);
        }
        private static Transformer _load(XElement element)
        {
            XElement leftTop = new XElement("LeftTop");
            XElement rightTop = new XElement("RightTop");
            XElement rightBottom = new XElement("RightBottom");
            XElement leftBottom = new XElement("LeftBottom");
                       
            return new Transformer
            {
                LeftTop = new Vector2
                {
                    X = (float)leftTop.Element("X"),
                    Y = (float)leftTop.Element("Y"),
                },
                RightTop = new Vector2
                {
                    X = (float)rightTop.Element("X"),
                    Y = (float)rightTop.Element("Y"),
                },
                RightBottom = new Vector2
                {
                    X = (float)rightBottom.Element("X"),
                    Y = (float)rightBottom.Element("Y"),
                },
                LeftBottom = new Vector2
                {
                    X = (float)leftBottom.Element("X"),
                    Y = (float)leftBottom.Element("Y"),
                },
            };
        }


        #endregion
               

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