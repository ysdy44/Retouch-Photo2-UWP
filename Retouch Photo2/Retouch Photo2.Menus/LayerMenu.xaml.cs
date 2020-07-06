using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using Retouch_Photo2.ViewModels;
using System;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
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
        LayerMainPage LayerMainPage = new LayerMainPage();


        //@Construct
        /// <summary>
        /// Initializes a LayerMenu. 
        /// </summary>
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.MainPage = this.LayerMainPage;
            this.LayerMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.SecondPage != secondPage) this.SecondPage = secondPage;
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
        BlendModeComboBox BlendModeComboBox = new BlendModeComboBox();


        //@Converter
        private bool VisibilityToBoolConverter(Visibility visibility) => visibility == Visibility.Visible;
        private float OpacityToValueConverter(float opacity) => opacity * 100.0f;


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
            this.ConstructOpacity();
            this.ConstructBlendMode();
            this.ConstructVisibility();
            this.ConstructFollowTransform();
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

            this.FollowTransformTextBlock.Text = resource.GetString("/Menus/Layer_FollowTransform");

            this.TagTypeTextBlock.Text = resource.GetString("/Menus/Layer_TagType");
        }

    }

    /// <summary>
    /// MainPage of <see cref = "LayerMenu"/>.
    /// </summary>
    public sealed partial class LayerMainPage : UserControl
    {

        //Opacity
        private void ConstructOpacity()
        {
            this.OpacitySlider.Minimum = 0.0d;
            this.OpacitySlider.Maximum = 1.0d;
            this.OpacitySlider.ValueChangeStarted += (s, value) => this.MethodViewModel.ILayerChangeStarted(cache: (iLayer) => iLayer.CacheOpacity());
            this.OpacitySlider.ValueChangeDelta += (s, value) => this.MethodViewModel.ILayerChangeDelta((iLayer) => iLayer.Opacity = (float)value);
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value;

                this.MethodViewModel.ILayerChangeCompleted<float>
                (
                    setSelectionViewModel: () => this.SelectionViewModel.Opacity = opacity,
                    set: (iLayer) => iLayer.Opacity = opacity,

                    historyTitle: "Set opacity",
                    getHistory: (iLayer) => iLayer.StartingOpacity,
                    setHistory: (iLayer, previous) => iLayer.Opacity = previous
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

            this.BlendModeComboBox.ModeChanged += (s, mode) => this.MethodViewModel.ILayerChanged<BlendEffectMode?>
            (
                setSelectionViewModel: () => this.SelectionViewModel.BlendMode = mode,
                set: (iLayer) => iLayer.BlendMode = mode,

                historyTitle: "Set blend mode",
                getHistory: (iLayer) => iLayer.BlendMode,
                setHistory: (iLayer, previous) => iLayer.BlendMode = previous
            );
        }


        //Visibility
        private void ConstructVisibility()
        {
            this.VisibilityButton.Click += (s, e) =>
            {
                Visibility value = (this.SelectionViewModel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                this.MethodViewModel.ILayerChanged<Visibility>
                (
                    setSelectionViewModel: () => this.SelectionViewModel.Visibility = value,
                    set: (iLayer) => iLayer.Visibility = value,

                    historyTitle: "Set visibility",
                    getHistory: (iLayer) => iLayer.Visibility,
                    setHistory: (iLayer, previous) => iLayer.Visibility = previous
                );
            };
        }


        //FollowTransform
        private void ConstructFollowTransform()
        {
            this.FollowTransformToggleButton.Click += (s, e) =>
            {
                bool value = !this.SelectionViewModel.IsFollowTransform;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    setSelectionViewModel: () => this.SelectionViewModel.IsFollowTransform = value,
                    set: (iLayer) => iLayer.Style.IsFollowTransform = value,

                    historyTitle: "Set style follow transform",
                    getHistory: (iLayer) => iLayer.Style.IsFollowTransform,
                    setHistory: (iLayer, previous) => iLayer.Style.IsFollowTransform = previous
                );
            };
        }


        //Tag Type
        private void ConstructTagType()
        {
            this.TagTypeSegmented.TypeChanged += (s, type) => this.MethodViewModel.ILayerChanged<Retouch_Photo2.Blends.TagType>
            (
                setSelectionViewModel: () => this.SelectionViewModel.TagType = type,
                set: (iLayer) => iLayer.TagType = type,

                historyTitle: "Set tag type",
                getHistory: (iLayer) => iLayer.TagType,
                setHistory: (iLayer, previous) => iLayer.TagType = previous
            );
        }

    }
}