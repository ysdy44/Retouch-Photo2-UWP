using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
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
                    break;
            }

            
            switch (this.Type)
            {
                case BrushType.LinearGradient:
                    element.Add(FanKit.Transformers.XML.SaveVector2("StartPoint", this.Center));
                    element.Add(FanKit.Transformers.XML.SaveVector2("EndPoint", this.YPoint));
                    break;
                case BrushType.RadialGradient:
                    element.Add(FanKit.Transformers.XML.SaveVector2("Center", this.Center));
                    element.Add(FanKit.Transformers.XML.SaveVector2("Point", this.YPoint));
                    break;
                case BrushType.EllipticalGradient:
                case BrushType.Image:
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
                        in stops.Elements("Stop")
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
                    break;
            }

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                    if (element.Element("StartPoint") is XElement startPoint) this.Center = FanKit.Transformers.XML.LoadVector2(startPoint);
                    if (element.Element("EndPoint") is XElement endPoint) this.YPoint = FanKit.Transformers.XML.LoadVector2(endPoint);
                    break;
                case BrushType.RadialGradient:
                    if (element.Element("Center") is XElement center) this.Center = FanKit.Transformers.XML.LoadVector2(center);
                    if (element.Element("Point") is XElement point) this.YPoint = FanKit.Transformers.XML.LoadVector2(point);
                    break;
                case BrushType.EllipticalGradient:
                case BrushType.Image:
                    if (element.Element("Center") is XElement center2) this.Center = FanKit.Transformers.XML.LoadVector2(center2);
                    if (element.Element("XPoint") is XElement xPoint) this.XPoint = FanKit.Transformers.XML.LoadVector2(xPoint);
                    if (element.Element("YPoint") is XElement yPoint) this.YPoint = FanKit.Transformers.XML.LoadVector2(yPoint);
                    break;
            }

        }

    }
}