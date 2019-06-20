using Retouch_Photo2.Layers;
using Retouch_Photo2.Transformers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Gets selection-mode by count of checked layers. 
        /// 0>None.
        /// 1>Single.
        /// 2>Multiple.
        /// Temporary Transformer: Extended
        /// </summary>
        public ListViewSelectionMode Mode
        {
            get => this.mode;
            set
            {
                this.mode = value;
                this.OnPropertyChanged(nameof(this.Mode));//Notify 
            }
        }
        private ListViewSelectionMode mode;


        /// <summary> Transformer of selection layers.  </summary>
        public Transformer Transformer
        {
            get => this.transformer;
            set
            {
                this.transformer = value;
                this.OnPropertyChanged(nameof(this.Transformer));//Notify 
            }
        }
        private Transformer transformer;


        /// <summary> Transformer of the single checked layer.  </summary>
        public Layer Layer { get; private set; }


        /// <summary> Transformer of the all checked layers.  </summary>
        public IEnumerable<Layer> Layers { get; private set; }

        
        /// <summary>
        /// Gets selection layer(s)'s transformer.
        /// </summary>
        /// <param name="action"> action </param>
        public Transformer GetTransformer()
        {
            if (this.Mode == ListViewSelectionMode.Single)
            {
                return this.Layer.TransformerMatrix.Destination;
            }

            return this.Transformer;
        }


        //Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="name"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}