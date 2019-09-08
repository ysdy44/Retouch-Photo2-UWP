using Retouch_Photo2.Tools;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels.Tips
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

        //@Construct
        public TipViewModel(ITool tool)
        {
            this.tool = tool;
        }


        /// <summary> Retouch_Photo2's the only <see cref = "TipViewModel.IsOpen" />. </summary>
        public bool IsOpen
        {
            get => this.isOpen;
            set
            {
                this.SetToolIsOpen(value);//Tool
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