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
    /// <see cref="IGeometryLayer"/>'s GeometryCookieLayer .
    /// </summary>
    public partial class GeometryCookieLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryCookieLayer";
         
        //@Content       
        public float InnerRadius = 0.5f;
        public float SweepAngle = FanKit.Math.Pi / 2f;

        //@Construct        
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryCookieLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        public GeometryCookieLayer()
        {
            base.Type = GeometryCookieLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCookieIcon(),
                Text = "Cookie",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateCookie(resourceCreator, transformer, canvasToVirtualMatrix, this.InnerRadius, this.SweepAngle);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCookieLayer CookieLayer = new GeometryCookieLayer();

            LayerBase.CopyWith(resourceCreator, CookieLayer, this);
            return CookieLayer;
        }


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("InnerRadius", this.InnerRadius));
            element.Add(new XElement("SweepAngle", this.SweepAngle));
        }
        public void Load(XElement element)
        {
            this.InnerRadius = (float)element.Element("InnerRadius");
            this.SweepAngle = (float)element.Element("SweepAngle");
        }

    }
}