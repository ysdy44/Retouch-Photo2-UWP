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
        

        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;
                

        //@VisualState
        bool _vsIsBlends;
        bool _vsIsGroupLayer;
        ListViewSelectionMode _vsMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsBlends) return this.Blends;

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

        private bool VisibilityToBoolConverter(Visibility visibility) => visibility == Visibility.Visible;
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
                this.VisualState = this.VisualState;//State
            };


            #region Property


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
                var layers = this.ViewModel.Layers.GetAllSelectedLayers();
                this.ViewModel.Layers.MezzanineRangeOnFirstSelectedLayer(layers);
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                this.ViewModel.Invalidate();//Invalidate
            };

            //Remove
            this.RemoveButton.Tapped += (s, e) =>
            {
                this.ViewModel.Layers.RemoveAllSelectedLayers();
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                this.ViewModel.Invalidate();//Invalidate
            };


            //Group
            this.GroupButton.Tapped += (s, e) =>
            {
                this.ViewModel.Layers.GroupAllSelectedLayers();
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                this.ViewModel.Invalidate();//Invalidate
            };


            //UnGroup
            this.UnGroupButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.Layer!=null)
                {
                    if (this.SelectionViewModel.Layer is GroupLayer groupLayer)
                    {
                        ILayer parent = groupLayer.Parents;
                        IList<ILayer> parentChildren = (parent == null) ? this.ViewModel.Layers.RootLayers : parent.Children;

                        int index = parentChildren.IndexOf(groupLayer);
                        parentChildren.Remove(groupLayer);

                        foreach (ILayer child in groupLayer.Children)
                        {
                            child.Parents = parent;
                            child.SelectMode = SelectMode.Selected;
                            parentChildren.Insert(index, child);
                        }

                        this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                        this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };

            #endregion

        }
        
    }
}