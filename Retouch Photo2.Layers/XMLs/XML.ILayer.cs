using Microsoft.Graphics.Canvas;
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
        /// Saves the entire <see cref="ILayer"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="layer"> The destination <see cref="ILayer"/>. </param>
        public static XElement SaveILayer(string elementName, ILayer layer)
        {
            XElement element = new XElement(elementName);

            //SaveWith
            {
                element.Add(new XAttribute("Id", layer.Id));
                element.Add(new XAttribute("Type", layer.Type));
                if (layer.Name != null) element.Add(new XAttribute("Name", layer.Name));
                element.Add(new XAttribute("Opacity", layer.Opacity));
                element.Add(new XAttribute("BlendMode", layer.BlendMode == null ? "None" : $"{layer.BlendMode}"));

                element.Add(new XAttribute("Visibility", layer.Visibility));
                element.Add(new XAttribute("TagType", layer.TagType));

                element.Add(new XAttribute("IsExpand", layer.IsExpand));
                element.Add(new XAttribute("IsSelected", layer.IsSelected));

                layer.SaveWith(element);

                element.Add(Retouch_Photo2.Styles.XML.SaveStyle("Style", layer.Style));
                element.Add(Retouch_Photo2.Layers.XML.SaveTransform("Transform", layer.Transform));
                element.Add(Retouch_Photo2.Effects.XML.SaveEffect("Effect", layer.Effect));
                element.Add(Retouch_Photo2.Filters.XML.SaveFilter("Filter", layer.Filter));
            }
            return element;
        }

        /// <summary>
        ///  Loads a <see cref="ILayer"/> from an XElement.
        /// </summary>   
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="ILayer"/>. </returns>
        public static ILayer LoadILayer(CanvasDevice customDevice, XElement element)
        {
            if (element.Attribute("Type") is XAttribute type2)
            {
                string type = type2.Value;

                //Load
                ILayer layer = XML.CreateLayer(customDevice, type);
                {
                    if (element.Attribute("Id") is XAttribute id) layer.Id = id.Value;
                    //if (element.Attribute("Type") is XAttribute type) layer.Type = type.Value;
                    if (element.Attribute("Name") is XAttribute name) layer.Name = name.Value;
                    if (element.Attribute("Opacity") is XAttribute opacity) layer.Opacity = (float)opacity;
                    if (element.Attribute("BlendMode") is XAttribute blendMode) layer.BlendMode = Retouch_Photo2.Blends.XML.CreateBlendMode(blendMode.Value);

                    if (element.Attribute("Visibility") is XAttribute visibility) layer.Visibility = XML.CreateVisibility(visibility.Value);
                    if (element.Attribute("TagType") is XAttribute tagType) layer.TagType = Retouch_Photo2.Blends.XML.CreateTagType(tagType.Value);

                    if (element.Attribute("IsExpand") is XAttribute isExpand) layer.IsExpand = (bool)isExpand;
                    if (element.Attribute("IsSelected") is XAttribute isSelected) layer.IsSelected = (bool)isSelected;
                    
                    layer.Load(element);

                    if (element.Element("Style") is XElement style) layer.Style = Retouch_Photo2.Styles.XML.LoadStyle(style);
                    if (element.Element("Transform") is XElement transform) layer.Transform = Retouch_Photo2.Layers.XML.LoadTransform(transform);
                    if (element.Element("Effect") is XElement effect) layer.Effect = Retouch_Photo2.Effects.XML.LoadEffect(effect);
                    if (element.Element("Filter") is XElement filter) layer.Filter = Retouch_Photo2.Filters.XML.LoadFilter(filter);
                }
                return layer;
            }
            else return new GroupLayer(customDevice);
        }

    }
}