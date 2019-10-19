using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {
        //@Static
        /// <summary>
        /// Saves the entire ILayer to a XElement.
        /// </summary>
        /// <param name="layer"> The source ILayer. </param>
        /// <returns> The saved XElement. </returns>
        public static void SaveWidth(XElement element, ILayer layer)
        {
            element.Add(new XElement("LayerType", layer.Type));

            element.Add(new XElement("LayerName", layer.Name));
            element.Add(new XElement("LayerVisual", layer.Visibility));
            element.Add(new XElement("LayerOpacity", layer.Opacity));
            element.Add(new XElement("LayerBlendIndex", layer.BlendType));

            element.Add(TransformManager.Save(layer.TransformManager));
        }
        /// <summary>
        ///  Loads a ILayer from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded ILayer. </returns>
      /*
         public static ILayer(XElement element)
        {
            return new LayerBase
            {
                Source = TransformManager.Load22222(element.Element("Source")),
                Destination = TransformManager.Load22222(element.Element("Destination")),
                DisabledRadian = (bool)element.Element("DisabledRadian"),

                IsCrop = (bool)element.Element("IsCrop"),
                CropDestination = TransformManager.Load22222(element.Element("CropDestination")),
            };
        }
        */

    }
}