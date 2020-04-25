using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayerMenu" />. 
    /// </summary>
    public sealed partial class LayerMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
         

        //@Converter
        private double OpacityToValueConverter(float opacity) => opacity * 100.0d;
        private float ValueToOpacityConverter(double value) => (float)value / 100.0f;

        private bool VisibilityToBoolConverter(Visibility visibility) => visibility == Visibility.Visible;
        private bool GroupLayerToboolConverter(GroupLayer groupLayer) => (groupLayer == null) ? false : true;
        

        //@VisualState
        bool _vsIsGroupLayer;
        ListViewSelectionMode _vsMode;
        public VisualState VisualState
        {
            get
            {
                switch (this.Mode)
                {
                    case ListViewSelectionMode.None:
                        return this.Disable;
                    case ListViewSelectionMode.Single:
                        {
                            return this._vsIsGroupLayer ?
                                this.SingleLayerWithChildren :
                                this.SingleLayerWithoutChildren;
                        }
                    case ListViewSelectionMode.Multiple:
                        return this.MultipleLayer;
                }

                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        
        #region DependencyProperty


        /// <summary> Gets or sets the selection mode. </summary>
        public ListViewSelectionMode Mode
        {
            get { return (ListViewSelectionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "LayerMenu.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ListViewSelectionMode), typeof(LayerMenu), new PropertyMetadata(ListViewSelectionMode.None, (sender, e) =>
        {
            LayerMenu con = (LayerMenu)sender;

            if (e.NewValue is ListViewSelectionMode value)
            {
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
        /// <summary> Identifies the <see cref = "LayerMenu.IsGroupLayer" /> dependency property. </summary>
        public static readonly DependencyProperty IsGroupLayerProperty = DependencyProperty.Register(nameof(IsGroupLayer), typeof(bool), typeof(LayerMenu), new PropertyMetadata(false, (sender, e) =>
        {
            LayerMenu con = (LayerMenu)sender;

            if (e.NewValue is bool value)
            {
                con._vsIsGroupLayer = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion
         

        //@Construct
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.ConstructLayer();
            this.ConstructLayers();
        }

    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayerMenu" />. 
    /// </summary>
    public sealed partial class LayerMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._Expander.Title = resource.GetString("/Menus/Layer");
            
            this.OpacityTextBlock.Text = resource.GetString("/Menus/Layer_Opacity");

            this.BlendModeTextBlock.Text = resource.GetString("/Menus/Layer_BlendMode");

            this.LayersTextBlock.Text = resource.GetString("/Menus/Layer_Layers");

            this.RemoveButton.Content = resource.GetString("/Menus/Layer_Remove");
            this.DuplicateButton.Content = resource.GetString("/Menus/Layer_Duplicate");

            this.GroupButton.Content = resource.GetString("/Menus/Layer_Group");
            this.UnGroupButton.Content = resource.GetString("/Menus/Layer_UnGroup");

            this.TagTypeTextBlock.Text = resource.GetString("/Menus/Layer_TagType");
        }
        
        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }


        //@Content
        public MenuType Type => MenuType.Layer;
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Left;
        public Point Postion { get; set; }
        public FrameworkElement Layout => this;
        public FrameworkElement Button { get; } = new Border();
        
        public MenuState State
        {
            get => this.state;
            set
            {
                this._Expander.State = value;
                MenuHelper.SetMenuState(value, this);
                this.state = value;
            }
        }
        private MenuState state;


        //@Construct  
        public void ConstructMenu()
        {
            this.State = MenuState.Hide;
            this._Expander.CloseButton.Tapped += (s, e) => this.State = MenuState.Hide;
            this._Expander.StateButton.Tapped += (s, e) => this.State = MenuHelper.GetState2(this.State);
            this._Expander.ResetButton.Visibility = Visibility.Collapsed;
            this._Expander.BackButton.Tapped += (s, e) => this._Expander.IsSecondPage = false;
            MenuHelper.ConstructTitleGrid(this._Expander.TitleGrid, this);
        }


    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayerMenu" />. 
    /// </summary>
    public sealed partial class LayerMenu : UserControl, IMenu
    {
        
        //Strings
        private void ConstructLayer()
        {
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

                this._Expander.IsSecondPage = true;
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


            //TagType
            this.TagControl.TagTypeChanged += (s, tagType) =>
            {
                //Selection
                this.SelectionViewModel.TagType = tagType;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TagType = tagType;
                });
            };

        }

        //Strings
        private void ConstructLayers()
        {

            //Duplicate
            this.DuplicateButton.Tapped += (s, e) =>
            {
                IList<ILayer> layers = this.ViewModel.Layers.GetAllSelectedLayers();
                IEnumerable<ILayer> duplicateLayers = from i in layers select i.Clone(this.ViewModel.CanvasDevice);

                this.ViewModel.Layers.MezzanineRangeOnFirstSelectedLayer(duplicateLayers.ToList());
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
                if (this.SelectionViewModel.Layer != null)
                {
                    if (this.SelectionViewModel.Layer is GroupLayer groupLayer)
                    {
                        ILayer parent = groupLayer.Parents;
                        IList<ILayer> parentChildren = (parent == null) ? this.ViewModel.Layers.RootLayers : parent.Children;

                        int index = parentChildren.IndexOf(groupLayer);

                        foreach (ILayer child in groupLayer.Children)
                        {
                            child.Parents = parent;
                            child.SelectMode = SelectMode.Selected;
                            parentChildren.Insert(index, child);
                        }
                        groupLayer.Children.Clear();
                        parentChildren.Remove(groupLayer);

                        this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

                        this.SelectionViewModel.SetMode(this.ViewModel.Layers);
                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };

        }

    }
}
