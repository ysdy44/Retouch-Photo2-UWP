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
        public HighlightsAndShadowsAdjustment Adjustment { get; set; }

        //@Construct
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
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/HighlightsAndShadows");

            this.ShadowsTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Shadows");
            this.HighlightsTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Highlights");
            this.ClarityTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Clarity");
            this.MaskBlurAmountTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_MaskBlurAmount");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.HighlightsAndShadows;
        public FrameworkElement Icon { get; } = new HighlightsAndShadowsIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new HighlightsAndShadowsAdjustment();


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


                    var previous1 = adjustment.Shadows;
                    var previous2 = adjustment.Highlights;
                    var previous3 = adjustment.Clarity;
                    var previous4 = adjustment.MaskBlurAmount;
                    history.UndoActions.Push(() =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.Shadows = previous1;
                        adjustment.Highlights = previous2;
                        adjustment.Clarity = previous3;
                        adjustment.MaskBlurAmount = previous4;
                    });

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

        public void ConstructShadows()
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

                        var previous = adjustment.StartingShadows;
                        history.UndoActions.Push(() =>
                        {  
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.Shadows = previous;
                        });

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

        public void ConstructHighlights()
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

                        var previous = adjustment.StartingHighlights;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.Highlights = previous;
                        });

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

        public void ConstructClarity()
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

                        var previous = adjustment.StartingClarity;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.Clarity = previous;
                        });

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

        public void ConstructMaskBlurAmount()
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
                        
                        var previous = adjustment.StartingMaskBlurAmount;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.MaskBlurAmount = previous;
                        });

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