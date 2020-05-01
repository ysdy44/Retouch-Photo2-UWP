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
            {
                element.Add(new XAttribute("Type", layer.Type));
                element.Add(new XAttribute("Name", layer.Name));
                element.Add(new XAttribute("Opacity", layer.Opacity));
                element.Add(new XAttribute("BlendMode", layer.BlendMode == null ? "None" : $"{layer.BlendMode}"));
                element.Add(new XAttribute("Visibility", layer.Visibility));
                element.Add(new XAttribute("TagType", layer.TagType));

                layer.SaveWith(element);

                element.Add(Retouch_Photo2.Brushs.XML.SaveStyleManager("StyleManager", layer.StyleManager));
                element.Add(XML.SaveTransformManager("TransformManager", layer.TransformManager));
                element.Add(Retouch_Photo2.Effects.XML.SaveEffectManager("EffectManager", layer.EffectManager));
                element.Add(Retouch_Photo2.Adjustments.XML.SaveAdjustmentManager("AdjustmentManager", layer.AdjustmentManager));

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
            string type = element.Attribute("Type").Value;

            //Load
            ILayer layer = XML.CreateLayer(type);
            {
                //if (element.Attribute("Type") is XAttribute type) layer.Type = type.Value;
                if (element.Attribute("Name") is XAttribute name) layer.Name = name.Value;
                if (element.Attribute("Opacity") is XAttribute opacity) layer.Opacity = (float)opacity;
                if (element.Attribute("BlendMode") is XAttribute blendMode) layer.BlendMode = Retouch_Photo2.Blends.XML.CreateBlendMode(blendMode.Value);
                if (element.Attribute("Visibility") is XAttribute visibility) layer.Visibility = XML.CreateVisibility(visibility.Value);
                if (element.Attribute("TagType") is XAttribute tagType) layer.TagType = Retouch_Photo2.Blends.XML.CreateTagType(tagType.Value);

                layer.Load(element);

                if (element.Element("StyleManager") is XElement styleManager) layer.StyleManager = Retouch_Photo2.Brushs.XML.LoadStyleManager(styleManager);
                if (element.Element("TransformManager") is XElement transformManager) layer.TransformManager = XML.LoadTransformManager(transformManager);
                if (element.Element("EffectManager") is XElement effectManager) layer.EffectManager = Retouch_Photo2.Effects.XML.LoadEffectManager(effectManager);
                if (element.Element("AdjustmentManager") is XElement adjustmentManager) layer.AdjustmentManager = Retouch_Photo2.Adjustments.XML.LoadAdjustmentManager(adjustmentManager);

                if (element.Element("Children") is XElement children)
                {
                    foreach (XElement child in children.Elements())
                    {
                        layer.Children.Add(XML.LoadILayer(child));
                    }
                }
            }
            return layer;
        }

    }
}