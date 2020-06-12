using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Arrange all layers's control, depth, parents and expand.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        public static void ArrangeLayers(LayerageCollection layerageCollection)
        {
            layerageCollection.RootControls.Clear();
            LayerageCollection._arrangeLayers(layerageCollection, null, layerageCollection.RootLayerages, 0, Visibility.Visible);
        }
        private static void _arrangeLayers(LayerageCollection layerageCollection, Layerage parents, IList<Layerage> layers, int depth, Visibility visibility)
        {
            foreach (Layerage layerage in layers)
            {
                ILayer layer = layerage.Self;
                
                //Depth
                layer.Control.Depth = depth;
                //Parents
                layerage.Parents = parents;
                //IsExpand
                layer.Control.Self.Visibility = visibility;
                Visibility childVisibility = (layer.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                //Children
                layer.Control.ChildrenCount = layerage.Children.Count;

                layerageCollection.RootControls.Add(layer.Control.Self);

                //Recursive
                LayerageCollection._arrangeLayers(layerageCollection, layerage, layerage.Children, depth + 1, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's parents.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        public static void ArrangeLayersParents(LayerageCollection layerageCollection) 
        {
            foreach (Layerage layerage in layerageCollection.RootLayerages)
            {
                layerage.Parents = null;
                LayerageCollection._arrangeLayersParents(layerage, layerage.Children);
            }
        }
        private static void _arrangeLayersParents(Layerage parents, IEnumerable<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                layerage.Parents = parents;
                LayerageCollection._arrangeLayersParents(layerage, layerage.Children);
            }
        }



        /// <summary>
        /// Arrange all layers's depth.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        public static void ArrangeLayersDepth(LayerageCollection layerageCollection) => LayerageCollection._arrangeLayersDepth(layerageCollection.RootLayerages, 0);
        private static void _arrangeLayersDepth(IEnumerable<Layerage> layerages, int depth)
        {
            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;

                layer.Control.Depth = depth;

                LayerageCollection._arrangeLayersDepth(layerage.Children, depth + 1);
            }
        }

        
        /// <summary>
        /// Arrange all layers's visibility.
        /// </summary>
        public static void ArrangeLayersVisibility(Layerage layerage)
        {
            Visibility childVisibility = layerage.Self.IsExpand ? Visibility.Visible : Visibility.Collapsed;
            LayerageCollection._arrangeLayersExpaned(layerage.Children, childVisibility);
        }
        private static void _arrangeLayersExpaned(IList<Layerage> layerages, Visibility visibility)
        {
            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;

                layer.Control.Self.Visibility = visibility;

                //Recursive
                Visibility childVisibility = (layerage.Self.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                LayerageCollection._arrangeLayersExpaned(layerage.Children, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's "children is zero".
        /// </summary>
        public static void ArrangeLayersChildrenZero(LayerageCollection layerageCollection) => LayerageCollection._arrangeLayersChildrenZero(layerageCollection.RootLayerages);
        private static void _arrangeLayersChildrenZero(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;
                
                layer.Control.ChildrenCount = layerage.Children.Count;

                LayerageCollection._arrangeLayersChildrenZero(layerage.Children);
            }
        }


    }
}