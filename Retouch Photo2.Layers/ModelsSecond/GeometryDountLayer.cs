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
    /// <see cref="GeometryLayer"/>'s GeometryDountLayer .
    /// </summary>
    public partial class GeometryDountLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryDount;

        //@Content       
        public float HoleRadius = 0.5f;
        public float StartingHoleRadius { get; private set; }
        public void CacheHoleRadius() => this.StartingHoleRadius = this.HoleRadius;
         

        public override ILayer Clone() => LayerBase.CopyWith(this, new GeometryDountLayer
        {
            HoleRadius = this.HoleRadius,
        });

        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("HoleRadius", this.HoleRadius));
        }
        public override void Load(XElement element)
        {
            if (element.Element("HoleRadius") is XElement holeRadius) this.HoleRadius = (float)holeRadius;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, this.HoleRadius);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, matrix, this.HoleRadius);
        }

    }
}