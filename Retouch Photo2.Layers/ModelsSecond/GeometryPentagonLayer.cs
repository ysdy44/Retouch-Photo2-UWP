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
    /// <see cref="GeometryLayer"/>'s GeometryPentagonLayer .
    /// </summary>
    public class GeometryPentagonLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryPentagon;

        //@Content
        public int Points = 5;
        public int StartingPoints { get; private set; }
        public void CachePoints() => this.StartingPoints = this.Points;


        public override ILayer Clone() => LayerBase.CopyWith(this, new GeometryPentagonLayer
        {
            Points = this.Points,
        });

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Points", this.Points));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Points") is XElement points) this.Points = (int)points;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreatePentagon(resourceCreator, transformer, this.Points);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreatePentagon(resourceCreator, transformer, matrix, this.Points);
        }

    }
}