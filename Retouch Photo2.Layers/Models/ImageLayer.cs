// Core:              ★★★★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★★
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Photos;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Image;


        /// <summary> <see cref = "ImageLayer" />'s photocopier. </summary>
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

        //@Construct   
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>       
        public ImageLayer()
        {
        }
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        /// <param name="photo"> The fill photo. </param>
        public ImageLayer(Photo photo)
        {
            Photocopier photocopier = photo.ToPhotocopier();
            this.Photocopier = photocopier;

            //Transformer
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);
            base.Transform = new Transform(transformerSource);
        }


        public override ILayer Clone() => LayerBase.CopyWith(this, new ImageLayer
        {
            Photocopier = this.Photocopier,
        });


        public override void SaveWith(XElement element)
        {
            element.Add(Retouch_Photo2.Photos.XML.SavePhotocopier("Photocopier", this.Photocopier));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Photocopier") is XElement photocopier) this.Photocopier = Retouch_Photo2.Photos.XML.LoadPhotocopier(photocopier);
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            if (this.bitmap == null)
            {
                CanvasCommandList command = new CanvasCommandList(resourceCreator);
                using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
                {
                    Transformer transformer = base.Transform.GetActualTransformer();
                    CanvasGeometry geometry = transformer.ToRectangle(resourceCreator);

                    drawingSession.FillGeometry(geometry, Colors.White);
                }
                return command;
            }


            //Image
            Matrix3x2 matrix = Transformer.FindHomography(this.transformerRect, base.Transform.Transformer);
            Transform2DEffect effect = new Transform2DEffect
            {
                TransformMatrix = matrix,
                Source = this.bitmap,
            };

            if (this.Transform.IsCrop == false)
            {
                switch (base.Style.Transparency.Type)
                {
                    case BrushType.LinearGradient:
                    case BrushType.RadialGradient:
                    case BrushType.EllipticalGradient:
                        {
                            CanvasCommandList command = new CanvasCommandList(resourceCreator);
                            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
                            {
                                Transformer transformer = base.Transform.Transformer;
                                CanvasGeometry geometryCrop = transformer.ToRectangle(resourceCreator);
                                ICanvasBrush canvasBrush = this.Style.Transparency.GetICanvasBrush(resourceCreator);

                                using (drawingSession.CreateLayer(canvasBrush, geometryCrop))
                                {
                                    drawingSession.DrawImage(effect);
                                }
                            }
                            return command;
                        }
                    default:
                        return effect;
                }
            }


            //Crop
            {
                CanvasCommandList command = new CanvasCommandList(resourceCreator);
                using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
                {
                    Transformer transformer = base.Transform.CropTransformer;
                    CanvasGeometry geometryCrop = transformer.ToRectangle(resourceCreator);

                    switch (base.Style.Transparency.Type)
                    {
                        case BrushType.LinearGradient:
                        case BrushType.RadialGradient:
                        case BrushType.EllipticalGradient:
                            {
                                ICanvasBrush canvasBrush = this.Style.Transparency.GetICanvasBrush(resourceCreator);
                                using (drawingSession.CreateLayer(canvasBrush, geometryCrop))
                                {
                                    drawingSession.DrawImage(effect);
                                }
                            }
                            break;
                        default:
                            using (drawingSession.CreateLayer(1, geometryCrop))
                            {
                                drawingSession.DrawImage(effect);
                            }
                            break;
                    }
                }
                return command;
            }
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator, matrix);
        }


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            CanvasGeometry geometry = transformer.ToRectangle(resourceCreator);

            return new NodeCollection(geometry);
        }

        /// <summary>
        /// Convert to image brush.
        /// </summary>
        /// <returns> The product brush. </returns>
        public IBrush ToBrush()
        {
            Photocopier photocopier = this.Photocopier;
            Transformer transformer = this.Transform.Transformer;
            return BrushBase.ImageBrush(transformer, photocopier);
        }

    }
}