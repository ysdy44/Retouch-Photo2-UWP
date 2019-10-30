using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// 
        /// </summary>
        public void ArrangeChildrenExpand() => LayerCollection._arrangeChildrenExpand(this.RootLayers);


        /// <summary>
        /// 
        /// </summary>
        public static void _arrangeChildrenExpand(IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                bool isZero = child.Children.Count == 0;

                if (isZero)
                {
                    child.ExpandMode =  ExpandMode.NoChildren;
                }
                else
                {
                    child.ExpandMode = ExpandMode.UnExpand;

                    LayerCollection._arrangeChildrenExpand(child.Children);
                }
            }
        }

         
    }
}