using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        
        /// <summary>
        /// Mezzanine on the first selected layer.
        /// </summary>
        /// <param name="mezzanineLayer"> The mezzanine layer. </param>
        public void MezzanineOnFirstSelectedLayer(ILayer mezzanineLayer) => this._mezzanineOnFirstSelectedLayer(mezzanineLayer, null);
        /// <summary>
        /// Mezzanine range on first selected layer
        /// </summary>
        /// <param name="mezzanineLayers"> The mezzanine layers. </param>
        public void MezzanineRangeOnFirstSelectedLayer(IList<ILayer> mezzanineLayers) => this._mezzanineOnFirstSelectedLayer(null, mezzanineLayers);

        private void _mezzanineOnFirstSelectedLayer(ILayer mezzanineLayer, IList<ILayer> mezzanineLayers)
        {
            int firstIndex=-1; 
            ILayer firstIParent = null;

             ILayer firstSelectedLayer = null;
            IList<ILayer> parentChildren = null;

            void mezzanineOnFirstSelectedLayer(IList<ILayer> layers)
            {
                foreach (ILayer child in layers)
                {
                    if (child.SelectMode.ToBool())
                    {
                        firstSelectedLayer = child;
                        parentChildren = layers;
                        break;
                    }
                    else
                    {
                        //Recursive
                       mezzanineOnFirstSelectedLayer( child.Children);
                    }
                }
            }

            //Recursive
            mezzanineOnFirstSelectedLayer( this.RootLayers);



            if (firstSelectedLayer == null || parentChildren == null)
            {
                firstIndex = 0;
                firstIParent = null;
                parentChildren = this.RootLayers;
            }
            else
            {
                firstIndex = parentChildren.IndexOf(firstSelectedLayer);
                firstIndex--;
                if (firstIndex < 0) firstIndex = 0;
                if (firstIndex >= parentChildren.Count) firstIndex = parentChildren.Count - 1;

                firstIParent = firstSelectedLayer.Parents;
            }
            
                       
            if (mezzanineLayer!=null)
            {
                mezzanineLayer.Parents = firstIParent;
                parentChildren.Insert(firstIndex, mezzanineLayer);//Insert
            }
            else if (mezzanineLayers != null)
            {
                foreach (ILayer child in mezzanineLayers)
                {
                    child.Parents = firstIParent;
                    parentChildren.Insert(firstIndex, child);//Insert
                }
            }
        }


        /// <summary>
        /// Remove the mezzanine layer.
        /// </summary>
        public void RemoveMezzanineLayer(ILayer mezzanineLayer)
        {
            if (mezzanineLayer == null) return;
     
            IList<ILayer> parentsChildren = (mezzanineLayer.Parents == null) ?
                this.RootLayers : mezzanineLayer.Parents.Children;

            parentsChildren.Remove(mezzanineLayer);
        }


    }
}