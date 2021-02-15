using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Arrange all layers's control, depth, parents and expand.
        /// </summary>
        public static void ArrangeLayers()
        {
            LayerManager.RootStackPanel.Children.Clear();

            int depth = 0;
            bool isExpand = true;
            LayerManager._arrangeLayers(LayerManager.RootLayerage, depth, isExpand);
        }
        private static void _arrangeLayers(Layerage layerage, int depth, bool isExpand)
        {
            foreach (Layerage child in layerage.Children)
            {
                child.Parents = layerage;

                ILayer layer = child.Self;

                layer.Control.Depth = depth;
                layer.Control.Visibility = isExpand ? Visibility.Visible : Visibility.Collapsed;
                layer.Control.ChildrenCount = child.Children.Count;
                LayerManager.RootStackPanel.Children.Add(layer.Control);

                int childDepth = depth + 1;
                bool childIsExpand = layer.IsExpand && isExpand;
                LayerManager._arrangeLayers(child, childDepth, childIsExpand);
            }
        }
         

        /// <summary>
        /// Arrange all layers's IsExpand.
        /// </summary>
        public static void ArrangeLayersIsExpand(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            bool isExpand = layer.IsExpand;
            LayerManager._arrangeLayersIsExpand(layerage, isExpand);
        }
        private static void _arrangeLayersIsExpand(Layerage layerage, bool isExpand)
        {
            foreach (Layerage child in layerage.Children)
            {
                ILayer layer = child.Self;

                bool childIsExpand = layer.IsExpand && isExpand;
                layer.Control.Visibility = childIsExpand ? Visibility.Visible : Visibility.Collapsed;

                LayerManager._arrangeLayersIsExpand(child, childIsExpand);
            }
        } 

    }
}