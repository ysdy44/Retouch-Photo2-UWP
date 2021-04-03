// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int StrokeWidthToNumberConverter(float strokeWidth) => (int)(strokeWidth * 10.0f);
        private int StyleOffsetToNumberConverter(float styleOffset) => (int)(styleOffset * 10.0f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "StrokeMenu" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "StrokeMenu.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(StrokeMenu), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a StrokeMenu. 
        /// </summary>
        public StrokeMenu()
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

    public sealed partial class StrokeMenu : UserControl
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


        //Dash
        private void ConstructDash()
        {
            this.DashSegmented.DashChanged += (s, dash) =>
            {
                CanvasDashStyle strokeStyleDash = dash;
                this.SelectionViewModel.StrokeStyle_Dash = strokeStyleDash;

                this.MethodViewModel.ILayerChanged<CanvasDashStyle>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashStyle = strokeStyleDash;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    type: HistoryType.LayersProperty_SetStyle_StrokeStyle_Dash,
                    getUndo: (layer) => layer.Style.StrokeStyle.DashStyle,
                    setUndo: (layer, previous) => layer.Style.StrokeStyle.DashStyle = previous
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

                    type: HistoryType.LayersProperty_SetStyle_StrokeWidth,
                    getUndo: (layer) => layer.Style.StrokeWidth,
                    setUndo: (layer, previous) => layer.Style.StrokeWidth = previous
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

                    type: HistoryType.LayersProperty_SetStyle_StrokeWidth,
                    getUndo: (layer) => layer.Style.StartingStrokeWidth,
                    setUndo: (layer, previous) => layer.Style.StrokeWidth = previous
                );
            };
        }


        //Cap
        private void ConstructCap()
        {
            this.CapSegmented.CapChanged += (s, cap) =>
            {
                CanvasCapStyle strokeStyleCap = cap;
                this.SelectionViewModel.StrokeStyle_Cap = strokeStyleCap;

                this.MethodViewModel.ILayerChanged<CanvasCapStyle>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashCap = strokeStyleCap;
                        layer.Style.StrokeStyle.StartCap = strokeStyleCap;
                        layer.Style.StrokeStyle.EndCap = strokeStyleCap;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    type: HistoryType.LayersProperty_SetStyle_StrokeStyle_Cap,
                    getUndo: (layer) => layer.Style.StrokeStyle.DashCap,
                    setUndo: (layer, previous) => layer.Style.StrokeStyle.DashCap = previous
                );
            };
        }


        //Join
        private void ConstructJoin()
        {
            this.JoinSegmented.JoinChanged += (s, join) =>
            {
                CanvasLineJoin strokeStyleJoin = join;
                this.SelectionViewModel.StrokeStyle_Join = strokeStyleJoin;

                this.MethodViewModel.ILayerChanged<CanvasLineJoin>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.LineJoin = strokeStyleJoin;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    type: HistoryType.LayersProperty_SetStyle_StrokeStyle_Join,
                    getUndo: (layer) => layer.Style.StrokeStyle.LineJoin,
                    setUndo: (layer, previous) => layer.Style.StrokeStyle.LineJoin = previous
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
                this.SelectionViewModel.StrokeStyle_Offset = strokeOffset;

                this.MethodViewModel.ILayerChanged<float>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashOffset = strokeOffset;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    type: HistoryType.LayersProperty_SetStyle_StrokeStyle_Offset,
                    getUndo: (layer) => layer.Style.StrokeStyle.DashOffset,
                    setUndo: (layer, previous) => layer.Style.StrokeStyle.DashOffset = previous
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
                this.SelectionViewModel.StrokeStyle_Offset = strokeOffset;

                this.MethodViewModel.ILayerChangeDelta(set: (layer) => layer.Style.StrokeStyle.DashOffset = strokeOffset);
            };
            this.OffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float strokeOffset = (float)value;
                this.SelectionViewModel.StrokeStyle_Offset = strokeOffset;

                this.MethodViewModel.ILayerChangeCompleted<float>
                (
                    set: (layer) =>
                    {
                        layer.Style.StrokeStyle.DashOffset = strokeOffset;
                        this.SelectionViewModel.StandStyleLayer = layer;
                    },

                    type: HistoryType.LayersProperty_SetStyle_StrokeStyle_Offset,
                    getUndo: (layer) => layer.Style.StartingOffset,
                    setUndo: (layer, previous) => layer.Style.StrokeStyle.DashOffset = previous
                );
            };
        }


        //IsFollowTransform
        private void ConstructIsFollowTransform()
        {
            this.IsFollowTransformCheckBox.Tapped += (s, e) =>
            {
                bool isFollowTransform = !this.SelectionViewModel.IsFollowTransform;
                this.SelectionViewModel.IsFollowTransform = isFollowTransform;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    set: (layer) => layer.Style.IsFollowTransform = isFollowTransform,

                    type: HistoryType.LayersProperty_SetStyle_IsFollowTransform,
                    getUndo: (layer) => layer.Style.IsFollowTransform,
                    setUndo: (layer, previous) => layer.Style.IsFollowTransform = previous
                );
            };
        }


        //IsStrokeBehindFill
        private void ConstructIsStrokeBehindFill()
        {
            this.IsStrokeBehindFillCheckBox.Tapped += (s, e) =>
            {
                bool IsStrokeBehindFill = !this.SelectionViewModel.IsStrokeBehindFill;
                this.SelectionViewModel.IsStrokeBehindFill = IsStrokeBehindFill;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    set: (layer) => layer.Style.IsStrokeBehindFill = IsStrokeBehindFill,

                    type: HistoryType.LayersProperty_SetStyle_IsStrokeBehindFill,
                    getUndo: (layer) => layer.Style.IsStrokeBehindFill,
                    setUndo: (layer, previous) => layer.Style.IsStrokeBehindFill = previous
                );
            };
        }


        //IsStrokeWidthFollowScale
        private void ConstructIsStrokeWidthFollowScale()
        {
            this.IsStrokeWidthFollowScaleCheckBox.Tapped += (s, e) =>
            {
                bool IsStrokeWidthFollowScale = !this.SelectionViewModel.IsStrokeWidthFollowScale;
                this.SelectionViewModel.IsStrokeWidthFollowScale = IsStrokeWidthFollowScale;

                this.MethodViewModel.ILayerChanged<bool>
                (
                    set: (layer) => layer.Style.IsStrokeWidthFollowScale = IsStrokeWidthFollowScale,

                    type: HistoryType.LayersProperty_SetStyle_IsStrokeWidthFollowScale,
                    getUndo: (layer) => layer.Style.IsStrokeWidthFollowScale,
                    setUndo: (layer, previous) => layer.Style.IsStrokeWidthFollowScale = previous
                );
            };
        }

    }
}