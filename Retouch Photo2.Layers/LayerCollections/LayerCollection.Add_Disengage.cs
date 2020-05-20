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
        public static void Add(ILayer currentLayer, ILayer layer) => LayerCollection._add(currentLayer, layer, null);

        /// <summary>
        /// Add some layers into children.
        /// </summary>
        /// <param name="currentLayer"> The current layers. </param>
        /// <param name="layers"> The source layers. </param>
        public static void AddRange(ILayer currentLayer, IEnumerable<ILayer> layers) => LayerCollection._add(currentLayer, null, layers);

        private static void _add(ILayer currentLayer, ILayer layer, IEnumerable<ILayer> layers)
        {
            if (layer != null)
            {
                currentLayer.Children.Add(layer);
            }
            else if (layers != null)
            {
                foreach (ILayer child in layers)
                {
                    currentLayer.Children.Add(child);
                }
            }
        }


    }
}