using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        //@Static
        /// <summary>
        /// Gets layer controls height.
        /// </summary>
        public static int ControlsHeight { get; private set; } = 50;

        /// <summary>
        /// Sets all layer controls height.
        /// </summary>
        public static void SetControlHeight(LayerCollection layerCollection, int controlHeight)
        {
            LayerCollection.ControlsHeight = controlHeight;

            //Recursive
            LayerCollection._setControlHeight(layerCollection.RootLayers, controlHeight);
        }

        private static void _setControlHeight(IEnumerable<ILayer> layers, int controlHeight)
        {
            foreach (ILayer child in layers)
            {
                if (child.Control.ControlHeight != controlHeight)
                    child.Control.ControlHeight = controlHeight;

                //Recursive
                LayerCollection._setControlHeight(child.Children, controlHeight);
            }
        }
        
    }
}