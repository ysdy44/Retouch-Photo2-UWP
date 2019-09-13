using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Manager of <see cref="TransformerControlState"/>. 
    /// </summary>
    public class TransformerControlStateManager
    {
        /// <summary> <see cref = "TransformerControl.Tool" />/ </summary>
        public bool DisabledTool;
        /// <summary> <see cref = "TransformerControl.DisabledRadian" />/ </summary>
        public bool DisabledRadian;
        /// <summary> <see cref = "TransformerControl.Mode" />/ </summary>
        public ListViewSelectionMode Mode;

        /// <summary>
        /// Return status based on propertys.
        /// </summary>
        /// <returns> state </returns>
        public TransformerControlState GetState()
        {
            if (this.DisabledTool) return TransformerControlState.Disabled;

            switch (this.Mode)
            {
                case ListViewSelectionMode.None: return TransformerControlState.Disabled;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        if (this.DisabledRadian)
                            return TransformerControlState.EnabledWithoutRadian;
                        else
                            return TransformerControlState.Enabled;
                    }
            }

            return TransformerControlState.Enabled;
        }
    }
}