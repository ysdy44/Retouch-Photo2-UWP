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

        //@Construct
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

                this.ViewModel.SelectionSingle(layer);//Selection

                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : FlyoutShow
            Layer.FlyoutShowAction = (layer, placementTarget) =>
            {

            };

            //Layer : ItemVisualChanged
            Layer.ItemVisualChangedAction = (visualLayer) =>
            {
                bool isVisual = !visualLayer.IsVisual;

                if (visualLayer.IsChecked == false)
                {
                    visualLayer.IsVisual = isVisual;
                }
                else
                {
                    foreach (Layer layer in this.ViewModel.Layers)
                    {
                        if (layer.IsChecked)
                        {
                            layer.IsVisual = isVisual;
                        }
                    }
                }

                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : ItemIsCheckedChanged
            Layer.ItemIsCheckedChangedAction = (layer) =>
            {
                layer.IsChecked = !layer.IsChecked;

                this.ViewModel.SetSelection();//Selection

                this.ViewModel.Invalidate();//Invalidate
            };


            this.AddButton.Tapped += (s, e) =>
            {

            };
        }


    }
}