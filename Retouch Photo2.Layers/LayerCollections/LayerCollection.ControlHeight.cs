using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Gets or sets all layer controls height.
        /// </summary>
        public int ControlsHeight
        {
            get => this.controlHeight;
            set
            {
                if(this.controlHeight == value) return;

                //Recursive
                this._setControlHeight(this.RootLayers, value);

                this.controlHeight = value;
            }
        }
        private int controlHeight = 40;


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