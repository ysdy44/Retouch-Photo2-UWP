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
                //IsExpand
                bool isZero = layerage.Children.Count == 0;
                layer.Control.SetChildrenZero(isZero);

                layerageCollection.RootControls.Add(layer.Control.Self);

                //Recursive
                LayerageCollection._arrangeLayers(layerageCollection, layerage, layerage.Children, depth + 1, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's parents.
        /// </summary>
        public static void ArrangeLayersParents(LayerageCollection layerageCollection) 
        {
            foreach (Layerage layer in layerageCollection.RootLayerages)
            {
                layer.Parents = null;
                LayerageCollection._arrangeLayersParents(layer, layer.Children);
            }
        }
        private static void _arrangeLayersParents(Layerage parents, IEnumerable<Layerage> layers)
        {
            foreach (Layerage layer in layers)
            {
                layer.Parents = parents;
                LayerageCollection._arrangeLayersParents(layer, layer.Children);
            }
        }



        /// <summary>
        /// Arrange all layers's depth.
        /// </summary>
        public static void ArrangeLayersDepth(LayerageCollection layerageCollection) => LayerageCollection._arrangeLayersDepth(layerageCollection.RootLayerages, 0);
        private static void _arrangeLayersDepth(IEnumerable<Layerage> layers, int depth)
        {
            foreach (Layerage layer in layers)
            {
                layer.Self.Control.Depth = depth;
                LayerageCollection._arrangeLayersDepth(layer.Children, depth + 1);
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
            foreach (Layerage layer in layerages)
            {
                layer.Self.Control.Self.Visibility = visibility;

                //Recursive
                Visibility childVisibility = (layer.Self.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                LayerageCollection._arrangeLayersExpaned(layer.Children, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's "children is zero".
        /// </summary>
        public static void ArrangeLayersChildrenZero(LayerageCollection layerageCollection) => LayerageCollection._arrangeLayersChildrenZero(layerageCollection.RootLayerages);
        public static void _arrangeLayersChildrenZero(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage child in layerages)
            {
                bool isZero = child.Children.Count == 0;
                child.Self.Control.SetChildrenZero(isZero);

                LayerageCollection._arrangeLayersChildrenZero(child.Children);
            }
        }


    }
}