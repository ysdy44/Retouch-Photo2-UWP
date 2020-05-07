using FanKit.Transformers;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
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
        public void SetValue(Action<ILayer> action)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;

                case ListViewSelectionMode.Single:
                    action(this.Layer);
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (ILayer child in this.Layers)
                    {
                        action(child);
                    }
                    break;
            }
        }


        /// <summary>
        /// Refactoring the transformer.
        /// </summary>
        /// <returns> The transformer. </returns>
        public Transformer RefactoringTransformer()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    return new Transformer();

                case ListViewSelectionMode.Single:
                    return this.Layer.GetActualDestinationWithRefactoringTransformer;

                case ListViewSelectionMode.Multiple:
                    return LayerCollection.RefactoringTransformer(this.Layers);

                default:
                    return new Transformer();
            }
        }


        /// <summary>
        /// Get the selected layer.
        /// None: null;
        /// Single: layer;
        /// Multiple: first layer;
        /// </summary>
        /// <returns> The selected layer. </returns>
        public ILayer GetFirstLayer()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;
                case ListViewSelectionMode.Single: return this.Layer;
                case ListViewSelectionMode.Multiple: return this.Layers.FirstOrDefault();
                default: return null;
            }
        }

    }
}
