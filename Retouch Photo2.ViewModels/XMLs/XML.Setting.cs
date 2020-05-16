using Retouch_Photo2.Menus;
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
        /// Saves the entire <see cref="Setting"/> to a XDocument.
        /// </summary>
        /// <param name="setting"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveSetting(Setting setting)
        {
            return new XDocument
            (
                //Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                     new XElement("Theme", setting.Theme),
                     Retouch_Photo2.Elements.XML.SaveDeviceLayout("DeviceLayout", setting.DeviceLayout),
                     new XElement("MenuTypes", 
                     (
                         from menuType
                         in setting.MenuTypes
                         select new XElement("MenuType", menuType)
                     ))
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Setting"/> from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="Setting"/>. </returns>
        public static Setting LoadSetting(XDocument document)
        {
            XElement root = document.Element("Root");

            Setting setting = new Setting();

            if (root.Element("Theme") is XElement theme) setting.Theme = Retouch_Photo2.Elements.XML.CreateTheme(theme.Value);
            if (root.Element("DeviceLayout") is XElement deviceLayout) setting.DeviceLayout = Retouch_Photo2.Elements.XML.LoadDeviceLayout(deviceLayout);
            if (root.Element("MenuTypes") is XElement menuTypes)
            {
                setting.MenuTypes =
                (
                    from menuType
                    in menuTypes.Elements()
                    select XML.CreateMenuType(menuType.Value.ToString())
                ).ToList();
            }

            return setting;
        }

    }
}