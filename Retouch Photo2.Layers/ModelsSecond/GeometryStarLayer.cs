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
    /// <see cref="ILayer"/>'s GeometryStarLayer .
    /// </summary>
    public class GeometryStarLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryStar;

        //@Content       
        public int Points = 5;
        public float InnerRadius = 0.38f;

        //@Construct
        /// <summary>
        /// Construct a star-layer.
        /// </summary>
        public GeometryStarLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryStarIcon(),
                Text = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryStarLayer StarLayer = new GeometryStarLayer
            {
                Points=this.Points,
                InnerRadius= this.InnerRadius,
            };

            LayerBase.CopyWith(resourceCreator, StarLayer, this);
            return StarLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("Points", this.Points));
            element.Add(new XElement("InnerRadius", this.InnerRadius));
        }
        public override void Load(XElement element)
        {
            this.Points = (int)element.Element("Points");
            this.InnerRadius = (float)element.Element("InnerRadius");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateStar(resourceCreator, transformer, canvasToVirtualMatrix, this.Points, this.InnerRadius);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromStar(transformer, this.Points, this.InnerRadius);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryStar");
        }

    }
}