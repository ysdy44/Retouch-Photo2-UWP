using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerageCollection
    {

        /// <summary>
        /// Arrange all layers's control, depth, parents and expand.
        /// </summary>
        public static void ArrangeLayers()
        {
            LayerageCollection.StackPanel.Children.Clear();

            int depth = -1;
            bool isExpand = true;

            LayerageCollection._arrangeLayers(LayerageCollection.Layerage, depth, isExpand);
        }
        private static void _arrangeLayers(Layerage layerage, int depth, bool isExpand)
        {
            foreach (Layerage child in layerage.Children)
            {
                child.Parents = layerage;

                ILayer layer = child.Self;

                int childDepth = depth + 1;
                bool childIsExpand = layer.IsExpand && isExpand;

                layer.Control.Depth = childDepth;
                layer.Control.Visibility = childIsExpand ? Visibility.Visible : Visibility.Collapsed;
                layer.Control.ChildrenCount = layerage.Children.Count;
                LayerageCollection.StackPanel.Children.Add(layer.Control);

                LayerageCollection._arrangeLayers(child, childDepth, childIsExpand);
            }
        }
         

        /// <summary>
        /// Arrange all layers's IsExpand.
        /// </summary>
        public static void ArrangeLayersIsExpand(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            bool isExpand = layer.IsExpand;
            LayerageCollection._arrangeLayersIsExpand(layerage, isExpand);
        }
        private static void _arrangeLayersIsExpand(Layerage layerage, bool isExpand)
        {
            foreach (Layerage child in layerage.Children)
            {
                ILayer layer = child.Self;

                bool childIsExpand = layer.IsExpand && isExpand;
                layer.Control.Visibility = childIsExpand ? Visibility.Visible : Visibility.Collapsed;

                LayerageCollection._arrangeLayersIsExpand(child, childIsExpand);
            }
        } 

    }
}