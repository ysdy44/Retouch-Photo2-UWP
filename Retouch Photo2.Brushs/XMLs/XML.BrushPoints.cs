using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="BrushPoints"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="points"> The source data. </param>
        /// <param name="type"> The gradient type. </param>
        /// <returns> The saved XElement. </returns>
        private static XElement SaveBrushPoints(string elementName, BrushPoints points, GradientBrushType type)
        {
            switch (type)
            {
                case GradientBrushType.Linear:
                    return new XElement
                    (
                         elementName,
                         FanKit.Transformers.XML.SaveVector2("LinearGradientStartPoint", points.LinearGradientStartPoint),
                         FanKit.Transformers.XML.SaveVector2("LinearGradientEndPoint", points.LinearGradientEndPoint)
                   );
                case GradientBrushType.Radial:
                    return new XElement
                    (
                         elementName,
                         FanKit.Transformers.XML.SaveVector2("RadialGradientCenter", points.RadialGradientCenter),
                         FanKit.Transformers.XML.SaveVector2("RadialGradientPoint", points.RadialGradientPoint)
                    );
                default: //Elliptical
                    return new XElement
                    (
                         elementName,
                         FanKit.Transformers.XML.SaveVector2("EllipticalGradientCenter", points.EllipticalGradientCenter),
                         FanKit.Transformers.XML.SaveVector2("EllipticalGradientXPoint", points.EllipticalGradientXPoint),
                         FanKit.Transformers.XML.SaveVector2("EllipticalGradientYPoint", points.EllipticalGradientYPoint)
                    );
            }
        }

        /// <summary>
        ///  Loads a <see cref="BrushPoints"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <param name="type"> The gradient type. </param>
        /// <returns> The loaded <see cref="BrushPoints"/>. </returns>
        private static BrushPoints LoadBrushPoints(XElement element, GradientBrushType type)
        {
            BrushPoints points = new BrushPoints();

            switch (type)
            {
                case GradientBrushType.Linear:
                    if (element.Element("LinearGradientStartPoint") is XElement lgsp) points.LinearGradientStartPoint = FanKit.Transformers.XML.LoadVector2(lgsp);
                    if (element.Element("LinearGradientEndPoint") is XElement lgep) points.LinearGradientEndPoint = FanKit.Transformers.XML.LoadVector2(lgep);
                    break;
                case GradientBrushType.Radial:
                    if (element.Element("RadialGradientCenter") is XElement rgc) points.RadialGradientCenter = FanKit.Transformers.XML.LoadVector2(rgc);
                    if (element.Element("RadialGradientPoint") is XElement rgp) points.RadialGradientPoint = FanKit.Transformers.XML.LoadVector2(rgp);
                    break;
                default: //Elliptical
                    if (element.Element("EllipticalGradientCenter") is XElement egc) points.EllipticalGradientCenter = FanKit.Transformers.XML.LoadVector2(egc);
                    if (element.Element("EllipticalGradientXPoint") is XElement egxp) points.EllipticalGradientXPoint = FanKit.Transformers.XML.LoadVector2(egxp);
                    if (element.Element("EllipticalGradientYPoint") is XElement egyp) points.EllipticalGradientYPoint = FanKit.Transformers.XML.LoadVector2(egyp);
                    break;
            }

            return points;
        }

    }
}