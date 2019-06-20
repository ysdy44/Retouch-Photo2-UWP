using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Transformers;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public sealed partial class LayerControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.TestApp.App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => Retouch_Photo2.TestApp.App.MezzanineViewModel;
        TipViewModel TipViewModel => Retouch_Photo2.TestApp.App.TipViewModel;


        ObservableCollection<Layer> _reference => this.ViewModel.Layers;
        ObservableCollection<Layer> _selection => this.SelectionViewModel.Layer.Children;

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
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Transformer), typeof(ListViewSelectionMode), typeof(LayerControl), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
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
                            Layer cloneLayer = this.SelectionViewModel.Layer.Clone(this.ViewModel.CanvasDevice);//Clone

                            //IsChecked
                            this.SelectionViewModel.Layer.IsChecked = false;
                            cloneLayer.IsChecked = true;

                            this.ViewModel.Layers.Insert(index, cloneLayer);//Insert
                            this.SelectionViewModel.SetModeSingle(cloneLayer);//Selection
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            IEnumerable<Layer> cloneLayers = from cloneLayer in this.SelectionViewModel.Layers select cloneLayer.Clone(this.ViewModel.CanvasDevice);//Clone


                            //IsChecked
                            foreach (Layer layer in this.SelectionViewModel.Layers)
                            {
                                layer.IsChecked = true;
                            }
                            foreach (Layer cloneLayer in cloneLayers)
                            {
                                cloneLayer.IsChecked = true;
                            }

                            foreach (Layer cloneLayer in cloneLayers)
                            {
                                this.ViewModel.Layers.Insert(index, cloneLayer);//Insert
                            }
                            this.SelectionViewModel.SetModeMultiple(cloneLayers);//Selection
                        }
                        break;
                }
                this.SelectionViewModel.SetValue((layer) =>
                {
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            //Remove
            this.RemoveButton.Tapped += (s, e) =>
            {
                switch (this.SelectionViewModel.Mode)
                {
                    case ListViewSelectionMode.None:
                        break;
                    case ListViewSelectionMode.Single:
                        {
                            this.ViewModel.Layers.Remove(this.SelectionViewModel.Layer);

                            this.SelectionViewModel.SetModeNone();//Selection
                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;
                    case ListViewSelectionMode.Multiple:
                        {
                            foreach (Layer layer in this.SelectionViewModel.Layers)
                            {
                                this.ViewModel.Layers.Remove(this.SelectionViewModel.Layer);
                            }

                            this.SelectionViewModel.SetModeNone();//Selection
                            this.ViewModel.Invalidate();//Invalidate
                        }
                        break;
                }
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


                //TransformerMatrix
                TransformerMatrix transformerMatrix = new TransformerMatrix(this.SelectionViewModel.Transformer);
                //GroupLayer
                GroupLayer groupLayer = new GroupLayer
                {
                    IsChecked = true,
                    TransformerMatrix = transformerMatrix
                };


                //Index
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                    groupLayer.Children.Add(layer);//Add
                });


                foreach (Layer layer in groupLayer.Children)
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
                    foreach (Layer layer in groupLayer.Children)
                    {
                        layer.IsChecked = true;
                        this.ViewModel.Layers.Insert(index, layer);//Insert
                    }

                    //SetMode
                    IEnumerable<Layer> layers = groupLayer.Children;
                    Transformer transformer = groupLayer.TransformerMatrix.Destination;
                    this.SelectionViewModel.SetModeMultiple(layers, transformer);//Selection

                    this.ViewModel.Invalidate();//Invalidate
                }
            };


            #endregion


            #region Drag and Drop


            //Target
            this.ListView.AllowDrop = true;

            this.ListView.CanDrag = false;
            this.ListView.CanDragItems = false;

            this.ListView.CanReorderItems = true;
            this.ListView.ReorderMode = ListViewReorderMode.Disabled;
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
                    Layer getLayer = e.DataView.GetLayer();

                    if (getLayer != this.SelectionViewModel.Layer)
                    {
                        if (this._reference.Contains(getLayer))
                        {
                            this._reference.Remove(getLayer);
                            this._selection.Add(getLayer);
                        }
                    }
                    else this.ViewModel.Text = "No!!!!";

                    e.AcceptedOperation = DataPackageOperation.Copy;
                    def.Complete();
                }
            };
            #endregion

        }

        //@DataTemplate
        /// <summary> DataTemplate's Button Tapped. </summary>
        private void VisibilityButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            LayersControl.GetButtonDataContext(sender, out Grid rootGrid, out Layer layer);

            layer.Visibility = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

            this.ViewModel.Invalidate();//Invalidate

            e.Handled = true;
        }
    }
}