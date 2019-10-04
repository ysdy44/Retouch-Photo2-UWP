using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {


        /// <summary>
        /// Mezzanine
        /// </summary>
        public void MezzanineOnFirstSelectedLayer(ILayer mezzanineLayer)
        {
            ILayer firstSelectedLayer = null;
            IList<ILayer> mezzanineLayers = null;

            void mezzanineOnFirstSelectedLayer(IList<ILayer> layers)
            {
                foreach (ILayer child in layers)
                {
                    if (child.SelectMode.ToBool())
                    {
                        firstSelectedLayer = child;
                        mezzanineLayers = layers;
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
            

            if (firstSelectedLayer == null || mezzanineLayers == null)
            {
                this.RootLayers.Insert(0, mezzanineLayer);//Insert
            }
            else
            {
                int index = mezzanineLayers.IndexOf(firstSelectedLayer);

                if (index >= 0)
                {
                    if (index < mezzanineLayers.Count)
                    {
                        mezzanineLayer.Parents = firstSelectedLayer.Parents;

                        mezzanineLayers.Insert(index, mezzanineLayer);//Insert
                    }
                }
            }
        }


        /// <summary>
        /// Mezzanine
        /// </summary>
        public void RemoveMezzanineLayer(ILayer mezzanineLayer)
        {
            IList<ILayer> parentsChildren = (mezzanineLayer.Parents == null) ?
                this.RootLayers : mezzanineLayer.Parents.Children;

            parentsChildren.Remove(mezzanineLayer);
        }


    }
}