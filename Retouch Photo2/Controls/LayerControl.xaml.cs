using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;

        ObservableCollection<ILayer> _reference => this.ViewModel.Layers;
        ObservableCollection<ILayer> _selection => this.SelectionViewModel.Layer.Children;


        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;


        //@VisualState
        bool _vsIsBlends;
        bool _vsIsChildren;
        bool _vsIsGroupLayer;
        ListViewSelectionMode _vsMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsBlends) return this.Blends;
                if (this._vsIsChildren) return this.Children;

                switch (this.Mode)
                {
                    case ListViewSelectionMode.None: return this.Disable;
                    case ListViewSelectionMode.Single:
                        return this._vsIsGroupLayer ?
                            this.SingleLayerWithChildren : 
                            this.SingleLayerWithoutChildren;
                    case ListViewSelectionMode.Multiple: return this.MultipleLayer;
                }

                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        //@Converter
        private double OpacityToValueConverter(float opacity) => opacity * 100.0d;
        private float ValueToOpacityConverter(double value) => (float)value / 100.0f;

        private double VisibilityToOpacityConverter(Visibility visibility) => (visibility == Visibility.Visible) ? 1.0 : 0.4;
        private bool GroupLayerToboolConverter(GroupLayer groupLayer) => (groupLayer == null) ? false : true;

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsOverlayExpanded;
        public bool IsOverlayExpanded { private get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets the selection mode. </summary>
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
                con._vsIsBlends = false;
                con._vsIsChildren = false;

                con._vsMode = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        /// <summary> Gets or sets the current layer is GroupLayer. </summary>
        public bool IsGroupLayer
        {
            get { return (bool)GetValue(IsGroupLayerProperty); }
            set { SetValue(IsGroupLayerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "LayerControl.IsGroupLayer" /> dependency property. </summary>
        public static readonly DependencyProperty IsGroupLayerProperty = DependencyProperty.Register(nameof(IsGroupLayer), typeof(bool), typeof(LayerControl), new PropertyMetadata(false, (sender, e) =>
        {
            LayerControl con = (LayerControl)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsBlends = false;
                con._vsIsChildren = false;

                con._vsIsGroupLayer = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        public LayerControl()
        {
            this.InitializeComponent();

            //Menu
            this._MenuTitle.BackButton.Tapped += (s, e) =>
            {
                this._vsIsBlends = false;
                this._vsIsChildren = false;
                this.VisualState = this.VisualState;//State
            };


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
            this.BlendButton.Tapped += (s, e) =>
            {
                this.BlendControl.BlendType = this.SelectionViewModel.BlendType;

                this._vsIsBlends = true;
                this.VisualState = this.VisualState;//State
            };
            this.BlendControl.BlendTypeChanged += (s, blendType) =>
            {
                //Selection
                this.SelectionViewModel.BlendType = blendType;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.BlendType = blendType;
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Layers


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

                switch (this.SelectionViewModel.SelectionMode)
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


            //Group
            this.GroupButton.Tapped += (s, e) =>
            {
                //Menu
                this.TipViewModel.SetMenuState(MenuType.Layer, destinations: MenuState.FlyoutHide);

                //Transformer
                Transformer transformer = this.SelectionViewModel.Transformer;

                //GroupLayer
                GroupLayer groupLayer = new GroupLayer
                {
                    IsChecked = true,
                    TransformManager = new TransformManager(transformer),
                };


                //Index
                int index = this.MezzanineViewModel.GetfFrstIndex(this.ViewModel.Layers);
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                    groupLayer.Children.Add(layer);//Add

                    if (layer.TransformManager.DisabledRadian)
                    {
                        //DisabledRadian
                        groupLayer.TransformManager.DisabledRadian = true;
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


            //Remove
            this.RemoveButton.Tapped += (s, e) =>
            {
                this.ViewModel.RemoveLayers();//Remove

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();//Invalidate
            };


            #endregion


            #region Children


            this.ChildrenButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.Layer == null) return;

                this.ChildrenListView.ItemsSource = this.SelectionViewModel.Layer.Children;

                this._vsIsChildren = true;
                this.VisualState = this.VisualState;//State
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


            this.ChildrenListView.AllowDrop = true;

            this.ChildrenListView.CanDrag = false;
            this.ChildrenListView.CanDragItems = false;

            this.ChildrenListView.CanReorderItems = true;
            this.ChildrenListView.ReorderMode = ListViewReorderMode.Enabled;
            this.ChildrenListView.SelectionMode = ListViewSelectionMode.None;

            /// DragOver is called when the dragged pointer moves over a UIElement with AllowDrop=True
            /// We need to return an AcceptedOperation != None in either DragOver or DragEnter
            this.ChildrenListView.DragOver += (object sender, DragEventArgs e) =>
            {
                // Our list only accepts text
                e.AcceptedOperation = (e.DataView.Contains2(LayerDataPackageExpansion.DataFormat)) ? DataPackageOperation.Copy : DataPackageOperation.None;
            };

            /// We need to return the effective operation from Drop
            /// This is not important for our source ListView, but it might be if the user
            /// drags text from another source
            this.ChildrenListView.Drop += (object sender, DragEventArgs e) =>
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