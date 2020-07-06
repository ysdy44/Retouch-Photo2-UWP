using Retouch_Photo2.Adjustments.Icons;
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
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentGenericPage<HighlightsAndShadowsAdjustment>
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Generic
        /// <summary> Gets IAdjustment's adjustment. </summary>
        public HighlightsAndShadowsAdjustment Adjustment { get; set; }


        //@Construct
        /// <summary>
        /// Initializes a BrightnessPage. 
        /// </summary>
        public HighlightsAndShadowsPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructShadows();
            this.ConstructHighlights();
            this.ConstructClarity();
            this.ConstructMaskBlurAmount();
        }
    }

    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentGenericPage<HighlightsAndShadowsAdjustment>
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

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.ShadowsSlider.Value = 0;
            this.HighlightsSlider.Value = 0;
            this.ClaritySlider.Value = 0;
            this.MaskBlurAmountSlider.Value = 12.5;


            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
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
        /// <param name="adjustment"> The adjustment. </param>
        public void Follow(HighlightsAndShadowsAdjustment adjustment)
        {
            this.ShadowsSlider.Value = adjustment.Shadows * 100;
            this.HighlightsSlider.Value = adjustment.Highlights * 100;
            this.ClaritySlider.Value = adjustment.Clarity * 100;
            this.MaskBlurAmountSlider.Value = adjustment.MaskBlurAmount * 10;
        }
    }

    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentGenericPage<HighlightsAndShadowsAdjustment>
    {

        private void ConstructShadows()
        {
            this.ShadowsSlider.Value = 0;
            this.ShadowsSlider.Minimum = -100;
            this.ShadowsSlider.Maximum = 100;

            this.ShadowsSlider.SliderBrush = this.ShadowsBrush;

            this.ShadowsSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        adjustment.CacheShadows();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.ShadowsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float shadows = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Shadows = shadows;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.ShadowsSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float shadows = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set highlights and shadows adjustment shadows");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingShadows;
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
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Shadows = shadows;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        private void ConstructHighlights()
        {
            this.HighlightsSlider.Value = 0;
            this.HighlightsSlider.Minimum = -100;
            this.HighlightsSlider.Maximum = 100;

            this.HighlightsSlider.SliderBrush = this.HighlightsBrush;

            this.HighlightsSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        adjustment.CacheHighlights();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.HighlightsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float highlights = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Highlights = highlights;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.HighlightsSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float highlights = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set highlights and shadows adjustment highlights");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingHighlights;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is HighlightsAndShadowsAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Highlights = previous1;
                            }
                        };

                        this.ViewModel.HistoryPush(history);

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Highlights = highlights;

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        private void ConstructClarity()
        {
            this.ClaritySlider.Value = 0;
            this.ClaritySlider.Minimum = -100;
            this.ClaritySlider.Maximum = 100;

            this.ClaritySlider.SliderBrush = this.ClarityBrush;

            this.ClaritySlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        adjustment.CacheClarity();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.ClaritySlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float clarity = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Clarity = clarity;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.ClaritySlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float clarity = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set highlights and shadows adjustment clarity");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingClarity;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is HighlightsAndShadowsAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Clarity = previous1;
                            }
                        };

                        this.ViewModel.HistoryPush(history);

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Clarity = clarity;

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        private void ConstructMaskBlurAmount()
        {
            this.MaskBlurAmountSlider.Value = 12.5f;
            this.MaskBlurAmountSlider.Minimum = 0;
            this.MaskBlurAmountSlider.Maximum = 100;

            this.MaskBlurAmountSlider.SliderBrush = this.MaskBlurAmountBrush;

            this.MaskBlurAmountSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        adjustment.CacheMaskBlurAmount();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.MaskBlurAmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float maskBlurAmount = (float)value / 10.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.MaskBlurAmount = maskBlurAmount;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.MaskBlurAmountSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is HighlightsAndShadowsAdjustment adjustment)
                    {
                        float maskBlurAmount = (float)value / 10.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set highlights and shadows adjustment mask blur amount");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingMaskBlurAmount;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is HighlightsAndShadowsAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.MaskBlurAmount = previous1;
                            }
                        };

                        this.ViewModel.HistoryPush(history);

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.MaskBlurAmount = maskBlurAmount;

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}