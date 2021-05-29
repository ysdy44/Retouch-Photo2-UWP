using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Remove a layerage.
        /// </summary>      
        /// <param name="removeLayerage"> The remove Layerage. </param>
        public static void Remove(Layerage removeLayerage)
        {
            Layerage parents = LayerManager.GetParentsChildren(removeLayerage);

            parents.Children.Remove(removeLayerage);
        }

        /// <summary>
        /// Remove all selected layerages.
        /// </summary>
        public static void RemoveAllSelected() => LayerManager.RemoveAllSelectedCore(LayerManager.RootLayerage);


        private static void RemoveAllSelectedCore(Layerage layerage)
        {
            foreach (Layerage child in layerage.Children)
            {
                ILayer layer = child.Self;

                // Recursive
                if (layer.IsSelected == true)
                {
                    // Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    child.RefactoringParentsTransformer();
                    child.RefactoringParentsRender();
                    child.RefactoringParentsIconRender();
                    LayerManager.RemoveAllCore(child);
                }
                // Recursive
                else
                    LayerManager.RemoveAllSelectedCore(child);
            }

            // Remove
            Layerage removeLayerage = null;
            do
            {
                layerage.Children.Remove(removeLayerage);

                removeLayerage = layerage.Children.FirstOrDefault(l => l.Self.IsSelected == true);
            }
            while (removeLayerage != null);
        }

        private static void RemoveAllCore(Layerage layerage)
        {
            foreach (Layerage child in layerage.Children)
            {
                ILayer layer = child.Self;

                // Recursive
                LayerManager.RemoveAllCore(child);

                LayerManager.RootStackPanel.Children.Remove(layer.Control);
            }
            layerage.Children.Clear();
        }


    }
}