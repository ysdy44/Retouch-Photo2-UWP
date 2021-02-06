// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : Expander, IMenu
    {

        //@Content     
        public override UIElement MainPage => this.StrokeMainPage;

        readonly StrokeMainPage StrokeMainPage = new StrokeMainPage();


        //@Construct
        /// <summary>
        /// Initializes a StrokeMenu. 
        /// </summary>
        public StrokeMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("Menus_Stroke");

            this.Button.ToolTip.Closed += (s, e) => this.StrokeMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.StrokeMainPage.IsOpen = true;
            };
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Stroke;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Strokes.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }

    /// <summary>
    /// MainPage of <see cref = "StrokeMenu"/>.
    /// </summary>
    public sealed partial class StrokeMainPage : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int StrokeWidthToNumberConverter(float strokeWidth) => (int)(strokeWidth * 10.0f);
        private int StyleOffsetToNumberConverter(float styleOffset) => (int)(styleOffset * 10.0f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "StrokeMainPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "StrokeMainPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(StrokeMainPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a StrokeMainPage. 
        /// </summary>
        public StrokeMainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructDash();
            this.ConstructWidth1();
            this.ConstructWidth2();
            this.ConstructCap();
            this.ConstructJoin();
            this.ConstructOffset1();
            this.ConstructOffset2();

            this.ConstructIsFollowTransform();
            this.ConstructIsStrokeBehindFill();
            this.ConstructIsStrokeWidthFollowScale();
        }
    }

    /// <summary>
    /// MainPage of <see cref = "StrokeMenu"/>.
    /// </summary>
    public sealed partial class StrokeMainPage : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.DashTextBlock.Text = resource.GetString("Strokes_Dash");
            this.WidthTextBlock.Text = resource.GetString("Strokes_Width");
            this.CapTextBlock.Text = resource.GetString("Strokes_Cap");
            this.JoinTextBlock.Text = resource.GetString("Strokes_Join");
            this.OffsetTextBlock.Text = resource.GetString("Strokes_Offset");

            this.IsFollowTransformCheckBox.Content = resource.GetString("Strokes_IsFollowTransform");
            this.IsStrokeBehindFillCheckBox.Content = resource.GetString("Strokes_IsStrokeBehindFill");
            this.IsStrokeWidthFollowScaleCheckBox.Content = resource.GetString("Strokes_IsStrokeWidthFollowScale");
        }

    }

    /// <summary>
    /// MainPage of <see cref = "StrokeMenu"/>.
    /// </summary>
    public sealed partial class StrokeMainPage : UserControl
    {

        //Dash
        private void ConstructDash()
        {
            this.DashSegmented.DashChanged += (s, dash) =>
            {
                CanvasDashStyle strokeStyleDash = dash;
                this.SelectionViewModel.StrokeStyleDash = strokeStyleDash;

                this.MethodViewModel.ILayerChanged<CanvasDashStyle>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashStyle = strokeStyleDash;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set stroke style dash",
                    getHistory: (layer) => layer.Style.StrokeStyle.DashStyle,
                    setHistory: (layer, previous) => layer.Style.StrokeStyle.DashStyle = previous
                );
            };
        }


        //Width
        private void ConstructWidth1()
        {
            this.WidthPicker.Unit = null;
            this.WidthPicker.Minimum = 0;
            this.WidthPicker.Maximum = 1280;
            this.WidthPicker.ValueChanged += (s, value) =>
            {
                float strokeWidth = (float)value / 10.0f;
                this.SelectionViewModel.StrokeWidth = strokeWidth;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeWidth = strokeWidth;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set opacity",
                    getHistory: (layer) => layer.Style.StrokeWidth,
                    setHistory: (layer, previous) => layer.Style.StrokeWidth = previous
                );
            };
        }

        private void ConstructWidth2()
        {
            this.WidthSlider.Minimum = 0.0d;
            this.WidthSlider.Maximum = 128.0d;
            this.WidthSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.ILayerChangeStarted(cache: (layer) => layer.Style.CacheStrokeWidth());
            this.WidthSlider.ValueChangeDelta += (s, value) =>
            {
                float strokeWidth = (float)value;
                this.SelectionViewModel.StrokeWidth = strokeWidth;

                this.MethodViewModel.ILayerChangeDelta(set: (layer) => layer.Style.StrokeWidth = strokeWidth);
            };
            this.WidthSlider.ValueChangeCompleted += (s, value) =>
            {
                float strokeWidth = (float)value;
                this.SelectionViewModel.StrokeWidth = strokeWidth;

                this.MethodViewModel.ILayerChangeCompleted<float>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeWidth = strokeWidth;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set stroke width",
                    getHistory: (layer) => layer.Style.StartingStrokeWidth,
                    setHistory: (layer, previous) => layer.Style.StrokeWidth = previous
                );
            };
        }


        //Cap
        private void ConstructCap()
        {
            this.CapSegmented.CapChanged += (s, cap) =>
            {
                CanvasCapStyle strokeStyleCap = cap;
                this.SelectionViewModel.StrokeStyleCap = strokeStyleCap;

                this.MethodViewModel.ILayerChanged<CanvasCapStyle>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashCap = strokeStyleCap;
                        layer.Style.StrokeStyle.StartCap = strokeStyleCap;
                        layer.Style.StrokeStyle.EndCap = strokeStyleCap;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set stroke style cap",
                    getHistory: (layer) => layer.Style.StrokeStyle.DashCap,
                    setHistory: (layer, previous) => layer.Style.StrokeStyle.DashCap = previous
                );
            };
        }


        //Join
        private void ConstructJoin()
        {
            this.JoinSegmented.JoinChanged += (s, join) =>
            {
                CanvasLineJoin strokeStyleJoin = join;
                this.SelectionViewModel.StrokeStyleJoin = strokeStyleJoin;

                this.MethodViewModel.ILayerChanged<CanvasLineJoin>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.LineJoin = strokeStyleJoin;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set stroke style join",
                    getHistory: (layer) => layer.Style.StrokeStyle.LineJoin,
                    setHistory: (layer, previous) => layer.Style.StrokeStyle.LineJoin = previous
                );
            };
        }


        //Offset
        private void ConstructOffset1()
        {
            this.OffsetPicker.Unit = null;
            this.OffsetPicker.Minimum = 0;
            this.OffsetPicker.Maximum = 100;
            this.OffsetPicker.ValueChanged += (s, value) =>
            {
                float strokeOffset = (float)value / 10.0f;
                this.SelectionViewModel.StrokeStyleOffset = strokeOffset;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashOffset = strokeOffset;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set stroke style offset",
                    getHistory: (layer) => layer.Style.StrokeStyle.DashOffset,
                    setHistory: (layer, previous) => layer.Style.StrokeStyle.DashOffset = previous
                );
            };
        }

        private void ConstructOffset2()
        {
            this.OffsetSlider.Minimum = 0.0d;
            this.OffsetSlider.Maximum = 10.0d;
            this.OffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.ILayerChangeStarted(cache: (layer) => layer.Style.CacheStrokeStyle());
            this.OffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float strokeOffset = (float)value;
                this.SelectionViewModel.StrokeStyleOffset = strokeOffset;

                this.MethodViewModel.ILayerChangeDelta(set: (layer) => layer.Style.StrokeStyle.DashOffset = strokeOffset);
            };
            this.OffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float strokeOffset = (float)value;
                this.SelectionViewModel.StrokeStyleOffset = strokeOffset;

                this.MethodViewModel.ILayerChangeCompleted<CanvasStrokeStyle>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashOffset = strokeOffset;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    historyTitle: "Set stroke width",
                    getHistory: (layer) => layer.Style.StartingStrokeStyle,
                    setHistory: (layer, previous) => layer.Style.StrokeStyle = previous
                );
            };
        }


        //IsFollowTransform
        private void ConstructIsFollowTransform()
        {
            this.IsFollowTransformCheckBox.Click += (s, e) =>
            {
                bool isFollowTransform = !this.SelectionViewModel.IsFollowTransform;
                this.SelectionViewModel.IsFollowTransform = isFollowTransform;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    set: (layer) => layer.Style.IsFollowTransform = isFollowTransform,

                    historyTitle: "Set style is follow transform",
                    getHistory: (layer) => layer.Style.IsFollowTransform,
                    setHistory: (layer, previous) => layer.Style.IsFollowTransform = previous
                );
            };
        }


        //IsStrokeBehindFill
        private void ConstructIsStrokeBehindFill()
        {
            this.IsStrokeBehindFillCheckBox.Click += (s, e) =>
            {
                bool IsStrokeBehindFill = !this.SelectionViewModel.IsStrokeBehindFill;
                this.SelectionViewModel.IsStrokeBehindFill = IsStrokeBehindFill;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    set: (layer) => layer.Style.IsStrokeBehindFill = IsStrokeBehindFill,

                    historyTitle: "Set style is stroke behind fill",
                    getHistory: (layer) => layer.Style.IsStrokeBehindFill,
                    setHistory: (layer, previous) => layer.Style.IsStrokeBehindFill = previous
                );
            };
        }


        //IsStrokeWidthFollowScale
        private void ConstructIsStrokeWidthFollowScale()
        {
            this.IsStrokeWidthFollowScaleCheckBox.Click += (s, e) =>
            {
                bool IsStrokeWidthFollowScale = !this.SelectionViewModel.IsStrokeWidthFollowScale;
                this.SelectionViewModel.IsStrokeWidthFollowScale = IsStrokeWidthFollowScale;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    set: (layer) => layer.Style.IsStrokeWidthFollowScale = IsStrokeWidthFollowScale,

                    historyTitle: "Set style is follow transform",
                    getHistory: (layer) => layer.Style.IsStrokeWidthFollowScale,
                    setHistory: (layer, previous) => layer.Style.IsStrokeWidthFollowScale = previous
                );
            };
        }

    }
}