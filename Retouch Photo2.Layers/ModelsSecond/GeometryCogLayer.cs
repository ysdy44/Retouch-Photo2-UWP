using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCogLayer .
    /// </summary>
    public class GeometryCogLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryCogLayer";
         
        //@Content
        public int Count = 8;
        public float InnerRadius = 0.7f;
        public float Tooth;
        public float Notch;

        //@Construct      
        /// <summary>
        /// Construct a cog-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryCogLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a cog-layer.
        /// </summary>
        public GeometryCogLayer()
        {
            base.Type = GeometryCogLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCogIcon(),
                Text = "Cog",
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
            Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;

                        
            float angle = FanKit.Math.Pi * 2f / this.Count;//angle
            float angleTooth = angle * this.Tooth;//angle tooth
            float angleNotch = angle * this.Notch;//angle notch
            float angleDiffHalf = (angleNotch - angleTooth) / 2;//Half the angle difference between the tooth and the notch

            float rotation = 0;//Start angle is zero
            int countQuadra = this.Count * 4;
            Vector2[] points = new Vector2[countQuadra];

            for (int i = 0; i < countQuadra; i++)
            {
                Vector2 vector = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
                int remainder = i % 4;//remainder

                if (remainder == 0)//凸 left-bottom point
                {
                    points[i] = Vector2.Transform(vector * this.InnerRadius, matrix);
                    rotation += angleDiffHalf;
                }
                else if (remainder == 1)//凸 left-top point
                {
                    points[i] = Vector2.Transform(vector, matrix);
                    rotation += angleTooth;
                }
                else if (remainder == 2)//凸 right-top point
                {
                    points[i] = Vector2.Transform(vector, matrix);
                    rotation += angleDiffHalf;
                }
                else if (remainder == 3)//凸 right-bottom point
                {
                    points[i] = Vector2.Transform(vector * this.InnerRadius, matrix);
                    rotation += angle - angleNotch;
                }
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCogLayer CogLayer = new GeometryCogLayer();

            LayerBase.CopyWith(resourceCreator, CogLayer, this);
            return CogLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GeometryCogLayer");
            
            element.Add(new XElement("Count", this.Count));
            element.Add(new XElement("InnerRadius", this.InnerRadius));
            element.Add(new XElement("Tooth", this.Tooth));
            element.Add(new XElement("Notch", this.Notch));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            this.Count = (int)element.Element("Count");
            this.InnerRadius = (float)element.Element("InnerRadius");
            this.Tooth = (float)element.Element("Tooth");
            this.Notch = (float)element.Element("Notch");

            LayerBase.LoadWith(element, this);
        }

    }
}