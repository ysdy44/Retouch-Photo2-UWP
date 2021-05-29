using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Drag complete.
        /// </summary>
        /// <param name="destination"> The destination layerage. </param>
        /// <param name="source"> The source layerage. </param>
        /// <param name="destinationOverlayMode"> The destination OverlayMode. </param>
        /// <param name="sourceIsSelected"> The source SelectMode. </param>
        public static void DragComplete(Layerage destination, Layerage source, OverlayMode destinationOverlayMode, bool sourceIsSelected)
        {
            if (source == null) return;
            if (destination == null) return;
            ILayer destinationLayer = destination.Self;
            ILayer sourceLayer = source.Self;

            destinationLayer.Control.OverlayMode = OverlayMode.None;
            if (destinationLayer.IsSelected) return;
            if (destinationOverlayMode == OverlayMode.None) return;

            if (source == destination) return;


            if (sourceIsSelected == false)
            {
                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        LayerManager.Insert(destination, source, isBottomInsert: false);
                        break;
                    case OverlayMode.Center:
                        LayerManager.Add(destination, source);
                        break;
                    case OverlayMode.Bottom:
                        LayerManager.Insert(destination, source, isBottomInsert: true);
                        break;
                }
            }
            else
            {
                // Layerages
                IEnumerable<Layerage> selectedLayerages = LayerManager.GetAllSelected();

                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        LayerManager.InsertRange(destination, selectedLayerages, isBottomInsert: false);
                        break;
                    case OverlayMode.Center:
                        LayerManager.AddRange(destination, selectedLayerages);
                        break;
                    case OverlayMode.Bottom:
                        LayerManager.InsertRange(destination, selectedLayerages, isBottomInsert: true);
                        break;
                }
            }
        }


        #region Move


        /// <summary>
        /// Move a layerage to the top of destination layerage.
        /// </summary>   
        /// <param name="destination"> The destination layerage. </param>
        /// <param name="source"> The source layerage. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void Move(Layerage destination, Layerage source, bool isBottomInsert)
        {
            Layerage parents = LayerManager.GetParentsChildren(destination);
            int index = parents.Children.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parents.Children.Count - 1) index = parents.Children.Count - 1;


            if (source.Parents != LayerManager.RootLayerage)
            {
                // Refactoring
                // ILayer sourceParents = source.Parents.Self;
                //source.Parents.IsRefactoringTransformer = true;
                //source.Parents.IsRefactoringRender = true;
                //source.Parents.IsRefactoringIconRender = true;
                source.Parents.RefactoringParentsTransformer();
                source.Parents.RefactoringParentsRender();
                source.Parents.RefactoringParentsIconRender();
            }

            parents.Children.Remove(source);
            parents.Children.Insert(index, source);


            if (destination.Parents != LayerManager.RootLayerage)
            {
                // Refactoring
                ILayer destinationParents = destination.Parents.Self;
                destinationParents.IsRefactoringTransformer = true;
                destinationParents.IsRefactoringRender = true;
                destinationParents.IsRefactoringIconRender = true;
                destination.Parents.RefactoringParentsTransformer();
                destination.Parents.RefactoringParentsRender();
                destination.Parents.RefactoringParentsIconRender();
            }
        }


        #endregion


        #region Insert


        /// <summary>
        /// Insert a layerage to the top of destination layerage.
        /// </summary>
        /// <param name="destination"> The destination layerage. </param>
        /// <param name="source"> The source layerage. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void Insert(Layerage destination, Layerage source, bool isBottomInsert) => LayerManager.InsertCore(destination, source, null, isBottomInsert);

        /// <summary>
        /// Insert some layers to the top of destination layerage.
        /// </summary>
        /// <param name="destination"> The destination layerage. </param>
        /// <param name="sources"> The source layers. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void InsertRange(Layerage destination, IEnumerable<Layerage> sources, bool isBottomInsert) => LayerManager.InsertCore(destination, null, sources, isBottomInsert);


        private static void InsertCore(Layerage destination, Layerage source, IEnumerable<Layerage> sources, bool isBottomInsert)
        {
            Layerage parents = LayerManager.GetParentsChildren(destination);
            int index = parents.Children.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parents.Children.Count - 1) index = parents.Children.Count - 1;

            if (source != null)
            {
                if (source.Parents != LayerManager.RootLayerage)
                {
                    // Refactoring
                    ILayer sourceParents = source.Parents.Self;
                    sourceParents.IsRefactoringTransformer = true;
                    sourceParents.IsRefactoringRender = true;
                    sourceParents.IsRefactoringIconRender = true;
                    source.Parents.RefactoringParentsTransformer();
                    source.Parents.RefactoringParentsRender();
                    source.Parents.RefactoringParentsIconRender();
                }

                {
                    Layerage sourceParents = LayerManager.GetParentsChildren(source);
                    sourceParents.Children.Remove(source);
                    parents.Children.Insert(index, source);
                }


                if (destination.Parents != LayerManager.RootLayerage)
                {
                    // Refactoring
                    ILayer destinationParents = destination.Parents.Self;
                    destinationParents.IsRefactoringTransformer = true;
                    destinationParents.IsRefactoringRender = true;
                    destinationParents.IsRefactoringIconRender = true;
                    destination.Parents.RefactoringParentsTransformer();
                    destination.Parents.RefactoringParentsRender();
                    destination.Parents.RefactoringParentsIconRender();
                }
            }
            else if (sources != null)
            {
                foreach (Layerage child in sources)
                {
                    if (child.Parents != LayerManager.RootLayerage)
                    {
                        // Refactoring
                        ILayer childParents = child.Parents.Self;
                        childParents.IsRefactoringTransformer = true;
                        childParents.IsRefactoringRender = true;
                        childParents.IsRefactoringIconRender = true;
                        child.Parents.RefactoringParentsTransformer();
                        child.Parents.RefactoringParentsRender();
                        child.Parents.RefactoringParentsIconRender();
                    }
                    {
                        Layerage childParents = LayerManager.GetParentsChildren(child);
                        childParents.Children.Remove(child);
                        parents.Children.Insert(index, child);
                    }
                }
            }


            if (destination.Parents != LayerManager.RootLayerage)
            {
                // Refactoring
                ILayer destinationParents = destination.Parents.Self;
                destinationParents.IsRefactoringTransformer = true;
                destinationParents.IsRefactoringRender = true;
                destinationParents.IsRefactoringIconRender = true;
                destination.Parents.RefactoringParentsTransformer();
                destination.Parents.RefactoringParentsRender();
                destination.Parents.RefactoringParentsIconRender();
            }
        }


        #endregion


        #region Add


        /// <summary>
        /// Add a current-layerage into source-layerage's children.
        /// </summary>
        /// <param name="currentLayerage"> The current layerage. </param>
        /// <param name="layerage"> The source layerage. </param>
        public static void Add(Layerage currentLayerage, Layerage layerage) => LayerManager.AddCore(currentLayerage, layerage, null);

        /// <summary>
        /// Add some layerages into children.
        /// </summary>
        /// <param name="currentLayerage"> The current layerages. </param>
        /// <param name="layerages"> The source layerages. </param>
        public static void AddRange(Layerage currentLayerage, IEnumerable<Layerage> layerages) => LayerManager.AddCore(currentLayerage, null, layerages);

        private static void AddCore(Layerage currentLayerage, Layerage layerage, IEnumerable<Layerage> layerages)
        {
            if (layerage != null)
            {
                if (layerage.Parents != LayerManager.RootLayerage)
                {
                    // Refactoring
                    ILayer layerageParents = layerage.Parents.Self;
                    layerageParents.IsRefactoringTransformer = true;
                    layerageParents.IsRefactoringRender = true;
                    layerageParents.IsRefactoringIconRender = true;
                    layerage.Parents.RefactoringParentsTransformer();
                    layerage.Parents.RefactoringParentsRender();
                    layerage.Parents.RefactoringParentsIconRender();
                }

                Layerage parents = LayerManager.GetParentsChildren(layerage);
                parents.Children.Remove(layerage);
                currentLayerage.Children.Add(layerage);


                // Refactoring
                ILayer currentLayer = currentLayerage.Self;
                currentLayer.IsRefactoringTransformer = true;
                currentLayer.IsRefactoringRender = true;
                currentLayer.IsRefactoringIconRender = true;
                currentLayerage.RefactoringParentsTransformer();
                currentLayerage.RefactoringParentsRender();
                currentLayerage.RefactoringParentsIconRender();
            }
            else if (layerages != null)
            {
                foreach (Layerage child in layerages)
                {
                    if (child.Parents != LayerManager.RootLayerage)
                    {
                        // Refactoring
                        ILayer childParents = child.Parents.Self;
                        childParents.IsRefactoringTransformer = true;
                        childParents.IsRefactoringRender = true;
                        childParents.IsRefactoringIconRender = true;
                        child.Parents.RefactoringParentsTransformer();
                        child.Parents.RefactoringParentsRender();
                        child.Parents.RefactoringParentsIconRender();
                    }
                    {
                        Layerage childParents = LayerManager.GetParentsChildren(child);
                        childParents.Children.Remove(child);
                        currentLayerage.Children.Add(child);
                    }

                    // Refactoring
                    ILayer currentLayer = currentLayerage.Self;
                    currentLayer.IsRefactoringTransformer = true;
                    currentLayer.IsRefactoringRender = true;
                    currentLayer.IsRefactoringIconRender = true;
                    currentLayerage.RefactoringParentsTransformer();
                    currentLayerage.RefactoringParentsRender();
                    currentLayerage.RefactoringParentsIconRender();
                }
            }
        }


        #endregion

    }
}