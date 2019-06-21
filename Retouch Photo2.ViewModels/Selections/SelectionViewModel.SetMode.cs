using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
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
        public void SetValue(Action<Layer> action, bool setChildrenValue = false)
        {
            switch (this.Mode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    {
                        this.SetValueForeachChildren(action, this.Layer, setChildrenValue);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        foreach (Layer selectionLayer in this.Layers)
                        {
                            this.SetValueForeachChildren(action, selectionLayer, setChildrenValue);
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// Sets the layer's children.
        /// </summary>
        /// <param name="action"> action </param>
        /// <param name="layer"> The source layer. </param>
        /// <param name="setChildrenValue"> Whether to set the value along with the child? </param>
        private void SetValueForeachChildren(Action<Layer> action, Layer layer, bool setChildrenValue)
        {
            action(layer);

            if (setChildrenValue)
            {
                foreach (Layer child in layer.Children)
                {
                    this.SetValueForeachChildren(action, child, setChildrenValue);
                }
            }
        }



    }
}