using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;

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
        /// <param name="customDevice"> The custom-device. </param>
        public ImageLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photocopier"> The fill photocopier. </param>
        public ImageLayer(CanvasDevice customDevice, Transformer transformer, Photocopier photocopier)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
            this.Photocopier = photocopier;
        }


        public override  ILayer Clone(CanvasDevice customDevice)
        {
            ImageLayer imageLayer = new ImageLayer(customDevice)
            {
                Photocopier = this.Photocopier,
            };

            LayerBase.CopyWith(customDevice, imageLayer, this);
            return imageLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", this.Photocopier));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Photocopier") is XElement photocopier) this.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(photocopier);
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            if (this.bitmap == null)
            {
                CanvasCommandList command = new CanvasCommandList(resourceCreator);

                Transformer transformer = base.Transform.GetActualTransformer();
                CanvasGeometry geometry = transformer.ToRectangle(resourceCreator);

                using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
                {
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
            if (this.Transform.IsCrop == false) return effect;


            //Crop
            {
                CanvasCommandList command = new CanvasCommandList(resourceCreator);
                using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
                {
                    CanvasGeometry geometryCrop = this.Transform.CropTransformer.ToRectangle(resourceCreator);

                    using (drawingSession.CreateLayer(1, geometryCrop))
                    {
                        drawingSession.DrawImage(effect);
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


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Image");
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