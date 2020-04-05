using Retouch_Photo2.ViewModels;
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Project"/> to a XDocument.
        /// </summary>
        /// <param name="project"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveProject(Project project)
        {
            return new XDocument
            (
                //Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                    new XElement("Width", project.Width),
                    new XElement("Height", project.Height),
                    new XElement
                    (
                         "Layers",
                         from layer
                         in project.Layers
                         select Retouch_Photo2.Layers.XML.SaveILayer("Layer", layer)
                    )
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Project"/> from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <param name="name"> The name. </param>
        /// <returns> The loaded <see cref="Project"/>. </returns>
        public static Project LoadProject(string name, XDocument document)
        {
            XElement root = document.Element("Root");
            XElement rootImages = root.Element("Images");
            XElement rootLayers = root.Element("Layers");

            return new Project
            {
                Name = name,
                Width = (int)root.Element("Width"),
                Height = (int)root.Element("Height"),
                Layers =
                    from layer
                    in rootLayers.Elements()
                    select Retouch_Photo2.Layers.XML.LoadILayer(layer)
            };
        }

    }
}