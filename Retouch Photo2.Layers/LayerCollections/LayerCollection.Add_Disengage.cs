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
        /// Add a layer into children.
        /// </summary>
        /// <param name="currentLayer"> The current layers. </param>
        /// <param name="layer"> The source layer. </param>
        public static void Add(ILayer currentLayer, ILayer layer) => LayerCollection._add(currentLayer, layer, null);

        /// <summary>
        /// Add some layers into children.
        /// </summary>
        /// <param name="currentLayer"> The current layers. </param>
        /// <param name="layers"> The source layers. </param>
        public static void AddRange(ILayer currentLayer, IList<ILayer> layers) => LayerCollection._add(currentLayer, null, layers);

        private static void _add(ILayer currentLayer, ILayer layer, IList<ILayer> layers)
        {
            bool isZero = currentLayer.Children.Count == 0;
            ExpandMode expandMode = isZero ? ExpandMode.Expand : currentLayer.ExpandMode;
            bool isSelected = currentLayer.SelectMode.ToBool();

            if (layer != null)
            {
                layer.Parents = currentLayer;
                currentLayer.Children.Add(layer);

                if (isSelected) layer.SelectMode = SelectMode.ParentsSelected;
            }
            else if (layers != null)
            {
                foreach (ILayer child in layers)
                {
                    child.Parents = currentLayer;
                    currentLayer.Children.Add(child);

                    if (isSelected) child.SelectMode = SelectMode.ParentsSelected;
                }
            }

            if (isZero) currentLayer.SelectMode = SelectMode.Selected;
            currentLayer.ExpandMode = expandMode;
        }


        /// <summary>
        /// As a child, to disengage with your parents.
        /// </summary>
        /// <param name="currentLayer"> The current layers. </param>
        /// <param name="layerCollection"> The layer collection. </param>
        public static void Disengage(ILayer currentLayer, LayerCollection layerCollection)
        {
            if (currentLayer.Parents == null)
            {
                layerCollection.RootLayers.Remove(currentLayer);
            }
            else
            {
                currentLayer.Parents.Children.Remove(currentLayer);

                bool isZero = currentLayer.Parents.Children.Count == 0;
                if (isZero)
                {
                    currentLayer.Parents.ExpandMode = ExpandMode.NoChildren;
                }
            }
        }

    }
}