using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {
        //@Static
        /// <summary>
        /// Gets layer controls height.
        /// </summary>
        public static int ControlsHeight { get; private set; } = 40;

        /// <summary>
        /// Sets all layer controls height.
        /// </summary>
        public static void SetControlHeight(int controlHeight)
        {
            LayerageCollection.ControlsHeight = controlHeight;
        }

        /// <summary>
        /// Sets all layer controls height.
        /// </summary>
        public static void SetControlHeight(LayerageCollection layerageCollection, int controlHeight)
        {
            LayerageCollection.ControlsHeight = controlHeight;

            //Recursive
            LayerageCollection._setControlHeight(layerageCollection.RootLayerages, controlHeight);
        }

        private static void _setControlHeight(IEnumerable<Layerage> layerages, int controlHeight)
        {        
            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;

                if (layer.Control.ControlHeight != controlHeight)
                {
                    layer.Control.ControlHeight = controlHeight;
                }

                //Recursive
                LayerageCollection._setControlHeight(layerage.Children, controlHeight);
            }
        }

    }
}