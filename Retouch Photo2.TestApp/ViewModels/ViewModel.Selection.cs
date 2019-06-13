using Retouch_Photo2.Layers;
using Retouch_Photo2.Transformers;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel
    {
        /// <summary>
        /// Gets selection-mode by count of checked layers. 
        /// 0>None.
        /// 1>Single.
        /// 2>Multiple.
        /// Temporary Transformer: Extended
        /// </summary>
        public ListViewSelectionMode SelectionMode
        {
            get => this.selectionMode;
            set
            {
                this.selectionMode = value;
                this.OnPropertyChanged(nameof(this.SelectionMode));//Notify 
            }
        }
        private ListViewSelectionMode selectionMode;

        /// <summary> Transformer of checked layers.  </summary>
        public Transformer SelectionTransformer
        {
            get => this.selectionTransformer;
             set
            {
                this.selectionTransformer = value;
                this.OnPropertyChanged(nameof(this.SelectionTransformer));//Notify 
            }
        }
        private Transformer selectionTransformer;

        /// <summary> Transformer of the single checked layer.  </summary>
        public Layer SelectionLayer { get; private set; }

        /// <summary> Transformer of the all checked layers.  </summary>
        public IEnumerable<Layer> SelectionLayers { get; private set; }      
    }
}