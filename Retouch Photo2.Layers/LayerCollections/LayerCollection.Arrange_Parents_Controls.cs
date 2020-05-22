using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        static int Index = 0;
        /// <summary>
        /// Arrange all layers's control, depth, parents and expand.
        /// </summary>
        public static void ArrangeLayersControls(LayerCollection layerCollection)
        {
            layerCollection.RootControls.Clear();
            LayerCollection.Index = 0;
            LayerCollection._arrangeLayersControls(layerCollection, layerCollection.RootLayers, 0, null, Visibility.Visible);
        }
        private static void _arrangeLayersControls(LayerCollection layerCollection, IList<Layerage> layers, int depth, Layerage parents,Visibility visibility)
        {
            foreach (Layerage layerage in layers)
            {
                ILayer layer = layerage.Self;
                
                //Depth
                layer.Control.Depth = depth;
                LayerCollection.Index++;
                //Parents
                layerage.Parents = parents;
                //IsExpand
                layer.Control.Self.Visibility = visibility;
                Visibility childVisibility = (layer.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                //IsExpand
                bool isZero = layerage.Children.Count == 0;
                layer.Control.SetChildrenZero(isZero);

                layerCollection.RootControls.Add(layer.Control.Self);

                //Recursive
                LayerCollection._arrangeLayersControls(layerCollection, layerage.Children, depth + 1, layerage, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's parents.
        /// </summary>
        public static void ArrangeLayersParents(LayerCollection layerCollection) 
        {
            foreach (Layerage layer in layerCollection.RootLayers)
            {
                layer.Parents = null;
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }
        private static void _arrangeLayersParents(Layerage parents, IEnumerable<Layerage> layers)
        {
            foreach (Layerage layer in layers)
            {
                layer.Parents = parents;
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }



        /// <summary>
        /// Arrange all layers's depth.
        /// </summary>
        public static void ArrangeLayersDepth(LayerCollection layerCollection) => LayerCollection._arrangeLayersDepth(layerCollection.RootLayers, 0);
        private static void _arrangeLayersDepth(IEnumerable<Layerage> layers, int depth)
        {
            foreach (Layerage layer in layers)
            {
                layer.Self.Control.Depth = depth;
                LayerCollection._arrangeLayersDepth(layer.Children, depth + 1);
            }
        }



        /// <summary>
        /// Arrange all layers's background.
        /// </summary>
        public static void ArrangeLayersBackgroundLayerCollection(LayerCollection layerCollection)
        {
            foreach (Layerage layer in layerCollection.RootLayers)
            {
                LayerCollection._arrangeLayersBackgroundNullParents(layer);
            }
        }
        public static void ArrangeLayersBackgroundItemClick(Layerage layer)
        {
            bool hasParentsSelected = LayerCollection._getLayersParentsIsSelected(layer);
            if (hasParentsSelected) return;
            else LayerCollection._arrangeLayersBackgroundNullParents(layer);
        }

        //Judge Recursive
        private static void _arrangeLayersBackgroundNullParents(Layerage layer)
        {
            if (layer.Self.IsSelected)
            {
                layer.Self.Control.SetBackground(BackgroundMode.Selected);
                foreach (Layerage child in layer.Children)
                {
                    LayerCollection._arrangeLayersBackgroundHasParentsSelected(child);
                }
            }
            else
            {
                bool childrenIsSelected = LayerCollection._getLayersChildrenIsSelected(layer);
                if (childrenIsSelected)
                    LayerCollection._arrangeLayersBackgroundWithoutParentsSelected(layer);
                else
                    LayerCollection._arrangeLayersBackgroundIsNotSelected(layer);
            }
        }
        private static void _arrangeLayersBackgroundWithoutParentsSelected(Layerage layer)
        {
            layer.Self.Control.SetBackground(BackgroundMode.ChildSelected);

            foreach (Layerage child in layer.Children)
            {
                LayerCollection._arrangeLayersBackgroundNullParents(child);
            }
        }

        //Self Recursive
        private static void _arrangeLayersBackgroundHasParentsSelected(Layerage layer)
        {
            layer.Self.Control.SetBackground(BackgroundMode.ParentsSelected);
            foreach (Layerage child in layer.Children)
            {
                LayerCollection._arrangeLayersBackgroundHasParentsSelected(child);
            }
        }
        private static void _arrangeLayersBackgroundIsNotSelected(Layerage layer)
        {
            layer.Self.Control.SetBackground(BackgroundMode.UnSelected);
            foreach (Layerage child in layer.Children)
            {
                LayerCollection._arrangeLayersBackgroundIsNotSelected(child);
            }
        }


        private static bool _getLayersChildrenIsSelected(Layerage layer)
        {
            foreach (Layerage child in layer.Children)
            {
                if (child.Self.IsSelected) return true;

                bool childrenIsSelected = LayerCollection._getLayersChildrenIsSelected(child);
                if (childrenIsSelected == true) return true;
            }
            return false;
        }
        private static bool _getLayersParentsIsSelected(Layerage layer)
        {
            if (layer.Parents == null) return false;
            if (layer.Parents.Self.IsSelected == true) return true;

            return LayerCollection._getLayersParentsIsSelected(layer.Parents);
        }



        /// <summary>
        /// Arrange all layers's visibility.
        /// </summary>
        public static void ArrangeLayersVisibility(Layerage layer)
        {
            Visibility childVisibility = layer.Self.IsExpand ? Visibility.Visible : Visibility.Collapsed;
            LayerCollection._arrangeLayersExpaned(layer.Children, childVisibility);
        }
        private static void _arrangeLayersExpaned(IList<Layerage> layers, Visibility visibility)
        {
            foreach (Layerage layer in layers)
            {
                layer.Self.Control.Self.Visibility = visibility;

                //Recursive
                Visibility childVisibility = (layer.Self.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                LayerCollection._arrangeLayersExpaned(layer.Children, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's "children is zero".
        /// </summary>
        public static void ArrangeLayersChildrenZero(LayerCollection layerCollection) => LayerCollection._arrangeLayersChildrenZero(layerCollection.RootLayers);
        public static void _arrangeLayersChildrenZero(IEnumerable<Layerage> layers)
        {
            foreach (Layerage child in layers)
            {
                bool isZero = child.Children.Count == 0;
                child.Self.Control.SetChildrenZero(isZero);

                LayerCollection._arrangeLayersChildrenZero(child.Children);
            }
        }


    }
}