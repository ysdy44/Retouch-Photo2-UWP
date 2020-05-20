using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {


        /// <summary>
        /// Arrange all layers's control, depth, parents and expand.
        /// </summary>
        public static void ArrangeLayersControls(LayerCollection layerCollection)
        {
            layerCollection.RootControls.Clear();

            LayerCollection._arrangeLayersControls(layerCollection, layerCollection.RootLayers, 0, null, Visibility.Visible);
        }
        private static void _arrangeLayersControls(LayerCollection layerCollection, IList<ILayer> layers, int depth, ILayer parents,Visibility visibility)
        {
            foreach (ILayer layer in layers)
            {
                //Depth
                layer.Control.Depth = depth;
                //Parents
                layer.Parents = parents;
                //IsExpand
                layer.Control.Self.Visibility = visibility;
                Visibility childVisibility = (layer.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                //IsExpand
                bool isZero = layer.Children.Count == 0;
                layer.Control.SetChildrenZero(isZero);

                layerCollection.RootControls.Add(layer.Control.Self);

                //Recursive
                LayerCollection._arrangeLayersControls(layerCollection, layer.Children, depth + 1, layer, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's parents.
        /// </summary>
        public static void ArrangeLayersParents(LayerCollection layerCollection) 
        {
            foreach (ILayer layer in layerCollection.RootLayers)
            {
                layer.Parents = null;
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }
        private static void _arrangeLayersParents(ILayer parents, IEnumerable<ILayer> layers)
        {
            foreach (ILayer layer in layers)
            {
                layer.Parents = parents;
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }



        /// <summary>
        /// Arrange all layers's depth.
        /// </summary>
        public static void ArrangeLayersDepth(LayerCollection layerCollection) => LayerCollection._arrangeLayersDepth(layerCollection.RootLayers, 0);
        private static void _arrangeLayersDepth(IEnumerable<ILayer> layers, int depth)
        {
            foreach (ILayer layer in layers)
            {
                layer.Control.Depth = depth;
                LayerCollection._arrangeLayersDepth(layer.Children, depth + 1);
            }
        }



        /// <summary>
        /// Arrange all layers's background.
        /// </summary>
        public static void ArrangeLayersBackgroundLayerCollection(LayerCollection layerCollection)
        {
            foreach (ILayer layer in layerCollection.RootLayers)
            {
                LayerCollection._arrangeLayersBackgroundNullParents(layer);
            }
        }
        public static void ArrangeLayersBackgroundItemClick(ILayer layer)
        {
            bool hasParentsSelected = LayerCollection._getLayersParentsIsSelected(layer);
            if (hasParentsSelected) return;
            else LayerCollection._arrangeLayersBackgroundNullParents(layer);
        }

        //Judge Recursive
        private static void _arrangeLayersBackgroundNullParents(ILayer layer)
        {
            if (layer.IsSelected)
            {
                layer.Control.SetBackground(BackgroundMode.Selected);
                foreach (ILayer child in layer.Children)
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
        private static void _arrangeLayersBackgroundWithoutParentsSelected(ILayer layer)
        {
            layer.Control.SetBackground(BackgroundMode.ChildSelected);

            foreach (ILayer child in layer.Children)
            {
                LayerCollection._arrangeLayersBackgroundNullParents(child);
            }
        }

        //Self Recursive
        private static void _arrangeLayersBackgroundHasParentsSelected(ILayer layer)
        {
            layer.Control.SetBackground(BackgroundMode.ParentsSelected);
            foreach (ILayer child in layer.Children)
            {
                LayerCollection._arrangeLayersBackgroundHasParentsSelected(child);
            }
        }
        private static void _arrangeLayersBackgroundIsNotSelected(ILayer layer)
        {
            layer.Control.SetBackground(BackgroundMode.UnSelected);
            foreach (ILayer child in layer.Children)
            {
                LayerCollection._arrangeLayersBackgroundIsNotSelected(child);
            }
        }


        private static bool _getLayersChildrenIsSelected(ILayer layer)
        {
            foreach (ILayer child in layer.Children)
            {
                if (child.IsSelected) return true;

                bool childrenIsSelected = LayerCollection._getLayersChildrenIsSelected(child);
                if (childrenIsSelected == true) return true;
            }
            return false;
        }
        private static bool _getLayersParentsIsSelected(ILayer layer)
        {
            if (layer.Parents == null) return false;
            if (layer.Parents.IsSelected == true) return true;

            return LayerCollection._getLayersParentsIsSelected(layer.Parents);
        }



        /// <summary>
        /// Arrange all layers's visibility.
        /// </summary>
        public static void ArrangeLayersVisibility(ILayer layer)
        {
            Visibility childVisibility = layer.IsExpand ? Visibility.Visible : Visibility.Collapsed;
            LayerCollection._arrangeLayersExpaned(layer.Children, childVisibility);
        }
        private static void _arrangeLayersExpaned(IList<ILayer> layers, Visibility visibility)
        {
            foreach (ILayer layer in layers)
            {
                layer.Control.Self.Visibility = visibility;

                //Recursive
                Visibility childVisibility = (layer.IsExpand && visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                LayerCollection._arrangeLayersExpaned(layer.Children, childVisibility);
            }
        }



        /// <summary>
        /// Arrange all layers's "children is zero".
        /// </summary>
        public static void ArrangeLayersChildrenZero(LayerCollection layerCollection) => LayerCollection._arrangeLayersChildrenZero(layerCollection.RootLayers);
        public static void _arrangeLayersChildrenZero(IEnumerable<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                bool isZero = child.Children.Count == 0;
                child.Control.SetChildrenZero(isZero);

                LayerCollection._arrangeLayersChildrenZero(child.Children);
            }
        }


    }
}