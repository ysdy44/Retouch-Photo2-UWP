namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerageCollection
    {

        /// <summary>
        /// Arrange all layers's background.
        /// </summary>
        public static void ArrangeLayersBackground()
        {
            foreach (Layerage child in LayerageCollection.Layerage.Children)
            {
                LayerageCollection._arrangeLayersBackgroundNullParents(child);
            }
        }
        //public static void ArrangeLayersBackgroundItemClick(Layerage layerage)
        //{
        //    bool hasParentsSelected = LayerManager._getLayersParentsIsSelected(layerage);
        //    if (hasParentsSelected) return;
        //    else LayerManager._arrangeLayersBackgroundNullParents(layerage);
        //}

        //Judge Recursive
        private static void _arrangeLayersBackgroundNullParents(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            if (layer.IsSelected)
            {
                layer.Control.BackgroundMode = BackgroundMode.Selected;
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
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.ChildSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerageCollection._arrangeLayersBackgroundNullParents(child);
            }
        }

        //Self Recursive
        private static void _arrangeLayersBackgroundHasParentsSelected(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.ParentsSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerageCollection._arrangeLayersBackgroundHasParentsSelected(child);
            }
        }
        private static void _arrangeLayersBackgroundIsNotSelected(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.UnSelected;

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
            if (layerage.Parents == LayerageCollection.Layerage) return false;
            if (layerage.Parents.Self.IsSelected == true) return true;

            return LayerageCollection._getLayersParentsIsSelected(layerage.Parents);
        }


    }
}
