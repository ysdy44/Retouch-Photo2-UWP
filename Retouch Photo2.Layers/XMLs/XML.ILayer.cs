using Retouch_Photo2.Blends;
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
            layer.SaveWith(element);
            {
                element.Add(new XElement("Type", layer.Type));

                element.Add(new XElement("Name", layer.Name));
                element.Add(new XElement("Opacity", layer.Opacity));
                element.Add(new XElement("BlendType", layer.BlendType));

                element.Add(new XElement("Visibility", layer.Visibility));
                element.Add(new XElement("TagType", layer.TagType));

                element.Add(XML.SaveTransformManager("TransformManager", layer.TransformManager));
                element.Add(Retouch_Photo2.Effects.XML.SaveEffectManager("EffectManager", layer.EffectManager));
                element.Add(Retouch_Photo2.Adjustments.XML.SaveAdjustmentManager("AdjustmentManager", layer.AdjustmentManager));
                element.Add(Retouch_Photo2.Brushs.XML.SaveStyleManager("StyleManager", layer.StyleManager));

                element.Add(new XElement
                (
                    "Children",
                    from child
                    in layer.Children
                    select XML.SaveILayer(elementName, child)
                ));
            }
            return element;
        }

        /// <summary>
        ///  Loads a <see cref="ILayer"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="ILayer"/>. </returns>
        public static ILayer LoadILayer(XElement element)
        {
            string type = element.Element("Type").Value;

            //SaveWith
            ILayer layer = XML.CreateLayer(type, element);
            {
                //layer.Type = element.Element("Type").Value;
                layer.Name = element.Element("Name").Value;
                layer.Opacity = (float)element.Element("Opacity");
                layer.BlendType = Retouch_Photo2.Blends.XML.CreateBlendType(element.Element("BlendType").Value);

                layer.Visibility = XML.CreateVisibility(element.Element("Visibility").Value);
                layer.TagType = Retouch_Photo2.Blends.XML.CreateTagType(element.Element("TagType").Value);

                layer.TransformManager = XML.LoadTransformManager(element.Element("TransformManager"));
                layer.EffectManager = Retouch_Photo2.Effects.XML.LoadEffectManager(element.Element("EffectManager"));
                layer.AdjustmentManager = Retouch_Photo2.Adjustments.XML.LoadAdjustmentManager(element.Element("AdjustmentManager"));
                layer.StyleManager = Retouch_Photo2.Brushs.XML.LoadStyleManager(element.Element("StyleManager"));

                XElement children = element.Element("Children");
                foreach (XElement child in children.Elements())
                {
                    layer.Children.Add(XML.LoadILayer(child));
                }
            }
            return layer;
        }

    }
}