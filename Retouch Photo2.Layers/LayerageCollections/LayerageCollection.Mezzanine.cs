using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Mezzanine a layerage.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <param name="mezzanineLayerage"> The mezzanine layerage. </param>
        public static void Mezzanine(LayerageCollection layerageCollection, Layerage mezzanineLayerage) => LayerageCollection._mezzanine(layerageCollection, mezzanineLayerage, null);

        /// <summary>
        /// Mezzanine layers.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <param name="mezzanineLayerages"> The mezzanine layers. </param>
        public static void MezzanineRange(LayerageCollection layerageCollection, IEnumerable<Layerage> mezzanineLayerages) => LayerageCollection._mezzanine(layerageCollection, null, mezzanineLayerages);

        private static void _mezzanine(LayerageCollection layerageCollection, Layerage mezzanineLayer, IEnumerable<Layerage> mezzanineLayers)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelected(layerageCollection);
            Layerage outermost = LayerageCollection.FindOutermostLayerage(selectedLayerages);
            //if (outermost == null) return; // If count = 0, it will be useless.
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;

            if (mezzanineLayer!=null)
            {
                mezzanineLayer.Self.IsSelected = true;
                parentsChildren.Insert(index, mezzanineLayer);//Insert
            }
            else if (mezzanineLayers != null)
            {
                foreach (Layerage child in mezzanineLayers)
                {
                    child.Self.IsSelected = true;
                    parentsChildren.Insert(index, child);//Insert
                }
            }
        }


        /// <summary>
        /// Remove the mezzanine layerage.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <param name="mezzanineLayerage"> The mezzanine-layerage. </param>
        public static void RemoveMezzanine(LayerageCollection layerageCollection, Layerage mezzanineLayerage)
        {
            if (mezzanineLayerage == null) return;
     
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(mezzanineLayerage);

            parentsChildren.Remove(mezzanineLayerage);
        }


    }
}