using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushBase : IBrush
    {
        
        public void SaveWith(XElement element)
        { 
            switch (this.Type)
            {
                case BrushType.Color:
                    element.Add(FanKit.Transformers.XML.SaveColor("Color", this.Color));
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    if (this.Stops == null) this.Type = BrushType.None;
                    else
                    {
                        element.Add(new XElement
                        (
                            "Stops",
                            from stop
                            in this.Stops
                            select XML.SaveStop("Stop", stop)
                        ));
                    }
                    break;

                case BrushType.Image:
                    element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", this.Photocopier));
                    break;
            }

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                case BrushType.Image:
                    element.Add(new XElement("Extend", this.Extend));

                    element.Add(FanKit.Transformers.XML.SaveVector2("Center", this.Center));
                    element.Add(FanKit.Transformers.XML.SaveVector2("XPoint", this.XPoint));
                    element.Add(FanKit.Transformers.XML.SaveVector2("YPoint", this.YPoint));
                    break;
            }
        }

        public void Load(XElement element)
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    if (element.Element("Color") is XElement color) this.Color = FanKit.Transformers.XML.LoadColor(color);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    if (element.Element("Stops") is XElement stops) this.Stops =
                    (
                        from stop
                        in stops.Elements()
                        select XML.LoadStop(stop)
                    ).ToArray();
                    break;

                case BrushType.Image:
                    if (element.Element("Photocopier") is XElement photocopier) this.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(photocopier);
                    break;
            }


            switch (this.Type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                case BrushType.Image:

                    if (element.Element("Extend") is XElement extend) this.Extend = Retouch_Photo2.Brushs.XML.CreateExtend(extend.Value);

                    if (element.Element("Center") is XElement center) this.Center = FanKit.Transformers.XML.LoadVector2(center);
                    if (element.Element("XPoint") is XElement xPoint) this.XPoint = FanKit.Transformers.XML.LoadVector2(xPoint);
                    if (element.Element("YPoint") is XElement yPoint) this.YPoint = FanKit.Transformers.XML.LoadVector2(yPoint);
                    break;
            }
        }

    }
}