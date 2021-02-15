namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Arrange all layers's background.
        /// </summary>
        public static void ArrangeLayersBackground()
        {
            foreach (Layerage child in LayerManager.RootLayerage.Children)
            {
                LayerManager._arrangeLayersBackgroundNullParents(child);
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
                    LayerManager._arrangeLayersBackgroundHasParentsSelected(child);
                }
            }
            else
            {
                bool childrenIsSelected = LayerManager._getLayersChildrenIsSelected(layerage);
                if (childrenIsSelected)
                    LayerManager._arrangeLayersBackgroundWithoutParentsSelected(layerage);
                else
                    LayerManager._arrangeLayersBackgroundIsNotSelected(layerage);
            }
        }
        private static void _arrangeLayersBackgroundWithoutParentsSelected(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.ChildSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerManager._arrangeLayersBackgroundNullParents(child);
            }
        }

        //Self Recursive
        private static void _arrangeLayersBackgroundHasParentsSelected(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.ParentsSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerManager._arrangeLayersBackgroundHasParentsSelected(child);
            }
        }
        private static void _arrangeLayersBackgroundIsNotSelected(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.UnSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerManager._arrangeLayersBackgroundIsNotSelected(child);
            }
        }


        private static bool _getLayersChildrenIsSelected(Layerage layerage)
        {
            foreach (Layerage child in layerage.Children)
            {
                if (child.Self.IsSelected) return true;

                bool childrenIsSelected = LayerManager._getLayersChildrenIsSelected(child);
                if (childrenIsSelected == true) return true;
            }
            return false;
        }
        private static bool _getLayersParentsIsSelected(Layerage layerage)
        {
            if (layerage.Parents == LayerManager.RootLayerage) return false;
            if (layerage.Parents.Self.IsSelected == true) return true;

            return LayerManager._getLayersParentsIsSelected(layerage.Parents);
        }


    }
}
