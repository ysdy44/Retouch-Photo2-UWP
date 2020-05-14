using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
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
using Windows.UI.Xaml.Input;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Historys.Models;

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
        private bool VisibilityToBoolConverter(Visibility visibility) => visibility == Visibility.Visible;


        //@Construct
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.ConstructOpacity();
            this.ConstructBlendMode();
            this.ConstructVisibility();
            this.ConstructTagType();

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


        //Menu
        public MenuType Type => MenuType.Layer;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            Visibility = Visibility.Collapsed
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.PlacementMode = FlyoutPlacementMode.Left;
            this._Expander.Initialize();
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayerMenu" />. 
    /// </summary>
    public sealed partial class LayerMenu : UserControl, IMenu
    {

        //Opacity
        private void ConstructOpacity()
        {
            this.OpacitySlider.Minimum = 0;
            this.OpacitySlider.Maximum = 1;
            this.OpacitySlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.CacheOpacity();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float opacity = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Opacity = opacity;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value;

                //History
                OpacityHistory history = new OpacityHistory();
                this.ViewModel.Push(history);

                //Selection
                this.SelectionViewModel.Opacity = opacity;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    history.Add(layer, layer.StartingOpacity, opacity);

                    layer.Opacity = opacity;
                });

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };

        }
        
        //Blend Mode
        private void ConstructBlendMode()
        {
            this.BlendModeButton.Tapped += (s, e) =>
            {
                this.BlendModeComboBox.Mode = this.SelectionViewModel.BlendMode;

                this._Expander.IsSecondPage = true;
            };
            this.BlendModeComboBox.ModeChanged += (s, mode) =>
            {
                //History
                BlendModeHistory history = new BlendModeHistory();
                this.ViewModel.Push(history);

                //Selection
                this.SelectionViewModel.BlendMode = mode;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    history.Add(layer, layer.BlendMode, mode);

                    layer.BlendMode = mode;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        //Visibility
        private void ConstructVisibility()
        {
            this.VisibilityButton.Tapped += (s, e) =>
            {
                Visibility value = (this.SelectionViewModel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                //History
                VisibilityHistory history = new VisibilityHistory();
                this.ViewModel.Push(history);

                //Selection
                this.SelectionViewModel.Visibility = value;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    history.Add(layer, layer.Visibility, value);

                    layer.Visibility = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        //Tag Type
        private void ConstructTagType()
        {
            this.TagTypeControl.TypeChanged += (s, type) =>
            {
                //History
                TagTypeHistory history = new TagTypeHistory();
                this.ViewModel.Push(history);

                //Selection
                this.SelectionViewModel.TagType = type;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    history.Add(layer, layer.TagType, type);

                    layer.TagType = type;
                });
            };
        }

    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayerMenu" />. 
    /// </summary>
    public sealed partial class LayerMenu : UserControl, IMenu
    {
        private void ConstructLayer()
        {
            //Follow
            this.FollowToggleControl.Tapped += (s, e) =>
            {
                bool value = (this.SelectionViewModel.IsFollowTransform) ? false : true;

                //Selection
                this.SelectionViewModel.IsFollowTransform = value;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.IsFollowTransform = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }


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
