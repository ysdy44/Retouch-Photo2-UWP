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
        //public CanvasEdgeBehavior Extend { get => CanvasEdgeBehavior.Clamp; set { } }


        /// <summary> <see cref = "ImageBrush" />'s photocopier. </summary>
        public Photocopier Photocopier { get; set; }

        /// <summary> The source transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination transformer. </summary>
        public Transformer Destination { get; set; }
        public Transformer StartingDestination;

        /// <summary> The edge behavior. </summary>
        public CanvasEdgeBehavior Extend { get; set; }


        //@Construct
        /// <summary>
        /// Constructs a <see cref = "ImageBrush" />.
        /// </summary>
        public ImageBrush() { }
        /// <summary>
        /// Constructs a <see cref = "ImageBrush" />.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="extend"> The edge behavior. </param>
        public ImageBrush(Transformer transformer, CanvasEdgeBehavior extend = CanvasEdgeBehavior.Clamp)
        {
            this.Source = transformer;
            this.Destination = transformer;
            this.Extend = extend;
        }
        /// <summary>
        /// Constructs a <see cref = "ImageBrush" />.
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <param name="extend"> The edge behavior. </param>
        public ImageBrush(Transformer source, Transformer destination, CanvasEdgeBehavior extend = CanvasEdgeBehavior.Clamp)
        {
            this.Source = source;
            this.Destination = destination;
            this.Extend = extend;
        }


        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator)
        {
            Photocopier photocopier = this.Photocopier;
            if (photocopier.Name == null) return null;

            Photo photo = Photo.FindFirstPhoto(photocopier);
            CanvasBitmap bitmap = photo.Source;

            Matrix3x2 matrix2 = Transformer.FindHomography(this.Source, this.Destination);
            return new CanvasImageBrush(resourceCreator, bitmap)
            {
                Transform = matrix2,
                ExtendX = this.Extend,
                ExtendY = this.Extend,
            };
        }
        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Photocopier photocopier = this.Photocopier;
            if (photocopier.Name == null) return null;

            Photo photo = Photo.FindFirstPhoto(photocopier);
            CanvasBitmap bitmap = photo.Source;

            Matrix3x2 matrix2 = Transformer.FindHomography(this.Source, this.Destination);
            return new CanvasImageBrush(resourceCreator, bitmap)
            {
                Transform = matrix2 * matrix,
                ExtendX = this.Extend,
                ExtendY = this.Extend,
            };
        }


        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix)
        {
            TransformerMode transformerMode = Transformer.ContainsNodeMode(point, this.Destination, matrix);

            return this.Converter(transformerMode);
        }
        public void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point)
        {
            TransformerMode transformerMode = this.Converter(mode);

            this.Destination = Transformer.Controller(transformerMode, startingPoint, point, this.StartingDestination);
        }

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
                Extend = this.Extend,
            };
        }

        public void SaveWith(XElement element)
        {
            element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", this.Photocopier));
            element.Add(FanKit.Transformers.XML.SaveTransformer("Source", this.Source));
            element.Add(FanKit.Transformers.XML.SaveTransformer("Destination", this.Destination));
            element.Add(new XAttribute("Extend", this.Extend));
        }
        public void Load(XElement element)
        {
            if (element.Element("Photocopier") is XElement photocopier) this.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(photocopier);
            if (element.Element("Source") is XElement source) this.Source = FanKit.Transformers.XML.LoadTransformer(source);
            if (element.Element("Destination") is XElement destination) this.Destination = FanKit.Transformers.XML.LoadTransformer(destination);
            if (element.Element("Extend") is XElement extend) this.Extend = Retouch_Photo2.Brushs.XML.CreateExtend(extend.Value);
        }


        //@Interface
        public void CacheTransform()
        {
            this.StartingDestination = this.Destination;
        }
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Destination = Transformer.Multiplies(this.StartingDestination, matrix);
        }
        public void TransformAdd(Vector2 vector)
        {
            this.Destination = Transformer.Add(this.StartingDestination, vector);
        }


        private BrushOperateMode Converter(TransformerMode mode)
        {
            switch (mode)
            {
                case TransformerMode.None: return BrushOperateMode.None;

                case TransformerMode.Translation: return BrushOperateMode.ImageTranslation;
                case TransformerMode.Rotation: return BrushOperateMode.ImageRotation;

                case TransformerMode.SkewLeft: return BrushOperateMode.ImageSkewLeft;
                case TransformerMode.SkewTop: return BrushOperateMode.ImageSkewTop;
                case TransformerMode.SkewRight: return BrushOperateMode.ImageSkewRight;
                case TransformerMode.SkewBottom: return BrushOperateMode.ImageSkewBottom;

                case TransformerMode.ScaleLeft: return BrushOperateMode.ImageScaleLeft;
                case TransformerMode.ScaleTop: return BrushOperateMode.ImageScaleTop;
                case TransformerMode.ScaleRight: return BrushOperateMode.ImageScaleRight;
                case TransformerMode.ScaleBottom: return BrushOperateMode.ImageScaleBottom;

                case TransformerMode.ScaleLeftTop: return BrushOperateMode.ImageScaleLeftTop;
                case TransformerMode.ScaleRightTop: return BrushOperateMode.ImageScaleRightTop;
                case TransformerMode.ScaleRightBottom: return BrushOperateMode.ImageScaleRightBottom;
                case TransformerMode.ScaleLeftBottom: return BrushOperateMode.ImageScaleLeftBottom;

                default: return BrushOperateMode.None;
            }
        }
        private TransformerMode Converter(BrushOperateMode mode)
        {
            switch (mode)
            {
                case BrushOperateMode.None: return TransformerMode.None;

                case BrushOperateMode.ImageTranslation: return TransformerMode.Translation;
                case BrushOperateMode.ImageRotation: return TransformerMode.Rotation;

                case BrushOperateMode.ImageSkewLeft: return TransformerMode.SkewLeft;
                case BrushOperateMode.ImageSkewTop: return TransformerMode.SkewTop;
                case BrushOperateMode.ImageSkewRight: return TransformerMode.SkewRight;
                case BrushOperateMode.ImageSkewBottom: return TransformerMode.SkewBottom;

                case BrushOperateMode.ImageScaleLeft: return TransformerMode.ScaleLeft;
                case BrushOperateMode.ImageScaleTop: return TransformerMode.ScaleTop;
                case BrushOperateMode.ImageScaleRight: return TransformerMode.ScaleRight;
                case BrushOperateMode.ImageScaleBottom: return TransformerMode.ScaleBottom;

                case BrushOperateMode.ImageScaleLeftTop: return TransformerMode.ScaleLeftTop;
                case BrushOperateMode.ImageScaleRightTop: return TransformerMode.ScaleRightTop;
                case BrushOperateMode.ImageScaleRightBottom: return TransformerMode.ScaleRightBottom;
                case BrushOperateMode.ImageScaleLeftBottom: return TransformerMode.ScaleLeftBottom;

                default: return TransformerMode.None;
            }
        }

    }
}