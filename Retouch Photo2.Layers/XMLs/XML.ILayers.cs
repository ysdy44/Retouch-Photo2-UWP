using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
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
        /// Saves the entire <see cref="LayerBase"/>s to a XDocument.
        /// </summary>
        /// <param name="layers"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveLayers(IEnumerable<ILayer> layers)
        {
            return new XDocument
            (
                //Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                    from layer
                    in layers
                    select XML.SaveILayer("Layer", layer)
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="ILayer"/>s from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="ILayer"/>s. </returns>
        public static IEnumerable<ILayer> LoadLayers(XDocument document)
        {
            XElement root = document.Element("Root");

            return
                from layer
                in root.Elements()
                select XML.LoadILayer(layer);
        }

    }
}