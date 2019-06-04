using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Models
{
    public abstract partial class Layer
    { 

        //Converter
        public double BoolToOpacityConverter(bool isChecked) => isChecked ? 1.0 : 0.5;
        public Visibility BoolToVisibilityConverter(bool isChecked) => isChecked ? Visibility.Visible : Visibility.Collapsed;


        /// <summary> IsChecked </summary>    
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
        
        /// <summary> IsVisual </summary>    
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
        
        /// <summary> Text </summary>      
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                this.OnPropertyChanged(nameof(this.Text));//Notify 
            }
        }
        private string text = "Layer";

        //Delegate
        public delegate void LayerItemClickHandler(Layer layer, FrameworkElement placementTarget);
        public static event LayerItemClickHandler LayerItemClick = null;
        public void RootGrid_Tapped(object sender, TappedRoutedEventArgs e) => Layer.LayerItemClick?.Invoke(this, (FrameworkElement)sender);
        public void VisualButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.IsVisual = !this.IsVisual;
            this.ViewModel.Invalidate();
            e.Handled = true;
        }
        
    }
}
