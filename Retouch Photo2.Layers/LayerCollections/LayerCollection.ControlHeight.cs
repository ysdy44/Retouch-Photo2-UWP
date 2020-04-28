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
        public void SetControlHeight(int controlHeight)
        {
            LayerCollection.ControlsHeight = controlHeight;

            //Recursive
            this._setControlHeight(this.RootLayers, controlHeight);
        }

        private void _setControlHeight(IList<ILayer> layers, int controlHeight)
        {
            foreach (ILayer child in layers)
            {
                if (child.Control.ControlHeight != controlHeight)
                    child.Control.ControlHeight = controlHeight;

                //Recursive
                this._setControlHeight(child.Children, controlHeight);
            }
        }
        
    }
}