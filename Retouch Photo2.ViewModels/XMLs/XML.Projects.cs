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
        /// Saves the entire <see cref="Project"/>s to a XDocument.
        /// </summary>
        /// <param name="projects"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveProjects(IEnumerable<Project> projects)
        {
            return new XDocument
            (
                // Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                    from project
                    in projects
                    select XML.SaveProjectCore("Project", project)
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Project"/>s from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="Project"/>s. </returns>
        public static IEnumerable<Project> LoadProjects(XDocument document)
        {
            if (document.Element("Root") is XElement root)
            {
                if (root.Elements("Project") is IEnumerable<XElement> projects)
                {
                    return
                        from project
                        in projects
                        select XML.LoadProjectCore(project);
                }
            }

            return null;
        }

    }
}