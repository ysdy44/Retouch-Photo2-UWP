// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Gets selection-mode by count of selected layers. 
        /// 0>None.
        /// 1>Single.
        /// 2>Multiple.
        /// MezzanineLayer > Extended
        /// </summary>
        public ListViewSelectionMode SelectionMode
        {
            get => this.selectionMode;
            private set
            {
                switch (value)
                {
                    case ListViewSelectionMode.Single:
                        this.SelectionUnNone = true;
                        this.SelectionSingle = true;
                        this.SelectionMultiple = false;
                        break;
                    case ListViewSelectionMode.Multiple:
                        this.SelectionUnNone = true;
                        this.SelectionSingle = false;
                        this.SelectionMultiple = true;
                        break;
                    default:
                        this.SelectionUnNone = false;
                        this.SelectionSingle = false;
                        this.SelectionMultiple = false;
                        break;
                }
                this.OnPropertyChanged(nameof(SelectionUnNone));//Notify 
                this.OnPropertyChanged(nameof(SelectionSingle));//Notify 
                this.OnPropertyChanged(nameof(SelectionMultiple));//Notify 


                this.selectionMode = value;
                this.OnPropertyChanged(nameof(SelectionMode));//Notify 
            }
        }
        private ListViewSelectionMode selectionMode;

        /// <summary>
        /// Gets selection-mode is not ""None"". 
        /// </summary>
        public bool SelectionUnNone { get; private set; }
        /// <summary>
        /// Gets selection-mode is ""Single"". 
        /// </summary>
        public bool SelectionSingle { get; private set; }
        /// <summary>
        /// Gets selection-mode is ""Multiple"". 
        /// </summary>
        public bool SelectionMultiple { get; private set; }


        //////////////////////////


        /// <summary> The single selected layerage. </summary>
        public Layerage SelectionLayerage { get; private set; }

        /// <summary> The all selected layerages. </summary>
        public IEnumerable<Layerage> SelectionLayerages { get; private set; }


        //////////////////////////


        /// <summary> Transformer of selection layers. </summary>
        public Transformer Transformer
        {
            get => this.transformer;
            set
            {
                this.transformer = value;
                this.OnPropertyChanged(nameof(Transformer));//Notify 
            }
        }
        private Transformer transformer;
        /// <summary> The cache of <see cref="ViewModel.Transformer"/>. </summary>
        public Transformer StartingTransformer { get; private set; }
        /// <summary> Cache <see cref="ViewModel.Transformer"/>. </summary>
        public void CacheTransformer() => this.StartingTransformer = this.Transformer;
                
    }
}