using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers.Models;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Layerage"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="layerage"> The destination <see cref="Layerage"/>. </param>
        public static XElement SaveLayerage(string elementName, Layerage layerage)
        {
            XElement element = new XElement(elementName);

            //SaveWith
            {
                element.Add(new XAttribute("Id", layerage.Id));

                if (layerage.Children.Count != 0)
                {
                    element.Add(new XElement
                    (
                        "Children",
                        from child
                        in layerage.Children
                        select XML.SaveLayerage(elementName, child)
                    ));
                }
            }
            return element;
        }

        /// <summary>
        ///  Loads a <see cref="Layerage"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Layerage"/>. </returns>
        public static Layerage LoadLayerage(XElement element)
        {
            Layerage layerage = new Layerage();
            if (element.Attribute("Id") is XAttribute id) layerage.Id = id.Value;

            if (element.Element("Children") is XElement children)
            {
                layerage.Children =
                (
                    from child
                    in children.Elements()
                    select XML.LoadLayerage(child)
                ).ToList();
            }

            return layerage;
        }

    }
}