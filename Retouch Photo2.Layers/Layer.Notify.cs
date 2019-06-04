using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// <see cref="LayersControl"/>items's Model Class.
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
        /// <summary>
        /// Occurs when an item in a list view receives an interaction.
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="placementTarget"></param>
        public static void ItemClick(Layer layer, FrameworkElement placementTarget) => Layer.ItemClickAction?.Invoke(layer, placementTarget);
        /// <summary> <see cref = "Action" /> of the <see cref = "Layer.ItemClick" />. </summary>
        public static Action<Layer, FrameworkElement> ItemClickAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void RootGrid_Tapped(object sender, TappedRoutedEventArgs e) => Layer.ItemClick(this, (FrameworkElement)sender);

        /// <summary>
        /// Occurs when the visual changes of an item in a list view.
        /// </summary>
        /// <param name="layer"></param>
        public static void ItemVisualChanged(Layer layer) => Layer.ItemVisualChangedAction?.Invoke(layer);
        /// <summary> <see cref = "Action" /> of the <see cref = "Layer.ItemVisualChanged" />. </summary>
        public static Action<Layer> ItemVisualChangedAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void IsVisualButton_Tapped(object sender, TappedRoutedEventArgs e) => Layer.ItemVisualChanged(this);
        
        /// <summary>
        /// Occurs when the value changes of an item in a list view.
        /// </summary>
        /// <param name="layer"></param>
        public static void ItemIsCheckedChanged(Layer layer) => Layer.ItemIsCheckedChangedAction?.Invoke(layer);
        /// <summary> <see cref = "Action" /> of the <see cref = "Layer.ItemIsCheckedChanged" />. </summary>
        public static Action<Layer> ItemIsCheckedChangedAction { private get; set; }
        /// <summary> Event of <see cref = "DataTemplate" />. </summary>
        public void CheckBox_Tapped(object sender, TappedRoutedEventArgs e) => Layer.ItemIsCheckedChanged(this);


        //Notify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}