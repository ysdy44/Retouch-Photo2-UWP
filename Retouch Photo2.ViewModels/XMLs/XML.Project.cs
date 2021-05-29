// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Collections.Generic;
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
                // Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                    new XElement("Width", project.Width),
                    new XElement("Height", project.Height),

                    new XElement
                    (
                         "Layerages",
                         from layerage
                         in project.Layerages
                         select Retouch_Photo2.Layers.XML.SaveLayerage("Layerage", layerage)
                    )
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Project"/> from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="Project"/>. </returns>
        public static Project LoadProject(XDocument document)
        {
            if (document.Element("Root") is XElement root)
            {
                Project project = new Project();

                if (root.Element("Layerages") is XElement layerages)
                {
                    if (layerages.Elements("Layerage") is IEnumerable<XElement> layerages2)
                    {
                        project.Layerages =
                            from layerage
                            in layerages2
                            select Retouch_Photo2.Layers.XML.LoadLayerage(layerage);
                    }
                }

                if (root.Element("Width") is XElement width) project.Width = (int)width;
                if (root.Element("Height") is XElement height) project.Height = (int)height;

                return project;
            }

            return null;
        }

    }
}