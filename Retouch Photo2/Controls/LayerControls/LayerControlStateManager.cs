using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Manager of <see cref="LayerControlState"/>. 
    /// </summary>
    public class LayerControlStateManager
    {
        public bool IsBlends;
        public bool IsChildren;

        public bool IsGroupLayer;
        public ListViewSelectionMode Mode;

        /// <summary>
        /// Return status based on propertys.
        /// </summary>
        public LayerControlState GetState()
        {
            if (this.IsBlends) return LayerControlState.Blends;
            if (this.IsChildren) return LayerControlState.Children;
            
            switch (this.Mode)
            {
                case ListViewSelectionMode.None: return LayerControlState.Disable;
                case ListViewSelectionMode.Single:
                    {
                        return (this.IsGroupLayer) ?
                            LayerControlState.SingleLayerWithChildren : 
                            LayerControlState.SingleLayerWithoutChildren;
                    }
                case ListViewSelectionMode.Multiple:return LayerControlState.MultipleLayer;
            }

            return LayerControlState.Disable;
        }
    }
}