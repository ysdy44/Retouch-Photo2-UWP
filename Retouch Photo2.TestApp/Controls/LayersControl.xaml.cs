using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
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
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.TestApp.App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => Retouch_Photo2.TestApp.App.MezzanineViewModel;
        TipViewModel TipViewModel => Retouch_Photo2.TestApp.App.TipViewModel;


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();                       

            //Drag and Drop
            this.ListView.DragItemsStarting += (object s, DragItemsStartingEventArgs e) =>
            {
                Layer setLayer = null;
                foreach (object item in e.Items)
                {
                    if (item is Layer layer)
                    {
                        setLayer = layer;
                        break;
                    }
                }
                if (setLayer == null) return;

                /// Set the content of the <see cref = "DataPackage" />
                e.Data.SetLayer(setLayer);
                e.Data.RequestedOperation = DataPackageOperation.Copy;
            };
                                 

            this.AddButton.Tapped += async (s, e) =>
            {
                //File
                FileOpenPicker openPicker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                    FileTypeFilter =
                 {
                     ".jpg",
                     ".jpeg",
                     ".png",
                     ".bmp",
                 }
                };

                var file = await openPicker.PickSingleFileAsync();
                if (file == null) return;

                //ImageLayer
                Layer imageLayer = await ImageLayer.CreateFromFlie(this.ViewModel.CanvasDevice, file);
                imageLayer.IsChecked = true;

                //Selection
                this.SelectionViewModel.SetValue((layer)=> 
                {
                    layer.IsChecked = false;
                });

                //Insert
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                this.ViewModel.Layers.Insert(index, imageLayer);
 
                this.ViewModel.Invalidate();//Invalidate
            };
        } 
        

        //@DataTemplate
        /// <summary> DataTemplate's Grid Tapped. </summary>
        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LayersControl.GetGridDataContext(sender, out Grid rootGrid, out Layer layer);

            if (this.SelectionViewModel.Layer == layer) //FlyoutShow
            {
                if (this.TipViewModel.LayerMenuLayoutState == MenuLayoutState.FlyoutHide)
                {
                this.TipViewModel.LayerMenuLayoutState = MenuLayoutState.FlyoutShow;
                }
            }
            else //ItemClick
            {
                //Selection
                this.SelectionViewModel.SetValue((layer2) =>
                {
                    layer2.IsChecked = false;
                });

                layer.IsChecked = true;

                this.SelectionViewModel.SetModeSingle(layer);//Selection
                this.ViewModel.Invalidate();//Invalidate
            }
        }
    
        /// <summary> DataTemplate's Grid RightTapped. </summary>
        private void RootGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (this.TipViewModel.LayerMenuLayoutState == MenuLayoutState.FlyoutHide)
            {
                this.TipViewModel.LayerMenuLayoutState = MenuLayoutState.FlyoutShow;
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

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
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