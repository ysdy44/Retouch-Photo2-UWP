using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Layer Classes.
    /// </summary>
    public abstract partial class Layer : INotifyPropertyChanged
    {

        /// <summary> <see cref = "Layer" />'s IsChecked. </summary>
        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.isChecked = value;
                this.OnPropertyChanged(nameof(this.IsChecked));//Notify 
            }
        }
        private bool isChecked;

        /// <summary> <see cref = "Layer" />'s IsVisual. </summary>
        public bool IsVisual
        {
            get => this.isVisual;
            set
            {
                this.isVisual = value;
                this.OnPropertyChanged(nameof(this.IsVisual));//Notify 
            }
        }
        private bool isVisual = true;


        //@Delegate
        /// <summary> Occurs when an item in a list view receives an interaction. </summary>
        public static Action<Layer, FrameworkElement> ItemClickAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void ItemClick_Tapped(FrameworkElement placementTarget) => Layer.ItemClickAction?.Invoke(this, placementTarget);

        /// <summary> Occurs when an item in a list view rightTapped or holding. </summary>
        public static Action<Layer, FrameworkElement> FlyoutShowAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void FlyoutShow_Tapped(FrameworkElement placementTarget) => Layer.FlyoutShowAction?.Invoke(this, placementTarget);


        /// <summary> Occurs when the visual changes of an item in a list view. </summary>
        public static Action<Layer> ItemVisualChangedAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void IsVisualButton_Tapped(object sender, TappedRoutedEventArgs e) => Layer.ItemVisualChangedAction?.Invoke(this);

        /// <summary> Occurs when the value changes of an item in a list view. </summary>
        public static Action<Layer> ItemIsCheckedChangedAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void CheckBox_Tapped(object sender, TappedRoutedEventArgs e) => Layer.ItemIsCheckedChangedAction?.Invoke(this);


        //Notify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}