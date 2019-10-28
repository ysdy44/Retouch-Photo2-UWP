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
            switch (type)
            {
                case GradientBrushType.Linear:
                    return new BrushPoints
                    {
                        LinearGradientStartPoint = FanKit.Transformers.XML.LoadVector2(element.Element("LinearGradientStartPoint")),
                        LinearGradientEndPoint = FanKit.Transformers.XML.LoadVector2(element.Element("LinearGradientEndPoint"))
                    };
                case GradientBrushType.Radial:
                    return new BrushPoints
                    {
                        RadialGradientCenter = FanKit.Transformers.XML.LoadVector2(element.Element("RadialGradientCenter")),
                        RadialGradientPoint = FanKit.Transformers.XML.LoadVector2(element.Element("RadialGradientPoint"))
                    };
                default: //Elliptical
                    return new BrushPoints
                    {
                        EllipticalGradientCenter = FanKit.Transformers.XML.LoadVector2(element.Element("EllipticalGradientCenter")),
                        EllipticalGradientXPoint = FanKit.Transformers.XML.LoadVector2(element.Element("EllipticalGradientXPoint")),
                        EllipticalGradientYPoint = FanKit.Transformers.XML.LoadVector2(element.Element("EllipticalGradientYPoint"))
                    };
            }
        }

    }
}