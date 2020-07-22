using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Layers.ILayer"/>.
    /// </summary>
    public sealed partial class LayerMenu : Expander, IMenu
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content     
        public override UIElement MainPage => this.LayerMainPage;
        LayerMainPage LayerMainPage = new LayerMainPage();


        //@Construct
        /// <summary>
        /// Initializes a LayerMenu. 
        /// </summary>
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.LayerMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Layers.ILayer"/>.
    /// </summary>
    public sealed partial class LayerMenu : Expander, IMenu
    {

        //DataContext
        private void ConstructDataContext(string path, DependencyProperty dp)
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

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Layer");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Layer;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Layers.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }


    /// <summary>
    /// MainPage of <see cref = "LayerMenu"/>.
    /// </summary>
    public sealed partial class LayerMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Delegate
        /// <summary> Occurs when second-page change. </summary>
        public event EventHandler<UIElement> SecondPageChanged;


        //@Content
        BlendModeComboBox BlendModeComboBox = new BlendModeComboBox
        {
            MinHeight = 165,
            MaxHeight = 300
        };


        //@Converter
        private bool VisibilityToBoolConverter(Visibility visibility) => visibility == Visibility.Visible;
        private int OpacityToNumberConverter(float opacity) => (int)(opacity * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a LayerMainPage. 
        /// </summary>
        public LayerMainPage()
        {
            this.InitializeComponent();
            this.ConstructBlendModeDataContext
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.BlendMode),
                 dp: BlendModeComboBox.ModeProperty
            );
            this.ConstructStrings();

            this.NameButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowRename?.Invoke();
            this.ConstructOpacity1();
            this.ConstructOpacity2();
            this.ConstructOpacity3();
            this.ConstructBlendMode();
            this.ConstructVisibility();
            this.ConstructTagType();
        }

    }

    /// <summary>
    /// MainPage of <see cref = "LayerMenu"/>.
    /// </summary>
    public sealed partial class LayerMainPage : UserControl
    {

        //DataContext
        private void ConstructBlendModeDataContext(object dataContext, string path, DependencyProperty dp)
        {
            this.BlendModeComboBox.DataContext = dataContext;

            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.BlendModeComboBox.SetBinding(dp, binding);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NameTextBlock.Text = resource.GetString("/Menus/Layer_Name");
            this.OpacityTextBlock.Text = resource.GetString("/Menus/Layer_Opacity");
            this.BlendModeTextBlock.Text = resource.GetString("/Menus/Layer_BlendMode");
            this.VisibilityTextBlock.Text = resource.GetString("/Menus/Layer_Visibility");
            this.TagTypeTextBlock.Text = resource.GetString("/Menus/Layer_TagType");
        }

    }

    /// <summary>
    /// MainPage of <see cref = "LayerMenu"/>.
    /// </summary>
    public sealed partial class LayerMainPage : UserControl
    {

        //Opacity
        private void ConstructOpacity1()
        {
            this.OpacityPicker.Unit = "%";
            this.OpacityPicker.Minimum = 0;
            this.OpacityPicker.Maximum = 100;
            this.OpacityPicker.ValueChanged += (s, value) =>
            {
                float opacity = (float)value / 100.0f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity",
                    getHistory: (layer) => layer.Opacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };
        }

        private void ConstructOpacity2()
        {
            this.OpacitySlider.Minimum = 0.0d;
            this.OpacitySlider.Maximum = 1.0d;
            this.OpacitySlider.ValueChangeStarted += (s, value) => this.MethodViewModel.ILayerChangeStarted(cache: (layer) => layer.CacheOpacity());
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float opacity = (float)value;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChangeDelta(set: (layer) => layer.Opacity = opacity);
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChangeCompleted<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity",
                    getHistory: (layer) => layer.StartingOpacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };
        }

        private void ConstructOpacity3()
        {
            this.Opacity0Button.Click += (s, e) =>
            {
                float opacity = 0.0f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity 0%",
                    getHistory: (layer) => layer.Opacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity25Button.Click += (s, e) =>
            {
                float opacity = 0.25f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity 25%",
                    getHistory: (layer) => layer.Opacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity50Button.Click += (s, e) =>
            {
                float opacity = 0.5f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity 50%",
                    getHistory: (layer) => layer.Opacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity75Button.Click += (s, e) =>
            {
                float opacity = 0.75f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity 75%",
                    getHistory: (layer) => layer.Opacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };

            this.Opacity100Button.Click += (s, e) =>
            {
                float opacity = 1.0f;
                this.SelectionViewModel.Opacity = opacity;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) => layer.Opacity = opacity,

                    historyTitle: "Set opacity 100%",
                    getHistory: (layer) => layer.Opacity,
                    setHistory: (layer, previous) => layer.Opacity = previous
                );
            };
        }


        //Blend Mode
        private void ConstructBlendMode()
        {
            this.BlendModeButton.Click += (s, e) =>
            {
                string title = this.BlendModeTextBlock.Text;
                UIElement secondPage = this.BlendModeComboBox;
                this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
            };

            this.BlendModeComboBox.ModeChanged += (s, mode) =>
            {
                BlendEffectMode? blendMode = mode;
                this.SelectionViewModel.BlendMode = blendMode;

                this.MethodViewModel.ILayerChanged<BlendEffectMode?>
                (
                    set: (layer) => layer.BlendMode = blendMode,

                    historyTitle: "Set blend mode",
                    getHistory: (layer) => layer.BlendMode,
                    setHistory: (layer, previous) => layer.BlendMode = previous
                );
            };
        }


        //Visibility
        private void ConstructVisibility()
        {
            this.VisibilityButton.Click += (s, e) =>
            {
                Visibility value = (this.SelectionViewModel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
                this.SelectionViewModel.Visibility = value;

                this.MethodViewModel.ILayerChanged<Visibility>
                (
                    set: (layer) => layer.Visibility = value,

                    historyTitle: "Set visibility",
                    getHistory: (layer) => layer.Visibility,
                    setHistory: (layer, previous) => layer.Visibility = previous
                );
            };
        }


        //Tag Type
        private void ConstructTagType()
        {
            this.TagTypeSegmented.TypeChanged += (s, type) =>
            {
                TagType tagType = (TagType)type;
                this.SelectionViewModel.TagType = tagType;

                this.MethodViewModel.ILayerChanged<Retouch_Photo2.Blends.TagType>
                (
                    set: (layer) => layer.TagType = tagType,

                    historyTitle: "Set tag type",
                    getHistory: (layer) => layer.TagType,
                    setHistory: (layer, previous) => layer.TagType = previous
                );
            };
        }

    }
}