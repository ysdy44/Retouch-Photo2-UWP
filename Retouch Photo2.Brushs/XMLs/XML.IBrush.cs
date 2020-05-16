using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="IBrush"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="brush"> The destination <see cref="IBrush"/>. </param>
        public static XElement SaveBrush(string elementName, IBrush brush)
        {
            XElement element = new XElement(elementName);
            element.Add(new XAttribute("Type", brush.Type));
            element.Add(FanKit.Transformers.XML.SaveColor("Color", brush.Color));

            if (brush.Stops != null) element.Add(new XElement
            (
                "Stops",
                from stop
                in brush.Stops
                select XML.SaveStop("Stop", stop)
            ));
            
            element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", brush.Photocopier));
            element.Add(new XAttribute("Extend", brush.Extend));

            element.Add(FanKit.Transformers.XML.SaveVector2("Center", brush.Center));
            element.Add(FanKit.Transformers.XML.SaveVector2("XPoint", brush.XPoint));
            element.Add(FanKit.Transformers.XML.SaveVector2("YPoint", brush.YPoint));

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="IBrush"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="IBrush"/>. </returns>
        public static IBrush LoadBrush(XElement element)
        {
            BrushBase brush = new BrushBase();
            if (element.Attribute("Type") is XAttribute type) brush.Type = XML.CreateBrushType(type.Value);

            if (element.Element("Color") is XElement color) brush.Color = FanKit.Transformers.XML.LoadColor(color);

            if (element.Element("Stops") is XElement stops) brush.Stops =
            (
                from stop
                in stops.Elements()
                select XML.LoadStop(stop)
            ).ToArray();

            if (element.Element("Photocopier") is XElement photocopier) brush.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(photocopier);
            if (element.Element("Extend") is XElement extend) brush.Extend = Retouch_Photo2.Brushs.XML.CreateExtend(extend.Value);

            if (element.Element("Center") is XElement center) brush.Center = FanKit.Transformers.XML.LoadVector2(center);
            if (element.Element("XPoint") is XElement xPoint) brush.XPoint = FanKit.Transformers.XML.LoadVector2(xPoint);
            if (element.Element("YPoint") is XElement yPoint) brush.YPoint = FanKit.Transformers.XML.LoadVector2(yPoint);

            return brush;
        }

    }
}