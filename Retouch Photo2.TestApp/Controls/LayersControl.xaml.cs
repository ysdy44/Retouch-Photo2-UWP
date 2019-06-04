using Retouch_Photo2.Layers;
using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayersControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        public LayersControl()
        {
            this.InitializeComponent();

            //Layer : ItemClick
            Layer.ItemClickAction = (layer, placementTarget) =>
            {
                foreach (Layer item in this.ViewModel.Layers)
                {
                    item.IsChecked = (item == layer);
                }
                  
                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : ItemVisualChanged
            Layer.ItemVisualChangedAction = (layer) =>
            {
                layer.IsVisual = !layer.IsVisual;
                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : ItemIsCheckedChanged
            Layer.ItemIsCheckedChangedAction = (layer) =>
            {
                layer.IsChecked = !layer.IsChecked;
                this.ViewModel.Invalidate();//Invalidate
            };

            this.AddButton.Tapped += (s, e) =>
            {

            };
        }

        private void CheckBox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void CheckBox_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}