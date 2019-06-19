using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();
            if (this.ViewModel.LayerPlacementTarget == null)
            {
                this.ViewModel.LayerPlacementTarget = this;
            }

            this.AddButton.Tapped += (s, e) =>
            {

            };
        } 
        

        //@DataTemplate
        /// <summary> DataTemplate's Grid Tapped. </summary>
        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LayersControl.GetGridDataContext(sender, out Grid rootGrid, out Layer layer);

            if (this.Selection.Layer == layer) //FlyoutShow
            {
                if (this.ViewModel.LayerMenuLayoutState == MenuLayoutState.FlyoutHide)
                {
                    this.ViewModel.LayerPlacementTarget = rootGrid;
                    this.ViewModel.LayerMenuLayoutState = MenuLayoutState.FlyoutShow;
                }
                else if (this.ViewModel.LayerMenuLayoutState == MenuLayoutState.FlyoutShow)
                {
                    this.ViewModel.LayerMenuLayoutState = MenuLayoutState.FlyoutHide;
                    this.ViewModel.LayerPlacementTarget = rootGrid;
                    this.ViewModel.LayerMenuLayoutState = MenuLayoutState.FlyoutShow;
                }
            }
            else  //ItemClick
            {             
                //Selection
                this.Selection.SetValue((layer2) =>
                {
                    layer2.IsChecked = false;
                });

                layer.IsChecked = true;

                this.Selection.SetModeSingle(layer);//Selection
                this.ViewModel.Invalidate();//Invalidate
            }
        }
    
        /// <summary> DataTemplate's Grid RightTapped. </summary>
        private void RootGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (this.ViewModel.LayerMenuLayoutState == MenuLayoutState.FlyoutHide)
            {
                this.ViewModel.LayerMenuLayoutState = MenuLayoutState.FlyoutShow;
            }
        }
     
        /// <summary> DataTemplate's Button Tapped. </summary>
        private void VisibilityButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out Layer layer);

            layer.Visibility = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

            this.ViewModel.Invalidate();//Invalidate

            e.Handled = true;
        }
       
        /// <summary> DataTemplate's CheckBox Tapped. </summary>
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        { 
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out Layer layer);
            
            layer.IsChecked = !layer.IsChecked;

            this.Selection.SetMode(this.ViewModel.Layers);//Selection
            this.ViewModel.Invalidate();//Invalidate

            e.Handled = true;
        }
        


        //@Static
        /// <summary>
        /// Get the data context of the Grid.
        /// </summary>
        /// <param name="senderGrid"> Grid. </param>
        /// <param name="rootGrid"> DataTemplate. </param>
        /// <param name="layer"> DataContext. </param>
        public static void GetGridDataContext(object senderGrid, out Grid rootGrid, out Layer layer)
        {
            if (senderGrid is Grid rootGrid2)
            {
                if (rootGrid2.DataContext is Layer layer2)
                {
                    rootGrid = rootGrid2;
                    layer = layer2;
                    return;
                }
            }

            rootGrid = null;
            layer = null;
        }
        /// <summary>
        /// Get the data context of the Grid's Button.
        /// </summary>
        /// <param name="senderButton"> Button. </param>
        /// <param name="rootGrid"> DataTemplate. </param>
        /// <param name="layer"> DataContext. </param>
        public static void GetButtonDataContext(object senderButton, out Grid rootGrid, out Layer layer)
        {
            if (senderButton is Button button)
            {
                if (button.Parent is Grid rootGrid2)
                {
                    if (rootGrid2.DataContext is Layer layer2)
                    {
                        rootGrid = rootGrid2;
                        layer = layer2;
                        return;
                    }
                }
            }


            rootGrid = null;
            layer = null;
        }

    }
}