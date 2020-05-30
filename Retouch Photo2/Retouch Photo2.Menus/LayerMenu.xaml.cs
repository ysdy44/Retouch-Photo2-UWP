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
using Retouch_Photo2.Blends;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayerMenu" />. 
    /// </summary>
    public sealed partial class LayerMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private bool VisibilityToBoolConverter(Visibility visibility) => visibility == Visibility.Visible;
        private float OpacityToValueConverter(float opacity) => opacity * 100.0f;


        //@Construct
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.NameButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowRename?.Invoke();
            this.ConstructOpacity();
            this.ConstructBlendMode();
            this.ConstructVisibility();
            this.ConstructFollowTransform();
            this.ConstructTagType();
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

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Layer");

            this.NameTextBlock.Text = resource.GetString("/Menus/Layer_Name");

            this.OpacityTextBlock.Text = resource.GetString("/Menus/Layer_Opacity");

            this.BlendModeTextBlock.Text = resource.GetString("/Menus/Layer_BlendMode");

            this.VisibilityTextBlock.Text = resource.GetString("/Menus/Layer_Visibility");

            this.FollowTransformTextBlock.Text = resource.GetString("/Menus/Layer_FollowTransform");

            this.TagTypeTextBlock.Text = resource.GetString("/Menus/Layer_TagType");
        }


        //Menu
        public MenuType Type => MenuType.Layer;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Layers.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
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
            this.OpacitySlider.Value = 100;
            this.OpacitySlider.Minimum = 0;
            this.OpacitySlider.Maximum = 100;
            this.OpacitySlider.ValueChangeStarted += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;
                    layer.CacheOpacity();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float opacity = (float)value / 100.0f;

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;
                    layer.Opacity = opacity;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value / 100.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set opacity");

                //Selection
                this.SelectionViewModel.Opacity = opacity;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.StartingOpacity;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Opacity = previous;
                    });

                    layer.Opacity = opacity;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };

        }
        

        //Blend Mode
        private void ConstructBlendMode()
        {
            this.BlendModeButton.Click += (s, e) =>
            {
                this.BlendModeComboBox.Mode = this.SelectionViewModel.BlendMode;

                this._Expander.IsSecondPage = true;
                this._Expander.CurrentTitle = this.BlendModeTextBlock.Text;
            };
            this.BlendModeComboBox.ModeChanged += (s, mode) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set blend mode");

                //Selection
                this.SelectionViewModel.BlendMode = mode;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.BlendMode;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.BlendMode = previous;
                    });

                    layer.BlendMode = mode;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Visibility
        private void ConstructVisibility()
        {
            this.VisibilityButton.Click += (s, e) =>
            {
                Visibility value = (this.SelectionViewModel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set visibility");

                //Selection
                this.SelectionViewModel.Visibility = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Visibility;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Visibility = previous;
                    });

                    layer.Visibility = value;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //FollowTransform
        private void ConstructFollowTransform()
        {
            this.FollowTransformToggleButton.Click += (s, e) =>
            {
                bool value = !this.SelectionViewModel.IsFollowTransform;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set style follow transform");

                //Selection
                this.SelectionViewModel.IsFollowTransform = value;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.IsFollowTransform;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Style.IsFollowTransform = previous;
                    });

                    layer.Style.IsFollowTransform = value;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Tag Type
        private void ConstructTagType()
        {
            this.TagTypeControl.TypeChanged += (s, type) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set tag type");

                //Selection
                this.SelectionViewModel.TagType = type;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.TagType;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.TagType = previous;
                    });

                    layer.TagType = type;
                });

                //History
                this.ViewModel.HistoryPush(history);
            };
        }

    }
}
