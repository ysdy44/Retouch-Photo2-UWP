using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerageCollection
    {

        /// <summary>
        /// Mezzanine a layerage.
        /// </summary>
        /// <param name="mezzanineLayerage"> The mezzanine layerage. </param>
        public static void Mezzanine( Layerage mezzanineLayerage) => LayerageCollection._mezzanine( mezzanineLayerage, null);

        /// <summary>
        /// Mezzanine layers.
        /// </summary>
        /// <param name="mezzanineLayerages"> The mezzanine layers. </param>
        public static void MezzanineRange( IEnumerable<Layerage> mezzanineLayerages) => LayerageCollection._mezzanine( null, mezzanineLayerages);

        private static void _mezzanine( Layerage mezzanineLayer, IEnumerable<Layerage> mezzanineLayers)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelected();
            Layerage outermost = LayerageCollection.FindOutermostLayerage(selectedLayerages);
            //if (outermost == null) return; // If count = 0, it will be useless.
            Layerage parents  = LayerageCollection.GetParentsChildren(outermost);
            int index = parents.Children.IndexOf(outermost);
            if (index < 0) index = 0;

            if (mezzanineLayer!=null)
            {
                mezzanineLayer.Self.IsSelected = true;
                parents.Children.Insert(index, mezzanineLayer);//Insert
            }
            else if (mezzanineLayers != null)
            {
                foreach (Layerage child in mezzanineLayers)
                {
                    child.Self.IsSelected = true;
                    parents.Children.Insert(index, child);//Insert
                }
            }
        }


        /// <summary>
        /// Remove the mezzanine layerage.
        /// </summary>
        /// <param name="mezzanineLayerage"> The mezzanine-layerage. </param>
        public static void RemoveMezzanine( Layerage mezzanineLayerage)
        {
            if (mezzanineLayerage == null) return;

            Layerage parents = LayerageCollection.GetParentsChildren(mezzanineLayerage);
            parents.Children.Remove(mezzanineLayerage);
        }


    }
}