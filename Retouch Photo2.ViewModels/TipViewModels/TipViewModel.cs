// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains <see cref="ITool"/> Menu and <see cref="ToolTip.IsOpen"/>
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {


        /// <summary> Gets or sets the <see cref="ToolTip.IsOpen"/>. </summary>
        public bool IsOpen
        {
            get => this.isOpen;
            set
            {
                this.isOpen = value;
                this.OnPropertyChanged(nameof(this.IsOpen));//Notify 
            }
        }
        private bool isOpen;


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