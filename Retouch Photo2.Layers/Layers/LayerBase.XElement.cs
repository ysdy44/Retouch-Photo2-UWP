using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using System;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        /// <summary>
        /// Copy a layer with self.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="source"> The source ILayer. </param>
        /// <param name="destination"> The destination ILayer. </param>
        public static void CopyWith(ICanvasResourceCreator resourceCreator, ILayer destination, ILayer source)
        {
            destination.Name = source.Name;
            destination.Opacity = source.Opacity;
            destination.BlendType = source.BlendType;
            destination.Visibility = source.Visibility;

            destination.TransformManager = source.TransformManager.Clone();
            destination.EffectManager = source.EffectManager.Clone();
            foreach (IAdjustment adjustment in source.AdjustmentManager.Adjustments)
            {
                IAdjustment clone = adjustment.Clone();
                destination.AdjustmentManager.Adjustments.Add(clone);
            }

            foreach (ILayer layer in source.Children)
            {
                ILayer clone = layer.Clone(resourceCreator);
                destination.Children.Add(clone);
            }
        }


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
            element.Add(new XElement("LayerBlendType", layer.BlendType));

            element.Add(TransformManager.Save(layer.TransformManager));
        }

        /// <summary>
        ///  Loads a ILayer from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded ILayer. </returns>
        public static void LoadWith(XElement element, ILayer layer)
        {
            layer.Name = element.Element("LayerName").Value;

            var visible = element.Element("LayerVisual").Value;
            switch (visible)
            {
                case "Visible": layer.Visibility = Visibility.Visible; break;
                case "Collapsed": layer.Visibility = Visibility.Collapsed; break;
            }

            layer.Opacity = (float)element.Element("LayerOpacity");

            var blendType2 = element.Element("LayerBlendType").ToString();
            foreach (BlendType blendType in Enum.GetValues(typeof(BlendType)))
            {
                if (blendType2 == blendType.ToString())
                {
                    layer.BlendType = blendType;
                    break;
                }
            }

            var transformManager = element.Element("TransformManager");
            layer.TransformManager = TransformManager.Load(transformManager);
        }

    }
}