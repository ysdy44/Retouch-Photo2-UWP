// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ColorMatchAdjustment"/>.
    /// </summary>
    public sealed partial class ColorMatchPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.SelectionViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.ColorMatch;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title => this.TextBlock.Text;

        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        private Color SourceColor {set => this.SourceColorEllipse.Color = value; }
        private Color DestinationColor {  set => this.DestinationColorEllipse.Color = value; }


        //@Construct
        /// <summary>
        /// Initializes a ColorMatchPage. 
        /// </summary>
        public ColorMatchPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSourceColor1();
            this.ConstructSourceColor2();

            this.ConstructDestinationColor1();
            this.ConstructDestinationColor2();
        }
    }

    public sealed partial class ColorMatchPage : IAdjustmentPage
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TextBlock.Text = resource.GetString("Adjustments_ColorMatch");

            this.SourceTextBlock.Text = resource.GetString("Adjustments_ColorMatch_Source");
            this.DestinationTextBlock.Text = resource.GetString("Adjustments_ColorMatch_Destination");
        }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.SourceColor = Colors.White;
            this.DestinationColor = Colors.Transparent;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is ColorMatchAdjustment adjustment)
                {
                    // History
                    LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_ResetAdjustment_ColorMatch);

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.SourceColor;
                    var previous2 = adjustment.DestinationColor;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is ColorMatchAdjustment adjustment2)
                        {
                            // Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.SourceColor = previous1;
                            adjustment2.DestinationColor = previous2;
                        }
                    };

                    // Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.SourceColor = Colors.White;
                    adjustment.DestinationColor = Colors.Transparent;

                    // History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate(); // Invalidate
                }
            }
        }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        public void Follow()
        {
            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is ColorMatchAdjustment adjustment)
                {
                    this.SourceColor = adjustment.SourceColor;
                    this.DestinationColor = adjustment.DestinationColor;
                }
            }
        }

    }

    public sealed partial class ColorMatchPage : IAdjustmentPage
    {


        // SourceColor
        private void ConstructSourceColor1()
        {
            this.SourceColorButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (layer.Filter.Adjustments[this.Index] is ColorMatchAdjustment adjustment)
                    {
                        this.SourceColorFlyout.ShowAt(this.SourceColorButton);
                        this.SourceColorPicker.Color = adjustment.SourceColor;
                    }
                }
            };

            //@Focus
            this.SourceColorPicker.HexPicker.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.SourceColorPicker.Focus(FocusState.Programmatic); };
            this.SourceColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.SourceColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.SourceColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.SourceColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.SourceColorPicker.ColorChanged += (s, value) =>
            {
                Color sourceColor = value;
                this.SourceColor = sourceColor;

                this.MethodViewModel.TAdjustmentChanged<Color, ColorMatchAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.SourceColor = sourceColor,

                    type: HistoryType.LayersProperty_SetAdjustment_ColorMatch_SourceColor,
                    getUndo: (tAdjustment) => tAdjustment.SourceColor,
                    setUndo: (tAdjustment, previous) => tAdjustment.SourceColor = previous
                );
            };
        }

        private void ConstructSourceColor2()
        {
            this.SourceColorPicker.ColorChangedStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<ColorMatchAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheSourceColor());
            this.SourceColorPicker.ColorChangedDelta += (s, value) =>
            {
                Color sourceColor = value;
                this.SourceColor = sourceColor;

                this.MethodViewModel.TAdjustmentChangeDelta<ColorMatchAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.SourceColor = sourceColor);
            };
            this.SourceColorPicker.ColorChangedCompleted += (s, value) =>
            {
                Color sourceColor = value;
                this.SourceColor = sourceColor;

                this.MethodViewModel.TAdjustmentChangeCompleted<Color, ColorMatchAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.SourceColor = sourceColor,

                    type: HistoryType.LayersProperty_SetAdjustment_ColorMatch_SourceColor,
                    getUndo: (tAdjustment) => tAdjustment.StartingSourceColor,
                    setUndo: (tAdjustment, previous) => tAdjustment.SourceColor = previous
                );
            };
        }


        // DestinationColor
        private void ConstructDestinationColor1()
        {
            this.DestinationColorButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (layer.Filter.Adjustments[this.Index] is ColorMatchAdjustment adjustment)
                    {
                        this.DestinationColorFlyout.ShowAt(this.DestinationColorButton);
                        this.DestinationColorPicker.Color = adjustment.DestinationColor;
                    }
                }
            };

            //@Focus
            this.DestinationColorPicker.HexPicker.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.DestinationColorPicker.Focus(FocusState.Programmatic); };
            this.DestinationColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.DestinationColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.DestinationColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.DestinationColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.DestinationColorPicker.ColorChanged += (s, value) =>
            {
                Color destinationColor = value;
                this.DestinationColor = destinationColor;

                this.MethodViewModel.TAdjustmentChanged<Color, ColorMatchAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.DestinationColor = destinationColor,

                    type: HistoryType.LayersProperty_SetAdjustment_ColorMatch_DestinationColor,
                    getUndo: (tAdjustment) => tAdjustment.DestinationColor,
                    setUndo: (tAdjustment, previous) => tAdjustment.DestinationColor = previous
                );
            };
        }

        private void ConstructDestinationColor2()
        {
            this.DestinationColorPicker.ColorChangedStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<ColorMatchAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheDestinationColor());
            this.DestinationColorPicker.ColorChangedDelta += (s, value) =>
            {
                Color destinationColor = value;
                this.DestinationColor = destinationColor;

                this.MethodViewModel.TAdjustmentChangeDelta<ColorMatchAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.DestinationColor = destinationColor);
            };
            this.DestinationColorPicker.ColorChangedCompleted += (s, value) =>
            {
                Color destinationColor = value;
                this.DestinationColor = destinationColor;

                this.MethodViewModel.TAdjustmentChangeCompleted<Color, ColorMatchAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.DestinationColor = destinationColor,

                    type: HistoryType.LayersProperty_SetAdjustment_ColorMatch_DestinationColor,
                    getUndo: (tAdjustment) => tAdjustment.StartingDestinationColor,
                    setUndo: (tAdjustment, previous) => tAdjustment.DestinationColor = previous
                );
            };
        }

    }
}