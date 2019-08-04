using Retouch_Photo2.Layers;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels.Selections
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Sets all selection layer(s).
        /// </summary>
        /// <param name="action"> action </param>
        /// <param name="setChildrenValue"> Whether to set the value along with the child? </param>
        public void SetValue(Action<ILayer> action, bool setChildrenValue = false)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None:
                    break;

                case ListViewSelectionMode.Single:
                    this.SetValueSingle(action, setChildrenValue);
                    break;

                case ListViewSelectionMode.Multiple:
                    this.SetValueMultiple(action, setChildrenValue);
                    break;
            }
        }

        /// <summary>
        /// Sets the selection layer.
        /// </summary>
        /// <param name="action"> action </param>
        /// <param name="setChildrenValue"> Whether to set the value along with the child? </param>
        private void SetValueSingle(Action<ILayer> action, bool setChildrenValue = false)
        {
            this.SetValueForeachChildren(action, this.Layer, setChildrenValue);
        }

        /// <summary>
        /// Sets all selection layers.
        /// </summary>
        /// <param name="action"> action </param>
        /// <param name="setChildrenValue"> Whether to set the value along with the child? </param>
        private void SetValueMultiple(Action<ILayer> action, bool setChildrenValue = false)
        {
            foreach (ILayer selectionLayer in this.Layers)
            {
                this.SetValueForeachChildren(action, selectionLayer, setChildrenValue);
            }
        }

               
        /// <summary>
        /// Sets the layer's children.
        /// </summary>
        /// <param name="action"> action </param>
        /// <param name="layer"> The source layer. </param>
        /// <param name="setChildrenValue"> Whether to set the value along with the child? </param>
        private void SetValueForeachChildren(Action<ILayer> action, ILayer layer, bool setChildrenValue)
        {
            action(layer);

            if (setChildrenValue)
            {
                foreach (ILayer child in layer.Children)
                {
                    this.SetValueForeachChildren(action, child, setChildrenValue);
                }
            }
        }



    }
}