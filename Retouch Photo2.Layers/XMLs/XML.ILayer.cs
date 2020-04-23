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
                element.Add(Retouch_Photo2.Blends.XML.SaveBlendType("BlendType", layer.BlendType));

                element.Add(new XElement("Visibility", layer.Visibility));
                element.Add(new XElement("TagType", layer.TagType));

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
            string type = element.Element("Type").Value;

            //Load
            ILayer layer = XML.CreateLayer(type);
            layer.Load(element);
            {
                //if (element.Element("Type") is XElement type) layer.Type = type.Value;
                if (element.Element("Name") is XElement name) layer.Name = name.Value;
                if (element.Element("Opacity") is XElement opacity) layer.Opacity = (float)opacity;
                if (element.Element("BlendType") is XElement blendType) layer.BlendType = Retouch_Photo2.Blends.XML.CreateBlendType(blendType.Value);

                if (element.Element("Visibility") is XElement visibility) layer.Visibility = XML.CreateVisibility(visibility.Value);
                if (element.Element("TagType") is XElement tagType) layer.TagType = Retouch_Photo2.Blends.XML.CreateTagType(tagType.Value);

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