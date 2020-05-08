using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Elements;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;

namespace Retouch_Photo2.Brushs.Models
{
    /// <summary>
    /// <see cref="IBrush"/>'s ImageBrush.
    /// </summary>
    public class ImageBrush : IBrush
    {
        //@Content
        public BrushType Type => BrushType.Image;
        
        public CanvasGradientStop[] Array { get => null; set { } }
        public Color Color { get; set; }
        //public Transformer Destination { set { } }
        //public Photocopier Photocopier { get => new Photocopier(); }


        /// <summary> <see cref = "ImageBrush" />'s photocopier. </summary>
        public Photocopier Photocopier { get; set; }

        /// <summary> The source transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination transformer. </summary>
        public Transformer Destination { get; set; }
        Transformer _startingDestination;


        //@Construct
        /// <summary>
        /// Constructs a <see cref = "ImageBrush" />.
        /// </summary>
        public ImageBrush() { }
        /// <summary>
        /// Constructs a <see cref = "ImageBrush" />.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        public ImageBrush(Transformer transformer)
        {
            this.Source = transformer;
            this.Destination = transformer;
        }
        /// <summary>
        /// Constructs a <see cref = "ImageBrush" />.
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        public ImageBrush(Transformer source, Transformer destination)
        {
            this.Source = source;
            this.Destination = destination;
        }


        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator)
        {         
            Photocopier photocopier = this.Photocopier;
            if (photocopier.Name == null)  return null;

            Photo photo = Photo.FindFirstPhoto(photocopier);
            CanvasBitmap bitmap = photo.Source;

            Matrix3x2 matrix2 = Transformer.FindHomography(this.Source, this.Destination);
            return new CanvasImageBrush(resourceCreator, bitmap)
            {
                Transform = matrix2
            };
        }
        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Photocopier photocopier = this.Photocopier;
            if (photocopier.Name == null)return null;

            Photo photo = Photo.FindFirstPhoto(photocopier);
            CanvasBitmap bitmap = photo.Source;

            Matrix3x2 matrix2 = Transformer.FindHomography(this.Source, this.Destination);
            return new CanvasImageBrush(resourceCreator, bitmap)
            {
                Transform = matrix2 * matrix
            };
        }


        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
        {
            return BrushOperateMode.None;
        }
        public void Controller(BrushOperateMode mode, Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor)
        {
            drawingSession.DrawBoundNodes(this.Destination * matrix);
        }


        public IBrush Clone()
        {
            return new ImageBrush
            {
                Photocopier = this.Photocopier,
                Source = this.Source,
                Destination = this.Destination,
            };
        }

        public void SaveWith(XElement element)
        {
            element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", this.Photocopier));
            element.Add(FanKit.Transformers.XML.SaveTransformer("Source", this.Source));
            element.Add(FanKit.Transformers.XML.SaveTransformer("Destination", this.Destination));
        }
        public void Load(XElement element)
        {
            if (element.Element("Photocopier") is XElement photocopier) this.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(photocopier);
            if (element.Element("Source") is XElement source) this.Source = FanKit.Transformers.XML.LoadTransformer(source);
            if (element.Element("Destination") is XElement destination) this.Destination = FanKit.Transformers.XML.LoadTransformer(destination);
        }


        public void OneBrushPoints(Transformer transformer)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(transformer, Transformer.One);

            this._startingDestination = Transformer.Multiplies(this.Destination, oneMatrix);
        }
        public void DeliverBrushPoints(Transformer transformer)
        {
            Matrix3x2 matrix = Transformer.FindHomography(Transformer.One, transformer);

            this.Destination = Transformer.Multiplies(this._startingDestination, matrix);
        }


        //@Interface
        public void CacheTransform()
        {
            this._startingDestination = this.Destination;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Destination = Transformer.Multiplies(this._startingDestination, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Destination = Transformer.Add(this._startingDestination, vector);
        }

    }
}