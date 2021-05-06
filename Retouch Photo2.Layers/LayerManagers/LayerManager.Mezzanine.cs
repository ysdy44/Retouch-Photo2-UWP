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
        /// Mezzanine a layerage.
        /// </summary>
        /// <param name="mezzanineLayerage"> The mezzanine layerage. </param>
        public static void Mezzanine( Layerage mezzanineLayerage) => LayerManager.MezzanineCore( mezzanineLayerage, null);

        /// <summary>
        /// Mezzanine layers.
        /// </summary>
        /// <param name="mezzanineLayerages"> The mezzanine layers. </param>
        public static void MezzanineRange( IEnumerable<Layerage> mezzanineLayerages) => LayerManager.MezzanineCore( null, mezzanineLayerages);

        private static void MezzanineCore( Layerage mezzanineLayer, IEnumerable<Layerage> mezzanineLayers)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerManager.GetAllSelected();
            Layerage outermost = LayerManager.FindOutermostLayerage(selectedLayerages);
            //if (outermost == null) return; // If count = 0, it will be useless.
            Layerage parents  = LayerManager.GetParentsChildren(outermost);
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

            Layerage parents = LayerManager.GetParentsChildren(mezzanineLayerage);
            parents.Children.Remove(mezzanineLayerage);
        }


    }
}