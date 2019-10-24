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
    /// <see cref="IGeometryLayer"/>'s GeometryPieLayer .
    /// </summary>
    public partial class GeometryPieLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryPieLayer";
         
        //@Content       
        public float InnerRadius = 0.0f;
        public float SweepAngle = FanKit.Math.Pi / 2f;

        //@Construct        
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryPieLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        public GeometryPieLayer()
        {
            base.Type = GeometryPieLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryPieIcon(),
                Text = "Pie",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            PieType pieType = this.GetPieType(this.InnerRadius == 0, this.SweepAngle == 0);

            switch (pieType)
            {
                case PieType.Cirle:
                    {
                        Transformer transformer = base.TransformManager.Destination;
                        return transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
                    }
                case PieType.Donut:
                    {
                        return this._getDonut(resourceCreator, this.InnerRadius, canvasToVirtualMatrix);
                    }
                case PieType.Pie:
                    {
                        Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
                        Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;

                        CanvasPathBuilder pie = this._getPie(resourceCreator, this.SweepAngle);
                        return CanvasGeometry.CreatePath(pie).Transform(matrix);
                    }
                case PieType.DonutAndPie:
                    {
                        Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
                        Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;

                        CanvasPathBuilder donutAndPie = this._getDonutAndPie(resourceCreator, this.InnerRadius, this.SweepAngle);
                        return CanvasGeometry.CreatePath(donutAndPie).Transform(matrix);
                    }
            }

            {
                Transformer transformer = base.TransformManager.Destination;
                return transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
            }
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPieLayer PieLayer = new GeometryPieLayer();

            LayerBase.CopyWith(resourceCreator, PieLayer, this);
            return PieLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GeometryPieLayer");
            
            element.Add(new XElement("InnerRadius", this.InnerRadius));
            element.Add(new XElement("SweepAngle", this.SweepAngle));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            this.InnerRadius = (float)element.Element("InnerRadius");
            this.SweepAngle = (float)element.Element("SweepAngle");
            LayerBase.LoadWith(element, this);
        }

    }
}