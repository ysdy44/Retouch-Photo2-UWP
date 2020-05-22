using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Add a current-layer into source-layer's children.
        /// </summary>
        /// <param name="currentLayer"> The current layers. </param>
        /// <param name="layer"> The source layer. </param>
        public static void Add(LayerCollection layerCollection, Layerage currentLayer, Layerage layer) => LayerCollection._add(layerCollection, currentLayer, layer, null);

        /// <summary>
        /// Add some layers into children.
        /// </summary>
        /// <param name="currentLayer"> The current layers. </param>
        /// <param name="layers"> The source layers. </param>
        public static void AddRange(LayerCollection layerCollection, Layerage currentLayer, IEnumerable<Layerage> layers) => LayerCollection._add(layerCollection, currentLayer, null, layers);

        private static void _add(LayerCollection layerCollection, Layerage currentLayer, Layerage layer, IEnumerable<Layerage> layers)
        {
            if (layer != null)
            {
                IList<Layerage> layerParentsChildren = layerCollection.GetParentsChildren(layer);
                layerParentsChildren.Remove(layer);
                //Add
                currentLayer.Children.Add(layer);
            }
            else if (layers != null)
            {
                foreach (Layerage child in layers)
                {
                    IList<Layerage> childParentsChildren = layerCollection.GetParentsChildren(child);
                    childParentsChildren.Remove(child);
                    //Add
                    currentLayer.Children.Add(child);
                }
            }
        }


    }
}