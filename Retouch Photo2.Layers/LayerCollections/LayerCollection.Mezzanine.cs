using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        
        /// <summary>
        /// Mezzanine a layer.
        /// </summary>
        /// <param name="mezzanineLayer"> The mezzanine layer. </param>
        public static void Mezzanine(LayerCollection layerCollection, ILayer mezzanineLayer) => LayerCollection._mezzanine(layerCollection, mezzanineLayer, null);
        /// <summary>
        /// Mezzanine layers.
        /// </summary>
        /// <param name="mezzanineLayers"> The mezzanine layers. </param>
        public static void MezzanineRange(LayerCollection layerCollection, IEnumerable<ILayer> mezzanineLayers) => LayerCollection._mezzanine(layerCollection, null, mezzanineLayers);

        private static void _mezzanine(LayerCollection layerCollection, ILayer mezzanineLayer, IEnumerable<ILayer> mezzanineLayers)
        {
            //Layers
            IEnumerable<ILayer> selectedLayers = LayerCollection.GetAllSelectedLayers(layerCollection);
            ILayer outermost = LayerCollection.FindOutermost_SelectedLayer(selectedLayers);
            //if (outermost == null) return; // If count = 0, it will be useless.
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;

            if (mezzanineLayer!=null)
            {
                mezzanineLayer.IsSelected = true;
                parentsChildren.Insert(index, mezzanineLayer);//Insert
            }
            else if (mezzanineLayers != null)
            {
                foreach (ILayer child in mezzanineLayers)
                {
                    child.IsSelected = true;
                    parentsChildren.Insert(index, child);//Insert
                }
            }
        }


        /// <summary>
        /// Remove the mezzanine layer.
        /// </summary>
        public static void RemoveMezzanineLayer(LayerCollection layerCollection, ILayer mezzanineLayer)
        {
            if (mezzanineLayer == null) return;
     
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(mezzanineLayer);

            parentsChildren.Remove(mezzanineLayer);
        }


    }
}