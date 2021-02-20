// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryStarLayer .
    /// </summary>
    public class GeometryStarLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryStar;

        //@Content       
        public int Points = 5;
        public int StartingPoints { get; private set; }
        public void CachePoints() => this.StartingPoints = this.Points;

        public float InnerRadius = 0.38f;
        public float StartingInnerRadius { get; private set; }
        public void CacheInnerRadius() => this.StartingInnerRadius = this.InnerRadius;


        public override ILayer Clone() => LayerBase.CopyWith(this, new GeometryStarLayer
        {
            Points = this.Points,
            InnerRadius = this.InnerRadius,
        });

        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("Points", this.Points));
            element.Add(new XElement("InnerRadius", this.InnerRadius));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Points") is XElement points) this.Points = (int)points;
            if (element.Element("InnerRadius") is XElement innerRadius) this.InnerRadius = (float)innerRadius;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateStar(resourceCreator, transformer, this.Points, this.InnerRadius);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateStar(resourceCreator, transformer, matrix, this.Points, this.InnerRadius);
        }

    }
}