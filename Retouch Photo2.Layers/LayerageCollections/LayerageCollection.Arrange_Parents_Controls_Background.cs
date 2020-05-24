using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

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
            ILayer layer = layerage.Self;

            if (layer.IsSelected)
            {
                layer.Control.SetBackground(BackgroundMode.Selected);
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


    }
}
