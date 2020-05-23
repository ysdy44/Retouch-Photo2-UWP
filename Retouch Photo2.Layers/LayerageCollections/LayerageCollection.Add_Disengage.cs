using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

        /// <summary>
        /// Add a current-layerage into source-layerage's children.
        /// </summary>
        /// <param name="currentLayerage"> The current layerage. </param>
        /// <param name="layerage"> The source layerage. </param>
        public static void Add(LayerageCollection layerageCollection, Layerage currentLayerage, Layerage layerage) => LayerageCollection._add(layerageCollection, currentLayerage, layerage, null);

        /// <summary>
        /// Add some layerages into children.
        /// </summary>
        /// <param name="currentLayerage"> The current layerages. </param>
        /// <param name="layerages"> The source layerages. </param>
        public static void AddRange(LayerageCollection layerageCollection, Layerage currentLayerage, IEnumerable<Layerage> layerages) => LayerageCollection._add(layerageCollection, currentLayerage, null, layerages);

        private static void _add(LayerageCollection layerageCollection, Layerage currentLayerage, Layerage layerage, IEnumerable<Layerage> layerages)
        {
            if (layerage != null)
            {
                IList<Layerage> layerageParentsChildren = layerageCollection.GetParentsChildren(layerage);
                layerageParentsChildren.Remove(layerage);
                //Add
                currentLayerage.Children.Add(layerage);
            }
            else if (layerages != null)
            {
                foreach (Layerage child in layerages)
                {
                    IList<Layerage> childParentsChildren = layerageCollection.GetParentsChildren(child);
                    childParentsChildren.Remove(child);
                    //Add
                    currentLayerage.Children.Add(child);
                }
            }
        }


    }
}