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
        /// <param name="sourceSelectMode"> The source SelectMode. </param>
        public void DragComplete(ILayer destination, ILayer source, OverlayMode destinationOverlayMode, SelectMode sourceSelectMode)
        {
            if (source == null) return;
            if (destination == null) return;

            destination.OverlayMode = OverlayMode.None;
            if (destination.SelectMode.ToBool()) return;
            if (destinationOverlayMode == OverlayMode.None) return;

            if (source == destination) return;


            bool isSelected = sourceSelectMode.ToBool();
            if (isSelected == false)
            {
                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        {
                            if (destination.Parents == source.Parents)
                                this.Move(destination, source, isBottomInsert: false);
                            else
                            {
                                LayerCollection.Disengage(source, this);
                                this.Insert(destination, source, isBottomInsert: false);
                            }
                        }
                        break;
                    case OverlayMode.Center:
                        {
                            LayerCollection.Disengage(source, this);
                            LayerCollection.Add(destination, source);
                        }
                        break;
                    case OverlayMode.Bottom:
                        {
                            if (destination.Parents == source.Parents)
                                this.Move(destination, source, isBottomInsert: true);
                            else
                            {
                                LayerCollection.Disengage(source, this);
                                this.Insert(destination, source, isBottomInsert: true);
                            }
                            break;
                        }
                }
            }
            else
            {
                IList<ILayer> selectedLayers = this.GetAllSelectedLayers();
                foreach (ILayer child in selectedLayers)
                {
                    LayerCollection.Disengage(child, this);
                }

                switch (destinationOverlayMode)
                {
                    case OverlayMode.Top:
                        this.InsertRange(destination, selectedLayers, isBottomInsert: false);
                        break;
                    case OverlayMode.Center:
                        LayerCollection.AddRange(destination, selectedLayers);
                        break;
                    case OverlayMode.Bottom:
                        this.InsertRange(destination, selectedLayers, isBottomInsert: true);
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
        public void Move(ILayer destination, ILayer source, bool isBottomInsert)
        {
            IList<ILayer> parentsChildren = (destination.Parents == null) ?
                this.RootLayers :
                destination.Parents.Children;

            parentsChildren.Remove(source);

            int index = parentsChildren.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parentsChildren.Count) index = parentsChildren.Count;

            parentsChildren.Insert(index, source);
        }


        #region Insert


        /// <summary>
        /// Insert a layer to the top of destination layer.
        /// </summary>
        /// <param name="destination"> The destination layer. </param>
        /// <param name="source"> The source layer. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public void Insert(ILayer destination, ILayer source, bool isBottomInsert) => this._insert(destination, source, null, isBottomInsert);

        /// <summary>
        /// Insert some layers to the top of destination layer.
        /// </summary>
        /// <param name="destination"> The destination layer. </param>
        /// <param name="sources"> The source layers. </param>
        /// <param name="isBottomInsert"> Insert to the top or bottom. </param>
        public void InsertRange(ILayer destination, IList<ILayer> sources, bool isBottomInsert) => this._insert(destination, null, sources, isBottomInsert);


        private void _insert(ILayer destination, ILayer source, IList<ILayer> sources, bool isBottomInsert)
        {
            IList<ILayer> parentsChildren = (destination.Parents == null) ?
                this.RootLayers :
                destination.Parents.Children;

            int index = parentsChildren.IndexOf(destination);
            if (isBottomInsert) index++;
            if (index < 0) index = 0;
            if (index > parentsChildren.Count) index = parentsChildren.Count;

            bool isSelected = false;
            if (destination.Parents != null)
            {
                if (destination.Parents.SelectMode.ToBool())
                {
                    isSelected = true;
                }
            }

            if (source != null)
            {
                //Insert
                source.Parents = destination.Parents;
                parentsChildren.Insert(index, source);

                if (isSelected) source.SelectMode = SelectMode.ParentsSelected;
            }
            else if (sources != null)
            {
                foreach (ILayer child in sources)
                {
                    child.Parents = destination.Parents;
                    parentsChildren.Insert(index, child);

                    if (isSelected) child.SelectMode = SelectMode.ParentsSelected;
                }
            }
        }


        #endregion


    }
}