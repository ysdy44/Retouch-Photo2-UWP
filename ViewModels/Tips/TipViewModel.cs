using System.ComponentModel;

namespace ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {



        /// <summary> Retouch_Photo2's the only <see cref = "TipViewModel.IsOpen" />. </summary>
        public bool IsOpen
        {
            get => this.isOpen;
            set
            {
                this.SetIsOpen(value);

                this.isOpen = value;
                this.OnPropertyChanged(nameof(this.IsOpen));//Notify 
            }
        }
        private bool isOpen;



        private void SetIsOpen(bool isOpen)
        {
            //Tool
            if (this.Tool==this.CursorTool)
                this.IsCursorToolOpen = isOpen;


            //MenuLayoutState
            if ( this.LayerMenuLayoutState == Elements.MenuLayoutState.RootExpanded)
                this.IsLayerMenuLayoutOpen = isOpen;

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
