using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

        static int Index = 0;
        /// <summary>
        /// Arrange all layers's control, depth, parents and expand.
        /// </summary>
        public static void ArrangeLayersControls(LayerageCollection layerageCollection)
        {
            layerageCollection.RootControls.Clear();
            LayerageCollection.Index = 0;
            LayerageCollection._arrangeLayersControls(layerageCollection, layerageCollection.RootLayerages, 0, null, Visibility.Visible);
        }
        private static void _arrangeLayersControls(LayerageCollection layerageCollection, IList<Layerage> layers, int depth, Layerage parents,Visibility visibility)
        {
            foreach (Layerage layerage in layers)
            {
                ILayer layer = layerage.Self;
                
                //Depth
                layer.Control.Depth = depth;
                LayerageCollection.Index++;
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
                LayerageCollection._arrangeLayersControls(layerageCollection, layerage.Children, depth + 1, layerage, childVisibility);
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
        /// Arrange all layers's background.
        /// </summary>
        public static void ArrangeLayersBackgroundLayerCollection(LayerageCollection layerageCollection)
        {
            foreach (Layerage layerage in layerageCollection.RootLayerages)
            {
                LayerageCollection._arrangeLayersBackgroundNullParents(layerage);
            }
        }
        public static void ArrangeLayersBackgroundItemClick(Layerage layerage)
        {
            bool hasParentsSelected = LayerageCollection._getLayersParentsIsSelected(layerage);
            if (hasParentsSelected) return;
            else LayerageCollection._arrangeLayersBackgroundNullParents(layerage);
        }

        //Judge Recursive
        private static void _arrangeLayersBackgroundNullParents(Layerage layerage)
        {
            if (layerage.Self.IsSelected)
            {
                layerage.Self.Control.SetBackground(BackgroundMode.Selected);
                foreach (Layerage child in layerage.Children)
                {
                    LayerageCollection._arrangeLayersBackgroundHasParentsSelected(child);
                }
            }
            else
            {
                bool childrenIsSelected = LayerageCollection._getLayersChildrenIsSelected(layerage);
                if (childrenIsSelected)
                    LayerageCollection._arrangeLayersBackgroundWithoutParentsSelected(layerage);
                else
                    LayerageCollection._arrangeLayersBackgroundIsNotSelected(layerage);
            }
        }
        private static void _arrangeLayersBackgroundWithoutParentsSelected(Layerage layerage)
        {
            layerage.Self.Control.SetBackground(BackgroundMode.ChildSelected);

            foreach (Layerage child in layerage.Children)
            {
                LayerageCollection._arrangeLayersBackgroundNullParents(child);
            }
        }

        //Self Recursive
        private static void _arrangeLayersBackgroundHasParentsSelected(Layerage layerage)
        {
            layerage.Self.Control.SetBackground(BackgroundMode.ParentsSelected);
            foreach (Layerage child in layerage.Children)
            {
                LayerageCollection._arrangeLayersBackgroundHasParentsSelected(child);
            }
        }
        private static void _arrangeLayersBackgroundIsNotSelected(Layerage layerage)
        {
            layerage.Self.Control.SetBackground(BackgroundMode.UnSelected);
            foreach (Layerage child in layerage.Children)
            {
                LayerageCollection._arrangeLayersBackgroundIsNotSelected(child);
            }
        }


        private static bool _getLayersChildrenIsSelected(Layerage layerage)
        {
            foreach (Layerage child in layerage.Children)
            {
                if (child.Self.IsSelected) return true;

                bool childrenIsSelected = LayerageCollection._getLayersChildrenIsSelected(child);
                if (childrenIsSelected == true) return true;
            }
            return false;
        }
        private static bool _getLayersParentsIsSelected(Layerage layerage)
        {
            if (layerage.Parents == null) return false;
            if (layerage.Parents.Self.IsSelected == true) return true;

            return LayerageCollection._getLayersParentsIsSelected(layerage.Parents);
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