using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayerControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        ObservableCollection<ILayer> _reference => this.ViewModel.Layers;
        ObservableCollection<ILayer> _selection => this.SelectionViewModel.Layer.Children;

        //@Converter
        private double OpacityToValueConverter(float opacity) => opacity * 100.0d;
        private float ValueToOpacityConverter(double value) => (float)value / 100.0f;

        private double VisibilityToOpacityConverter(Visibility visibility) => (visibility == Visibility.Visible) ? 1.0 : 0.4;
        private bool GroupLayerToBoolConverter(GroupLayer groupLayer) => (groupLayer == null) ? false : true;


        #region DependencyProperty
        
        /// <summary> Gets or sets <see cref = "LayerControl" />'s selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "LayerControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(LayerControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            LayerControl con = (LayerControl)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
                switch (value)
                {
                    case ListViewSelectionMode.None:
                        {
                            con.OpacitySlider.IsEnabled = false;
                            con.BlendControl.IsEnabled = false;
                            con.VisualButton.IsEnabled = false;
                            con.DuplicateButton.IsEnabled = false;
                            con.RemoveButton.IsEnabled = false;
                        }
                        break;
                    case ListViewSelectionMode.Single:
                    case ListViewSelectionMode.Multiple:
                        {
                            con.OpacitySlider.IsEnabled = true;
                            con.BlendControl.IsEnabled = true;
                            con.VisualButton.IsEnabled = true;
                            con.DuplicateButton.IsEnabled = true;
                            con.RemoveButton.IsEnabled = true;
                        }
                        break;
                }

                switch (value)
                {
                    case ListViewSelectionMode.None:
                    case ListViewSelectionMode.Single:
                        {
                            con.GroupButton.IsEnabled = false;
                        }
                        break;

                    case ListViewSelectionMode.Multiple:
                        {
                            con.GroupButton.IsEnabled = true;
                        }
                        break;

                }

                switch (value)
                {
                    case ListViewSelectionMode.Single:
                        {
                            con.ChildrenTextBlock.Visibility = Visibility.Visible;
                            con.ChildrenBorder.Visibility = Visibility.Visible;
                        }
                        break;
                    case ListViewSelectionMode.None:
                    case ListViewSelectionMode.Multiple:
                        {
                            con.ChildrenTextBlock.Visibility = Visibility.Collapsed;
                            con.ChildrenBorder.Visibility = Visibility.Collapsed;
                        }
                        break;
                }
            }
        }));
        
        #endregion


        //@Construct
        public LayerControl()
        {
            this.InitializeComponent();


            #region Layer


            //Opacity
            this.OpacitySlider.ValueChanged += (s, e) =>
            {
                float opacity = this.ValueToOpacityConverter(e.NewValue);

                //Selection
                this.SelectionViewModel.Opacity = opacity;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Opacity = opacity;
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            //Blend
            this.BlendControl.TypeChanged += (type) =>
            {
                //Selection
                this.SelectionViewModel.BlendType = type;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.BlendType = type;
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Layer


            //Visual
            this.VisualButton.Tapped += (s, e) =>
            {
                Visibility value = (this.SelectionViewModel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                //Selection
                this.SelectionViewModel.Visibility = value;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Visibility = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            //Duplicate
            this.DuplicateButton.Tapped += (s, e) =>
            {
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);

                switch (this.SelectionViewModel.Mode)
                {
                    case ListViewSelectionMode.None:
                        break;
                    case ListViewSelectionMode.Single:
                        {
                            //Clone
                            ILayer cloneLayer = this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice);

                            //IsChecked
                            this.SelectionViewModel.Layer.IsChecked = false;

                            //Insert
                            this.ViewModel.Layers.Insert(index, cloneLayer);

                            this.SelectionViewModel.SetModeSingle(cloneLayer);//Selection
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            List<ILayer> cloneLayers = new List<ILayer>();

                            foreach (ILayer layer in this.SelectionViewModel.Layers)
                            {
                                //Clone
                                cloneLayers.Add(layer.Clone(this.ViewModel.CanvasDevice));

                                //IsChecked
                                layer.IsChecked = false;
                            }

                            for (int i = 0; i < cloneLayers.Count; i++)
                            {
                                //Insert
                                this.ViewModel.Layers.Insert(index, cloneLayers[i]);//Insert
                            }

                            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                        }
                        break;
                }
                
                this.ViewModel.Invalidate();//Invalidate
            };


            //Remove
            this.RemoveButton.Tapped += (s, e) =>
            {
                this.ViewModel.RemoveLayers();//Remove

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Group


            //Group
            this.GroupButton.Tapped += (s, e) =>
            {
                if (this.TipViewModel.LayerMenuLayoutState == Elements.MenuLayoutState.FlyoutShow)
                {
                    this.TipViewModel.LayerMenuLayoutState = Elements.MenuLayoutState.FlyoutHide;
                }


                //Transformer
                Transformer transformer = this.SelectionViewModel.Transformer;

                //GroupLayer
                GroupLayer groupLayer = new GroupLayer
                {
                    IsChecked = true,

                    Source = transformer,
                    Destination = transformer,
                    DisabledRadian = false//DisabledRadian
                };


                //Index
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                    groupLayer.Children.Add(layer);//Add

                    if (layer.DisabledRadian)
                    {
                        groupLayer.DisabledRadian = true;//DisabledRadian
                    }
                });


                foreach (ILayer layer in groupLayer.Children)
                {
                    this.ViewModel.Layers.Remove(layer);//Remove
                }

                this.SelectionViewModel.SetModeSingle(groupLayer);//Selection
                this.ViewModel.Layers.Insert(index, groupLayer);//Insert
                this.ViewModel.Invalidate();//Invalidate
            };


            //UnGroup
            this.UnGroupButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.IsGroupLayer == false) return;

                if (this.SelectionViewModel.Layer is GroupLayer groupLayer)
                {
                    //Index
                    int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);

                    //Selection
                    this.SelectionViewModel.SetValue((layer) =>
                    {
                        layer.IsChecked = false;
                    });

                    //Insert
                    this.ViewModel.Layers.Remove(groupLayer);
                    foreach (ILayer layer in groupLayer.Children)
                    {
                        layer.IsChecked = true;
                        this.ViewModel.Layers.Insert(index, layer);//Insert
                    }

                    //SetMode
                    IEnumerable<ILayer> layers = groupLayer.Children;
                    this.SelectionViewModel.SetModeMultiple(layers);//Selection

                    this.ViewModel.Invalidate();//Invalidate
                }
            };


            #endregion


            #region Drag and Drop


            this.ListView.AllowDrop = true;

            this.ListView.CanDrag = false;
            this.ListView.CanDragItems = false;

            this.ListView.CanReorderItems = true;
            this.ListView.ReorderMode = ListViewReorderMode.Enabled;
            this.ListView.SelectionMode = ListViewSelectionMode.None;

            /// DragOver is called when the dragged pointer moves over a UIElement with AllowDrop=True
            /// We need to return an AcceptedOperation != None in either DragOver or DragEnter
            this.ListView.DragOver += (object sender, DragEventArgs e) =>
            {
                // Our list only accepts text
                e.AcceptedOperation = (e.DataView.Contains2(LayerDataPackageExpansion.DataFormat)) ? DataPackageOperation.Copy : DataPackageOperation.None;
            };

            /// We need to return the effective operation from Drop
            /// This is not important for our source ListView, but it might be if the user
            /// drags text from another source
            this.ListView.Drop += (object sender, DragEventArgs e) =>
            {
                // This test is in theory not needed as we returned DataPackageOperation.None if
                // the DataPackage did not contained text. However, it is always better if each
                // method is robust by itself
                if (e.DataView.Contains2(LayerDataPackageExpansion.DataFormat))
                {
                    // We need to take a Deferral as we won't be able to confirm the end
                    // of the operation synchronously
                    DragOperationDeferral def = e.GetDeferral();
                    ILayer getLayer = e.DataView.GetLayer();

                    if (getLayer != this.SelectionViewModel.Layer)
                    {
                        if (this._reference.Contains(getLayer))
                        {
                            this._reference.Remove(getLayer);
                            this._selection.Add(getLayer);
                        }
                    }

                    e.AcceptedOperation = DataPackageOperation.Copy;
                    def.Complete();
                }

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion

        }

        //@DataTemplate
        /// <summary> DataTemplate's Button Tapped. </summary>
        private void VisibilityButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out ILayer layer);

            layer.Visibility = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

            this.ViewModel.Invalidate();//Invalidate

            e.Handled = true;
        }
    }
}