using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Drag complete.
        /// </summary>
        /// <param name="destination"> The destination layerage. </param>
        /// <param name="source"> The source layerage. </param>
        /// <param name="destinationOverlayMode"> The destination OverlayMode. </param>
        /// <param name="sourceIsSelected"> The source SelectMode. </param>
        public static void DragComplete(LayerageCollection layerageCollection, Layerage destination, Layerage source, OverlayMode destinationOverlayMode, bool sourceIsSelected)
        {     
            if (source == null) return;
            if (destination == null) return;
            ILayer destination2 = destination.Self;
            ILayer source2 = source.Self;

            destination2.Control.OverlayMode = OverlayMode.None;
            if (destination2.IsSelected) return;
            if (destinationOverlayMode == OverlayMode.None) return;

            if (source == destination) return;


            bool isSelected = source2.IsSelected;
            if (isSelected == false)
            {
                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        LayerageCollection.Insert(layerageCollection, destination, source, isBottomInsert: false);
                        break;
                    case OverlayMode.Center:
                        LayerageCollection.Add(layerageCollection, destination, source);
                        break;
                    case OverlayMode.Bottom:
                        LayerageCollection.Insert(layerageCollection, destination, source, isBottomInsert: true);
                        break;
                }
            }
            else
            {
                //Layerages
                IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelectedLayerages(layerageCollection);

                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        LayerageCollection.InsertRange(layerageCollection, destination, selectedLayerages, isBottomInsert: false);
                        break;
                    case OverlayMode.Center:
                        LayerageCollection.AddRange(layerageCollection, destination, selectedLayerages);
                        break;
                    case OverlayMode.Bottom:
                        LayerageCollection.InsertRange(layerageCollection, destination, selectedLayerages, isBottomInsert: true);
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
        public static void Move(LayerageCollection layerageCollection, Layerage destination, Layerage source, bool isBottomInsert)
        {
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(destination);
            int index = parentsChildren.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parentsChildren.Count - 1) index = parentsChildren.Count - 1;


            if (source.Parents != null)
            {
                //Refactoring
                //ILayer sourceParents = source.Parents.Self;
                //sourceParents.IsRefactoringTransformer = true;
                //sourceParents.IsRefactoringRender = true;
                //sourceParents.IsRefactoringIconRender = true;
                source.Parents.RefactoringParentsTransformer();
                source.Parents.RefactoringParentsRender();
                source.Parents.RefactoringParentsIconRender();
            }

            parentsChildren.Remove(source);
            parentsChildren.Insert(index, source);

            
            if (destination.Parents != null)
            {
                //Refactoring
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
        public static void Insert(LayerageCollection layerageCollection, Layerage destination, Layerage source, bool isBottomInsert) => LayerageCollection._insert(layerageCollection, destination, source, null, isBottomInsert);

        /// <summary>
        /// Insert some layers to the top of destination layerage.
        /// </summary>
        /// <param name="destination"> The destination layerage. </param>
        /// <param name="sources"> The source layers. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void InsertRange(LayerageCollection layerageCollection, Layerage destination, IEnumerable<Layerage> sources, bool isBottomInsert) => LayerageCollection._insert(layerageCollection, destination, null, sources, isBottomInsert);


        private static void _insert(LayerageCollection layerageCollection, Layerage destination, Layerage source, IEnumerable<Layerage> sources, bool isBottomInsert)
        {
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(destination);
            int index = parentsChildren.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parentsChildren.Count - 1) index = parentsChildren.Count - 1;

            if (source != null)
            {
                if (source.Parents != null)
                {
                    //Refactoring
                    ILayer sourceParents = source.Parents.Self;
                    sourceParents.IsRefactoringTransformer = true;
                    sourceParents.IsRefactoringRender = true;
                    sourceParents.IsRefactoringIconRender = true;
                    source.Parents.RefactoringParentsTransformer();
                    source.Parents.RefactoringParentsRender();
                    source.Parents.RefactoringParentsIconRender();
                }


                IList<Layerage> sourceParentsChildren = layerageCollection.GetParentsChildren(source);
                sourceParentsChildren.Remove(source);
                parentsChildren.Insert(index, source);


                if (destination.Parents != null)
                {
                    //Refactoring
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
                    if (child.Parents != null)
                    {
                        //Refactoring
                        ILayer childParents = child.Parents.Self;
                        childParents.IsRefactoringTransformer = true;
                        childParents.IsRefactoringRender = true;
                        childParents.IsRefactoringIconRender = true;
                        child.Parents.RefactoringParentsTransformer();
                        child.Parents.RefactoringParentsRender();
                        child.Parents.RefactoringParentsIconRender();
                    }

                    IList<Layerage> childParentsChildren = layerageCollection.GetParentsChildren(child);
                    childParentsChildren.Remove(child);
                    parentsChildren.Insert(index, child);
                }
            }
            

            if (destination.Parents != null)
            {
                //Refactoring
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
        public static void Add(LayerageCollection layerageCollection, Layerage currentLayerage, Layerage layerage) => LayerageCollection._add(layerageCollection, currentLayerage, layerage, null);

        /// <summary>
        /// Add some layerages into children.
        /// </summary>
        /// <param name="currentLayerage"> The current layerages. </param>
        /// <param name="layerages"> The source layerages. </param>
        public static void AddRange(LayerageCollection layerageCollection, Layerage currentLayerage, IEnumerable<Layerage> layerages) => LayerageCollection._add(layerageCollection, currentLayerage, null, layerages);

        private static void _add(LayerageCollection layerageCollection, Layerage currentLayerage, Layerage layerage, IEnumerable<Layerage> layerages)
        {
            if (layerage != null)
            {
                if (layerage.Parents != null)
                {
                    //Refactoring
                    ILayer layerageParents = layerage.Parents.Self;
                    layerageParents.IsRefactoringTransformer = true;
                    layerageParents.IsRefactoringRender = true;
                    layerageParents.IsRefactoringIconRender = true;
                    layerage.Parents.RefactoringParentsTransformer();
                    layerage.Parents.RefactoringParentsRender();
                    layerage.Parents.RefactoringParentsIconRender();
                }

                IList<Layerage> layerageParentsChildren = layerageCollection.GetParentsChildren(layerage);
                layerageParentsChildren.Remove(layerage);
                currentLayerage.Children.Add(layerage);


                //Refactoring
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
                    if (child.Parents != null)
                    {
                        //Refactoring
                        ILayer childParents = child.Parents.Self;
                        childParents.IsRefactoringTransformer = true;
                        childParents.IsRefactoringRender = true;
                        childParents.IsRefactoringIconRender = true;
                        child.Parents.RefactoringParentsTransformer();
                        child.Parents.RefactoringParentsRender();
                        child.Parents.RefactoringParentsIconRender();
                    }


                    IList<Layerage> childParentsChildren = layerageCollection.GetParentsChildren(child);
                    childParentsChildren.Remove(child);
                    currentLayerage.Children.Add(child);


                    //Refactoring
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