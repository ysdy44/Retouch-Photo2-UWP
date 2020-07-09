using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryCookieLayer .
    /// </summary>
    public partial class GeometryCookieLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCookie;

        //@Content       
        public float InnerRadius = 0.5f;
        public float StartingInnerRadius { get; private set; }
        public void CacheInnerRadius() => this.StartingInnerRadius = this.InnerRadius;

        public float SweepAngle = FanKit.Math.Pi / 2f;
        public float StartingSweepAngle { get; private set; }
        public void CacheSweepAngle() => this.StartingSweepAngle = this.SweepAngle;

        //@Construct
        /// <summary>
        /// Initializes a pie-layer.
        /// </summary>
        public GeometryCookieLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryCookieLayer cookieLayer = new GeometryCookieLayer(customDevice)
            {
                InnerRadius = this.InnerRadius,
                SweepAngle = this.SweepAngle,
            };

            LayerBase.CopyWith(customDevice, cookieLayer, this);
            return cookieLayer;
        }

        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("InnerRadius", this.InnerRadius));
            element.Add(new XElement("SweepAngle", this.SweepAngle));
        }
        public override void Load(XElement element)
        {
            if (element.Element("InnerRadius") is XElement innerRadius) this.InnerRadius = (float)innerRadius;
            if (element.Element("SweepAngle") is XElement sweepAngle) this.SweepAngle = (float)sweepAngle;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateCookie(resourceCreator, transformer, this.InnerRadius, this.SweepAngle);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateCookie(resourceCreator, transformer, matrix, this.InnerRadius, this.SweepAngle);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryCookie");
        }

    }
}