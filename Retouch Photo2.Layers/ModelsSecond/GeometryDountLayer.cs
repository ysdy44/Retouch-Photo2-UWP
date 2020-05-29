using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GeometryDountLayer .
    /// </summary>
    public partial class GeometryDountLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryDount;

        //@Content       
        public float HoleRadius = 0.5f;
        public float StartingHoleRadius { get; private set; }
        public void CacheHoleRadius() => this.StartingHoleRadius = this.HoleRadius;

        //@Construct
        /// <summary>
        /// Initializes a pie-layer.
        /// </summary>
        public GeometryDountLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryDountIcon(),
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryDountLayer dountLayer = new GeometryDountLayer();

            LayerBase.CopyWith(resourceCreator, dountLayer, this);
            return dountLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("HoleRadius", this.HoleRadius));
        }
        public override void Load(XElement element)
        {
            this.HoleRadius = (float)element.Element("HoleRadius");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, this.HoleRadius);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, canvasToVirtualMatrix, this.HoleRadius);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.ConvertToCurvesFromDount(transformer, this.HoleRadius);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryDount");
        }

    }
}