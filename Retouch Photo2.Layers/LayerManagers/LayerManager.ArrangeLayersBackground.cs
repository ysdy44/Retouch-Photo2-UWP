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
                LayerManager.ArrangeLayersBackgroundCore(child);
            }
        }
        //public static void ArrangeLayersBackgroundItemClick(Layerage layerage)
        //{
        //    bool hasParentsSelected = LayerManager._getLayersParentsIsSelected(layerage);
        //    if (hasParentsSelected) return;
        //    else LayerManager._arrangeLayersBackgroundNullParents(layerage);
        //}

        //Judge Recursive
        private static void ArrangeLayersBackgroundCore(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            if (layer.IsSelected)
            {
                layer.Control.BackgroundMode = BackgroundMode.Selected;
                foreach (Layerage child in layerage.Children)
                {
                    LayerManager.ArrangeLayersBackgroundHasParentsSelectedCore(child);
                }
            }
            else
            {
                bool childrenIsSelected = LayerManager.GetLayersChildrenIsSelectedCore(layerage);
                if (childrenIsSelected)
                    LayerManager.ArrangeLayersBackgroundWithoutParentsSelectedCore(layerage);
                else
                    LayerManager.ArrangeLayersBackgroundIsNotSelectedCore(layerage);
            }
        }
        private static void ArrangeLayersBackgroundWithoutParentsSelectedCore(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.ChildSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerManager.ArrangeLayersBackgroundCore(child);
            }
        }

        //Self Recursive
        private static void ArrangeLayersBackgroundHasParentsSelectedCore(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.ParentsSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerManager.ArrangeLayersBackgroundHasParentsSelectedCore(child);
            }
        }
        private static void ArrangeLayersBackgroundIsNotSelectedCore(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            layer.Control.BackgroundMode = BackgroundMode.UnSelected;

            foreach (Layerage child in layerage.Children)
            {
                LayerManager.ArrangeLayersBackgroundIsNotSelectedCore(child);
            }
        }


        private static bool GetLayersChildrenIsSelectedCore(Layerage layerage)
        {
            foreach (Layerage child in layerage.Children)
            {
                if (child.Self.IsSelected) return true;

                bool childrenIsSelected = LayerManager.GetLayersChildrenIsSelectedCore(child);
                if (childrenIsSelected == true) return true;
            }
            return false;
        }
        private static bool GetLayersParentsIsSelectedCore(Layerage layerage)
        {
            if (layerage.Parents == LayerManager.RootLayerage) return false;
            if (layerage.Parents.Self.IsSelected == true) return true;

            return LayerManager.GetLayersParentsIsSelectedCore(layerage.Parents);
        }


    }
}
