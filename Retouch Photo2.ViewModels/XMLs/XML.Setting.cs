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
                     new XElement("LayersHeight", setting.LayersHeight),
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
            if (document.Element("Root") is XElement root)
            {
                Setting setting = new Setting();

                if (root.Element("Theme") is XElement theme) setting.Theme = Retouch_Photo2.Elements.XML.CreateTheme(theme.Value);
                if (root.Element("DeviceLayout") is XElement deviceLayout) setting.DeviceLayout = Retouch_Photo2.Elements.XML.LoadDeviceLayout(deviceLayout);
                if (root.Element("LayersHeight") is XElement layersHeight) setting.LayersHeight = (int)layersHeight;
                if (root.Element("MenuTypes") is XElement menuTypes)
                {
                    if (menuTypes.Elements("MenuType") is IEnumerable<XElement> menuTypes2)
                    {
                        setting.MenuTypes =
                        (
                            from menuType
                            in menuTypes2
                            select Retouch_Photo2.Menus.XML.CreateMenuType(menuType.Value.ToString())
                        ).ToList();
                    }
                }

                return setting;
            }

            return null;
        }

    }
}