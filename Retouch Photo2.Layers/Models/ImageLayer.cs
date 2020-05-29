using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

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
            base.Control = new LayerControl(this)
            {
                Icon = new ImageIcon(),
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photocopier"> The fill photocopier. </param>
        public ImageLayer(Transformer transformer, Photocopier photocopier)
        {
            base.Control = new LayerControl(this)
            {
                Icon = new ImageIcon(),
                Type = this.ConstructStrings(),
            };
            this.Photocopier = photocopier;
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer = new ImageLayer()
            {
                Photocopier = this.Photocopier,
            };

            LayerBase.CopyWith(resourceCreator, imageLayer, this);
            return imageLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", this.Photocopier));
        }
        public override void Load(XElement element)
        {
            this.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(element.Element("Photocopier"));
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            if (this.bitmap == null) return null;

            Matrix3x2 matrix2 = Transformer.FindHomography(this.transformerRect, base.Transform.Transformer);
            Transform2DEffect effect = new Transform2DEffect
            {
                TransformMatrix = matrix2,
                Source = this.bitmap,
                //TODO:  Cubic
                //InterpolationMode= CanvasImageInterpolation.Cubic
            };

            if (this.Transform.IsCrop)
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
            else return effect;
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.ConvertToCurvesFromRectangle(transformer);
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