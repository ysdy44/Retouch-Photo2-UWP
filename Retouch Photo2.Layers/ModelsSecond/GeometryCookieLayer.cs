using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCookieLayer .
    /// </summary>
    public partial class GeometryCookieLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCookie;

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
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCookieIcon(),
                Text = this.ConstructStrings(),
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateCookie(resourceCreator, transformer, canvasToVirtualMatrix, this.InnerRadius, this.SweepAngle);
        }


        public IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromCookie(transformer, this.InnerRadius, this.SweepAngle);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCookieLayer cookieLayer = new GeometryCookieLayer();

            LayerBase.CopyWith(resourceCreator, cookieLayer, this);
            return cookieLayer;
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

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryCookie");
        }

    }
}