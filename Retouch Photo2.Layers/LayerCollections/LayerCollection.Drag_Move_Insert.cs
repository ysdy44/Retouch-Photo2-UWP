using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Drag complete.
        /// </summary>
        /// <param name="destination"> The destination layer. </param>
        /// <param name="source"> The source layer. </param>
        /// <param name="destinationOverlayMode"> The destination OverlayMode. </param>
        /// <param name="sourceIsSelected"> The source SelectMode. </param>
        public static void DragComplete(LayerCollection layerCollection, ILayer destination, ILayer source, OverlayMode destinationOverlayMode, bool sourceIsSelected)
        {
            if (source == null) return;
            if (destination == null) return;

            destination.OverlayMode = OverlayMode.None;
            if (destination.IsSelected) return;
            if (destinationOverlayMode == OverlayMode.None) return;

            if (source == destination) return;


            bool isSelected = source.IsSelected;
            if (isSelected == false)
            {
                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        {
                            if (destination.Parents == source.Parents)
                            {
                                LayerCollection.Move(layerCollection, destination, source, isBottomInsert: false);
                            }
                            else
                            {
                                LayerCollection.Insert(layerCollection, destination, source, isBottomInsert: false);
                            }
                        }
                        break;
                    case OverlayMode.Center:
                        {
                            LayerCollection.Add(layerCollection, destination, source);
                        }
                        break;
                    case OverlayMode.Bottom:
                        {
                            if (destination.Parents == source.Parents)
                                LayerCollection.Move(layerCollection, destination, source, isBottomInsert: true);
                            else
                            {
                                LayerCollection.Insert(layerCollection, destination, source, isBottomInsert: true);
                            }
                            break;
                        }
                }
            }
            else
            {
                IEnumerable<ILayer> selectedLayers = LayerCollection.GetAllSelectedLayers(layerCollection);

                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        LayerCollection.InsertRange(layerCollection, destination, selectedLayers, isBottomInsert: false);
                        break;
                    case OverlayMode.Center:
                        LayerCollection.AddRange(layerCollection, destination, selectedLayers);
                        break;
                    case OverlayMode.Bottom:
                        LayerCollection.InsertRange(layerCollection, destination, selectedLayers, isBottomInsert: true);
                        break;
                }
            }
        }


        /// <summary>
        /// Move a layer to the top of destination layer.
        /// </summary>
        /// <param name="destination"> The destination layer. </param>
        /// <param name="source"> The source layer. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void Move(LayerCollection layerCollection, ILayer destination, ILayer source, bool isBottomInsert)
        {
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(destination);
            int index = parentsChildren.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parentsChildren.Count) index = parentsChildren.Count - 1;

            parentsChildren.Remove(source);
            parentsChildren.Insert(index, source);
        }


        #region Insert


        /// <summary>
        /// Insert a layer to the top of destination layer.
        /// </summary>
        /// <param name="destination"> The destination layer. </param>
        /// <param name="source"> The source layer. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void Insert(LayerCollection layerCollection, ILayer destination, ILayer source, bool isBottomInsert) => LayerCollection._insert(layerCollection, destination, source, null, isBottomInsert);

        /// <summary>
        /// Insert some layers to the top of destination layer.
        /// </summary>
        /// <param name="destination"> The destination layer. </param>
        /// <param name="sources"> The source layers. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public static void InsertRange(LayerCollection layerCollection, ILayer destination, IEnumerable<ILayer> sources, bool isBottomInsert) => LayerCollection._insert(layerCollection, destination, null, sources, isBottomInsert);


        private static void _insert(LayerCollection layerCollection, ILayer destination, ILayer source, IEnumerable<ILayer> sources, bool isBottomInsert)
        {
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(destination);
            int index = parentsChildren.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parentsChildren.Count) index = parentsChildren.Count - 1;

            if (source != null)
            {
                IList<ILayer> sourceParentsChildren = layerCollection.GetParentsChildren(source);
                sourceParentsChildren.Remove(source);
                //Insert
                parentsChildren.Insert(index, source);
            }
            else if (sources != null)
            {
                foreach (ILayer child in sources)
                {
                    IList<ILayer> childParentsChildren = layerCollection.GetParentsChildren(child);
                    childParentsChildren.Remove(child);
                    //Insert
                    parentsChildren.Insert(index, child);
                }
            }
        }


        #endregion


    }
}