using Retouch_Photo2.Layers;
using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Media3D;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayersControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel Selection => Retouch_Photo2.TestApp.App.Selection;
        KeyboardViewModel Keyboard => Retouch_Photo2.TestApp.App.Keyboard;


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();

            //Layer : ItemClick
            Layer.ItemClickAction = (itemClickLayer, placementTarget) =>
            {
                //Selection
                this.Selection.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                });
                
                itemClickLayer.IsChecked = true;

                this.Selection.SetModeSingle(itemClickLayer);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : FlyoutShow
            Layer.FlyoutShowAction = (layer, placementTarget) =>
            {

            };

            //Layer : ItemVisibilityChangedAction
            Layer.ItemVisibilityChangedAction = (visualLayer) =>
            {
                visualLayer.Visibility = (visualLayer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                this.ViewModel.Invalidate();//Invalidate
            };

            //Layer : ItemIsCheckedChanged
            Layer.ItemIsCheckedChangedAction = (layer) =>
            {
                layer.IsChecked = !layer.IsChecked;

                this.Selection.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            this.AddButton.Tapped += (s, e) =>
            {

            };
        }


    }
}