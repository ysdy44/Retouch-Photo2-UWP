using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
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

            parentsChildren.Remove(source);
            parentsChildren.Insert(index, source);

            //IsRefactoringTransformer
            if (source != null)
            {
                ILayer source2 = source.Self;
                source2.IsRefactoringTransformer = true;
            }
            if (destination != null)
            {
                ILayer destination2 = destination.Self;
                destination2.IsRefactoringTransformer = true;
            }
        }


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
                IList<Layerage> sourceParentsChildren = layerageCollection.GetParentsChildren(source);
                sourceParentsChildren.Remove(source);
                //Insert
                parentsChildren.Insert(index, source);
            }
            else if (sources != null)
            {
                foreach (Layerage child in sources)
                {
                    IList<Layerage> childParentsChildren = layerageCollection.GetParentsChildren(child);
                    childParentsChildren.Remove(child);
                    //Insert
                    parentsChildren.Insert(index, child);
                }
            }

            //IsRefactoringTransformer
            if (source != null)
            {
                ILayer source2 = source.Self;
                source2.IsRefactoringTransformer = true;
            }
            if (destination != null)
            {
                ILayer destination2 = destination.Self;
                destination2.IsRefactoringTransformer = true;
            }
        }


        #endregion


    }
}