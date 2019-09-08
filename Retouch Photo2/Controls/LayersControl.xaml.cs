using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System;
using System.Numerics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayersControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();

            this.ViewModel.Layers.CollectionChanged += (s, e) =>
            {
               if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    this.ViewModel.Invalidate();//Invalidate
                }
            };

            this.AddButton.Tapped += async (s, e) =>
            {
                //ImageRe
                ImageRe imageRe = await ImageRe.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, PickerLocationId.PicturesLibrary);
                if (imageRe == null) return;

               //Images
                this.ViewModel.DuplicateChecking(imageRe);

                //Transformer
                Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

                //Layer
                ImageLayer imageLayer = new ImageLayer
                {
                    ImageRe= imageRe,
                    Source = transformerSource,
                    Destination= transformerSource,
                    IsChecked = true
                };

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                });

                //Insert
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                this.ViewModel.Layers.Insert(index, imageLayer);

                this.SelectionViewModel.SetModeSingle(imageLayer);//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            #region Drag and Drop


            this.ListView.AllowDrop = true;

            this.ListView.CanDrag = false;
            this.ListView.CanDragItems = false;

            this.ListView.CanReorderItems = true;
            this.ListView.ReorderMode = ListViewReorderMode.Enabled;
            this.ListView.SelectionMode = ListViewSelectionMode.None;

            this.ListView.DragItemsStarting += (object s, DragItemsStartingEventArgs e) =>
            {
                ILayer setLayer = null;
                foreach (object item in e.Items)
                {
                    if (item is ILayer layer)
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


            #endregion
        } 
        

        //@DataTemplate
        /// <summary> DataTemplate's Grid Tapped. </summary>
        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {            //TODO:LayerMenuState

      //      LayersControl.GetGridDataContext(sender, out Grid rootGrid, out ILayer layer);

        //    if (this.SelectionViewModel.Layer == layer) //FlyoutShow
       //     {            //TODO:LayerMenuState

         //       if (this.TipViewModel.LayerMenuState == MenuState.FlyoutHide)
          //      {
            //        this.TipViewModel.LayerMenuState = MenuState.FlyoutShow;
            //    }
     //       }
         //   else //ItemClick
         //   {
          //      //Selection
         //       this.SelectionViewModel.SetValue((layer2) =>
         //       {
          //          layer2.IsChecked = false;
           //     });

          //      layer.IsChecked = true;

         //       this.SelectionViewModel.SetModeSingle(layer);//Selection
           //     this.ViewModel.Invalidate();//Invalidate
      //      }
        }
    
        /// <summary> DataTemplate's Grid RightTapped. </summary>
        private void RootGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //TODO:LayerMenuState
            //    if (this.TipViewModel.LayerMenuState == MenuState.FlyoutHide)
            //    {
            //      this.TipViewModel.LayerMenuState = MenuState.FlyoutShow;
    //    }
    }
     
        /// <summary> DataTemplate's Button Tapped. </summary>
        private void VisibilityButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out ILayer layer);

            layer.Visibility = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

            this.ViewModel.Invalidate();//Invalidate

            e.Handled = true;
        }
       
        /// <summary> DataTemplate's CheckBox Tapped. </summary>
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        { 
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out ILayer layer);
            
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
        public static void GetGridDataContext(object senderGrid, out Grid rootGrid, out ILayer layer)
        {
            if (senderGrid is Grid rootGrid2)
            {
                if (rootGrid2.DataContext is ILayer layer2)
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
        public static void GetButtonDataContext(object senderButton, out Grid rootGrid, out ILayer layer)
        {
            if (senderButton is Button button)
            {
                if (button.Parent is Grid rootGrid2)
                {
                    if (rootGrid2.DataContext is ILayer layer2)
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