using FanKit.Transformers;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary>
        ///  selection-count 
        /// </summary>
        public int SelectionCount
        {
            get => this.selectionCount;
            private set
            {
                this.selectionCount = value;
                this.OnPropertyChanged(nameof(this.SelectionCount));//Notify 
            }
        }
        private int selectionCount;
        
        /// <summary>
        /// Gets selection-mode by count of checked layers. 
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
                this.selectionMode = value;
                this.OnPropertyChanged(nameof(this.SelectionMode));//Notify 
            }
        }
        private ListViewSelectionMode selectionMode;

        /// <summary>
        /// Gets selection-mode is not ""None"". 
        /// </summary>
        public bool SelectionUnNone
        {
            get => this.selectionUnNone;
            private set
            {
                this.selectionUnNone = value;
                this.OnPropertyChanged(nameof(this.SelectionUnNone));//Notify 
            }
        }
        private bool selectionUnNone;

        /// <summary>
        /// Gets selection-mode is ""Single"". 
        /// </summary>
        public bool SelectionSingle
        {
            get => this.selectionSingle;
            private set
            {
                this.selectionSingle = value;
                this.OnPropertyChanged(nameof(this.SelectionSingle));//Notify 
            }
        }
        private bool selectionSingle;
        

        /// <summary> The single checked layer. </summary>
        public ILayer Layer { get; private set; }
        
        /// <summary> The all checked layers. </summary>
        public IEnumerable<ILayer> Layers { get; private set; }
        

        /// <summary> Transformer of selection layers. </summary>
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

        /// <summary> Is disable rotate radian? Defult **false**. </summary>
        public bool DisabledRadian
        {
            get => this.disabledRadian;
            set
            {
                this.disabledRadian = value;
                this.OnPropertyChanged(nameof(this.DisabledRadian));//Notify 
            }
        }
        private bool disabledRadian;
        

        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}