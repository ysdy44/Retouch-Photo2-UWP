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
        //DataContext
        public void ConstructDataContext(string path, DependencyProperty dp)
        {
            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.SetBinding(dp, binding);
        }

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


            //Blend Mode
            this.BlendModeButton.Tapped += (s, e) =>
            {
                this.BlendModeComboBox.Mode = this.SelectionViewModel.BlendMode;

                this._Expander.IsSecondPage = true;
            };
            this.BlendModeComboBox.ModeChanged += (s, mode) =>
            {
                //Selection
                this.SelectionViewModel.BlendMode = mode;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.BlendMode = mode;
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            //Visibility
            this.VisibilityButton.Tapped += (s, e) =>
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


            //Tag Type
            this.TagTypeControl.TypeChanged += (s, type) =>
            {
                //Selection
                this.SelectionViewModel.TagType = type;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.TagType = type;
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
            
        }

    }
}
