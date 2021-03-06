﻿// Core:              ★
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
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Vignette;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title => this.TextBlock.Text;

        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        private float Amount
        {
            set
            {
                this.AmountPicker.Value = (int)(value * 100.0f);
                this.AmountSlider.Value = value;
            }
        }
        private float Curve
        {
            set
            {
                this.CurvePicker.Value = (int)(value * 100.0f);
                this.CurveSlider.Value = value;
            }
        }
        /// <summary> Color </summary>
        public Color Color
        {
            get => this.ColorEllipse.Color;
            set
            {
                this.ColorEllipse.Color = value;
                this.AmountRight.Color = value;
                this.CurveRight.Color = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a VignettePage. 
        /// </summary>
        public VignettePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructAmount1();
            this.ConstructAmount2();

            this.ConstructCurve1();
            this.ConstructCurve2();

            this.ConstructColor1();
            this.ConstructColor2();
        }
    }

    public sealed partial class VignettePage : IAdjustmentPage
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TextBlock.Text = resource.GetString("Adjustments_Vignette");

            this.AmountTextBlock.Text = resource.GetString("Adjustments_Vignette_Amount");
            this.CurveTextBlock.Text = resource.GetString("Adjustments_Vignette_Curve");
            this.ColorTextBlock.Text = resource.GetString("Adjustments_Vignette_Color");
        }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Amount = 0.0f;
            this.Curve = 0.0f;
            this.Color = Colors.Black;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is VignetteAdjustment adjustment)
                {
                    // History
                    LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_ResetAdjustment_Vignette);

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Amount;
                    var previous2 = adjustment.Curve;
                    var previous3 = adjustment.Color;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is VignetteAdjustment adjustment2)
                        {
                            // Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Amount = previous1;
                            adjustment2.Curve = previous2;
                            adjustment2.Color = previous3;
                        }
                    };

                    // Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Amount = 0.0f;
                    adjustment.Curve = 0.0f;
                    adjustment.Color = Colors.Black;

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

                if (layer.Filter.Adjustments[this.Index] is VignetteAdjustment adjustment)
                {
                    this.Amount = adjustment.Amount;
                    this.Curve = adjustment.Curve;
                    this.Color = adjustment.Color;
                }
            }
        }

    }

    public sealed partial class VignettePage : IAdjustmentPage
    {

        // Amount
        private void ConstructAmount1()
        {
            this.AmountPicker.Unit = "%";
            this.AmountPicker.Minimum = 0;
            this.AmountPicker.Maximum = 100;
            this.AmountPicker.ValueChanged += (s, value) =>
            {
                float amount = (float)value / 100.0f;
                this.Amount = amount;

                this.MethodViewModel.TAdjustmentChanged<float, VignetteAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Amount = amount,

                    type: HistoryType.LayersProperty_SetAdjustment_Vignette_Amount,
                    getUndo: (tAdjustment) => tAdjustment.Amount,
                    setUndo: (tAdjustment, previous) => tAdjustment.Amount = previous
                );
            };
        }

        private void ConstructAmount2()
        {
            this.AmountSlider.SliderBrush = this.AmountBrush;
            this.AmountSlider.Minimum = 0.0d;
            this.AmountSlider.Maximum = 1.0d;
            this.AmountSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<VignetteAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheAmount());
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value;
                this.Amount = amount;

                this.MethodViewModel.TAdjustmentChangeDelta<VignetteAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Amount = amount);
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float amount = (float)value;
                this.Amount = amount;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, VignetteAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Amount = amount,

                    type: HistoryType.LayersProperty_SetAdjustment_Vignette_Amount,
                    getUndo: (tAdjustment) => tAdjustment.StartingAmount,
                    setUndo: (tAdjustment, previous) => tAdjustment.Amount = previous
                );
            };
        }


        // Curve
        private void ConstructCurve1()
        {
            this.CurvePicker.Unit = "%";
            this.CurvePicker.Minimum = 0;
            this.CurvePicker.Maximum = 100;
            this.CurvePicker.ValueChanged += (s, value) =>
            {
                float curve = (float)value / 100.0f;
                this.Curve = curve;

                this.MethodViewModel.TAdjustmentChanged<float, VignetteAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Curve = curve,

                    type: HistoryType.LayersProperty_SetAdjustment_Vignette_Curve,
                    getUndo: (tAdjustment) => tAdjustment.Curve,
                    setUndo: (tAdjustment, previous) => tAdjustment.Curve = previous
                );
            };
        }

        private void ConstructCurve2()
        {
            this.CurveSlider.SliderBrush = this.CurveBrush;
            this.CurveSlider.Minimum = 0.0d;
            this.CurveSlider.Maximum = 1.0d;
            this.CurveSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<VignetteAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheCurve());
            this.CurveSlider.ValueChangeDelta += (s, value) =>
            {
                float curve = (float)value;
                this.Curve = curve;

                this.MethodViewModel.TAdjustmentChangeDelta<VignetteAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Curve = curve);
            };
            this.CurveSlider.ValueChangeCompleted += (s, value) =>
            {
                float curve = (float)value;
                this.Curve = curve;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, VignetteAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Curve = curve,

                    type: HistoryType.LayersProperty_SetAdjustment_Vignette_Curve,
                    getUndo: (tAdjustment) => tAdjustment.StartingCurve,
                    setUndo: (tAdjustment, previous) => tAdjustment.Curve = previous
                );
            };
        }


        // Color
        private void ConstructColor1()
        {
            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (layer.Filter.Adjustments[this.Index] is VignetteAdjustment adjustment)
                    {
                        this.ColorFlyout.ShowAt(this.ColorButton);
                        this.ColorPicker.Color = adjustment.Color;
                    }
                }
            };

            //@Focus
            this.ColorPicker.HexPicker.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.ColorPicker.Focus(FocusState.Programmatic); };
            this.ColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.ColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.ColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.ColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                Color color = value;
                this.Color = color;

                this.MethodViewModel.TAdjustmentChanged<Color, VignetteAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Color = color,

                    type: HistoryType.LayersProperty_SetAdjustment_Vignette_Color,
                    getUndo: (tAdjustment) => tAdjustment.Color,
                    setUndo: (tAdjustment, previous) => tAdjustment.Color = previous
                );
            };
        }

        private void ConstructColor2()
        {
            this.ColorPicker.ColorChangedStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<VignetteAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheColor());
            this.ColorPicker.ColorChangedDelta += (s, value) => this.MethodViewModel.TAdjustmentChangeDelta<VignetteAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Color = (Color)value);
            this.ColorPicker.ColorChangedCompleted += (s, value) =>
            {
                Color color = value;
                this.Color = color;

                this.MethodViewModel.TAdjustmentChangeCompleted<Color, VignetteAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Color = color,

                    type: HistoryType.LayersProperty_SetAdjustment_Vignette_Color,
                    getUndo: (tAdjustment) => tAdjustment.StartingColor,
                    setUndo: (tAdjustment, previous) => tAdjustment.Color = previous
                );
            };
        }

    }
}