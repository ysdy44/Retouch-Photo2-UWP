﻿using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        private float Shadows
        {
            set
            {
                this.ShadowsPicker.Value = (int)(value * 100.0f);
                this.ShadowsSlider.Value = value;
            }
        }
        private float Highlights
        {
            set
            {
                this.HighlightsPicker.Value = (int)(value * 100.0f);
                this.HighlightsSlider.Value = value;
            }
        }
        private float Clarity
        {
            set
            {
                this.ClarityPicker.Value = (int)(value * 100.0f);
                this.ClaritySlider.Value = value;
            }
        }
        private float MaskBlurAmount
        {
            set
            {
                this.MaskBlurAmountPicker.Value = (int)(value * 100.0f);
                this.MaskBlurAmountSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a HighlightsAndShadowsPage. 
        /// </summary>
        public HighlightsAndShadowsPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            HighlightsAndShadowsAdjustment.GenericText = this.Text;
            HighlightsAndShadowsAdjustment.GenericPage = this;

            this.ConstructShadows1();
            this.ConstructShadows2();

            this.ConstructHighlights1();
            this.ConstructHighlights2();

            this.ConstructClarity1();
            this.ConstructClarity2();

            this.ConstructMaskBlurAmount1();
            this.ConstructMaskBlurAmount2();
        }
    }

    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentPage
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/HighlightsAndShadows");

            this.ShadowsTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Shadows");
            this.HighlightsTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Highlights");
            this.ClarityTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Clarity");
            this.MaskBlurAmountTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_MaskBlurAmount");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.HighlightsAndShadows;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new HighlightsAndShadowsIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }
        
        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new HighlightsAndShadowsAdjustment();
       
        
        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Shadows = 0.0f;
            this.Highlights = 0.0f; 
            this.Clarity = 0.0f;
            this.MaskBlurAmount = 1.25f;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is HighlightsAndShadowsAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set highlights and shadows adjustment");

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Shadows;
                    var previous2 = adjustment.Highlights;
                    var previous3 = adjustment.Clarity;
                    var previous4 = adjustment.MaskBlurAmount;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is HighlightsAndShadowsAdjustment adjustment2)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Shadows = previous1;
                            adjustment2.Highlights = previous2;
                            adjustment2.Clarity = previous3;
                            adjustment2.MaskBlurAmount = previous4;
                        }
                    };

                    this.ViewModel.HistoryPush(history);

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Shadows = 0.0f;
                    adjustment.Shadows = 0.0f;
                    adjustment.Clarity = 0.0f;
                    adjustment.MaskBlurAmount = 1.25f;

                    this.ViewModel.Invalidate();//Invalidate
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

                if (layer.Filter.Adjustments[this.Index] is HighlightsAndShadowsAdjustment adjustment)
                {
                    this.Shadows = adjustment.Shadows;
                    this.Highlights = adjustment.Highlights;
                    this.Clarity = adjustment.Clarity;
                    this.MaskBlurAmount = adjustment.MaskBlurAmount;
                }
            }
        }
    }

    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentPage
    {

        //Shadows
        private void ConstructShadows1()
        {
            this.ShadowsPicker.Unit = null;
            this.ShadowsPicker.Minimum = -100;
            this.ShadowsPicker.Maximum = 100;
            this.ShadowsPicker.ValueChanged += (s, value) =>
            {
                float shadows = (float)value / 100.0f;
                this.Shadows = shadows;

                this.MethodViewModel.TAdjustmentChanged<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Shadows = shadows,

                    historyTitle: "Set highlights and shadows adjustment shadows",
                    getHistory: (tAdjustment) => tAdjustment.Shadows,
                    setHistory: (tAdjustment, previous) => tAdjustment.Shadows = previous
                );
            };
        }

        private void ConstructShadows2()
        {
            this.ShadowsSlider.Minimum = -1.0d;
            this.ShadowsSlider.Maximum = 1.0d;
            this.ShadowsSlider.SliderBrush = this.ShadowsBrush;
            this.ShadowsSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<HighlightsAndShadowsAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheShadows());
            this.ShadowsSlider.ValueChangeDelta += (s, value) =>
            {
                float shadows = (float)value;
                this.Shadows = shadows;

                this.MethodViewModel.TAdjustmentChangeDelta<HighlightsAndShadowsAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Shadows = shadows);
            };
            this.ShadowsSlider.ValueChangeCompleted += (s, value) =>
            {
                float shadows = (float)value;
                this.Shadows = shadows;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Shadows = shadows,

                    historyTitle: "Set highlights and shadows adjustment shadows",
                    getHistory: (tAdjustment) => tAdjustment.StartingShadows,
                    setHistory: (tAdjustment, previous) => tAdjustment.Shadows = previous
                );
            };
        }


        //Highlights
        private void ConstructHighlights1()
        {
            this.HighlightsPicker.Unit = null;
            this.HighlightsPicker.Minimum = -100;
            this.HighlightsPicker.Maximum = 100;
            this.HighlightsPicker.ValueChanged += (s, value) =>
            {
                float highlights = (float)value / 100.0f;
                this.Highlights = highlights;

                this.MethodViewModel.TAdjustmentChanged<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Highlights = highlights,

                    historyTitle: "Set highlights and shadows adjustment highlights",
                    getHistory: (tAdjustment) => tAdjustment.Highlights,
                    setHistory: (tAdjustment, previous) => tAdjustment.Highlights = previous
                );
            };
        }

        private void ConstructHighlights2()
        {
            this.HighlightsSlider.Minimum = -1.0d;
            this.HighlightsSlider.Maximum = 1.0d;
            this.HighlightsSlider.SliderBrush = this.HighlightsBrush;
            this.HighlightsSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<HighlightsAndShadowsAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheHighlights());
            this.HighlightsSlider.ValueChangeDelta += (s, value) =>
            {
                float highlights = (float)value;
                this.Highlights = highlights;

                this.MethodViewModel.TAdjustmentChangeDelta<HighlightsAndShadowsAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Highlights = highlights);
            };
            this.HighlightsSlider.ValueChangeCompleted += (s, value) =>
            {
                float highlights = (float)value;
                this.Highlights = highlights;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Highlights = highlights,

                    historyTitle: "Set highlights and shadows adjustment highlights",
                    getHistory: (tAdjustment) => tAdjustment.StartingHighlights,
                    setHistory: (tAdjustment, previous) => tAdjustment.Highlights = previous
                );
            };
        }
        

        //Clarity
        private void ConstructClarity1()
        {
            this.ClarityPicker.Unit = null;
            this.ClarityPicker.Minimum = -100;
            this.ClarityPicker.Maximum = 100;
            this.ClarityPicker.ValueChanged += (s, value) =>
            {
                float clarity = (float)value / 100.0f;
                this.Clarity = clarity;

                this.MethodViewModel.TAdjustmentChanged<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Clarity = clarity,

                    historyTitle: "Set highlights and shadows adjustmentclarity",
                    getHistory: (tAdjustment) => tAdjustment.Clarity,
                    setHistory: (tAdjustment, previous) => tAdjustment.Clarity = previous
                );
            };
        }

        private void ConstructClarity2()
        {
            this.ClaritySlider.Minimum = -1.0d;
            this.ClaritySlider.Maximum = 1.0d;
            this.ClaritySlider.SliderBrush = this.ClarityBrush;
            this.ClaritySlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<HighlightsAndShadowsAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheClarity());
            this.ClaritySlider.ValueChangeDelta += (s, value) =>
            {
                float clarity = (float)value;
                this.Clarity = clarity;

                this.MethodViewModel.TAdjustmentChangeDelta<HighlightsAndShadowsAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Clarity = clarity);
            };
            this.ClaritySlider.ValueChangeCompleted += (s, value) =>
            {
                float clarity = (float)value;
                this.Clarity = clarity;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Clarity = clarity,

                    historyTitle: "Set highlights and shadows adjustmentclarity",
                    getHistory: (tAdjustment) => tAdjustment.StartingClarity,
                    setHistory: (tAdjustment, previous) => tAdjustment.Clarity = previous
                );
            };
        }


        //MaskBlurAmount
        private void ConstructMaskBlurAmount1()
        {
            this.MaskBlurAmountPicker.Unit = null;
            this.MaskBlurAmountPicker.Minimum = 0;
            this.MaskBlurAmountPicker.Maximum = 1000;
            this.MaskBlurAmountPicker.ValueChanged += (s, value) =>
            {
                float maskBlurAmount = (float)value / 100.0f;
                this.MaskBlurAmount = maskBlurAmount;

                this.MethodViewModel.TAdjustmentChanged<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.MaskBlurAmount = maskBlurAmount,

                    historyTitle: "Set highlights and shadows adjustment mask blur amount",
                    getHistory: (tAdjustment) => tAdjustment.MaskBlurAmount,
                    setHistory: (tAdjustment, previous) => tAdjustment.MaskBlurAmount = previous
                );
            };
        }

        private void ConstructMaskBlurAmount2()
        {
            this.MaskBlurAmountSlider.Minimum = 0.0d;
            this.MaskBlurAmountSlider.Maximum = 10.0d;
            this.MaskBlurAmountSlider.SliderBrush = this.MaskBlurAmountBrush;
            this.MaskBlurAmountSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<HighlightsAndShadowsAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheMaskBlurAmount());
            this.MaskBlurAmountSlider.ValueChangeDelta += (s, value) =>
            {
                float maskBlurAmount = (float)value;
                this.MaskBlurAmount = maskBlurAmount;

                this.MethodViewModel.TAdjustmentChangeDelta<HighlightsAndShadowsAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.MaskBlurAmount = maskBlurAmount);
            };
            this.MaskBlurAmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float maskBlurAmount = (float)value;
                this.MaskBlurAmount = maskBlurAmount;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, HighlightsAndShadowsAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.MaskBlurAmount = maskBlurAmount,

                    historyTitle: "Set highlights and shadows adjustment mask blur amount",
                    getHistory: (tAdjustment) => tAdjustment.StartingMaskBlurAmount,
                    setHistory: (tAdjustment, previous) => tAdjustment.MaskBlurAmount = previous
                );
            };
        }

    }
}