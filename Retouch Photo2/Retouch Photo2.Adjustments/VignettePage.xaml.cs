using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentGenericPage<VignetteAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        public Color Color
        {
            get => this.SolidColorBrush.Color;
            set
            {
                this.SolidColorBrush.Color = value;
                this.AmountRight.Color = value;
                this.CurveRight.Color = value;
            }
        }

        //@Generic
        public VignetteAdjustment Adjustment { get; set; }

        //@Construct
        public VignettePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructAmount();
            this.ConstructCurve();

            this.ConstructColor1();
            this.ConstructColor2();

            //TODO
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                this.Color = value;

                if (this.Adjustment == null) return;

                this.Adjustment.Color = value;
                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }

    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentGenericPage<VignetteAdjustment>
    {
        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Vignette");

            this.AmountTextBlock.Text = resource.GetString("/Adjustments/Vignette_Amount");
            this.CurveTextBlock.Text = resource.GetString("/Adjustments/Vignette_Curve");
            this.ColorTextBlock.Text = resource.GetString("/Adjustments/Vignette_Color");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new VignetteAdjustment();


        public void Reset()
        {
            this.AmountSlider.Value = 0;
            this.CurveSlider.Value = 0;
            this.Color = Colors.Black;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment");

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
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Amount = previous1;
                            adjustment2.Curve = previous2;
                            adjustment2.Color = previous3;
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Amount = 0.0f;
                    adjustment.Curve = 0.0f;
                    adjustment.Color = Colors.Black;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }
        public void Follow(VignetteAdjustment adjustment)
        {
            this.AmountSlider.Value = adjustment.Amount * 100;
            this.CurveSlider.Value = adjustment.Curve * 100;
            this.Color = adjustment.Color;
        }
    }

    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentGenericPage<VignetteAdjustment>
    {

        public void ConstructAmount()
        {
            this.AmountSlider.Value = 0;
            this.AmountSlider.Minimum = 0;
            this.AmountSlider.Maximum = 100;

            this.AmountSlider.SliderBrush = this.AmountBrush;

            this.AmountSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        adjustment.CacheAmount();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        float amount = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Amount = amount;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment amount");

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        float amount = (float)value / 100.0f;

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingAmount;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is VignetteAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Amount = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Amount = amount;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        public void ConstructCurve()
        {
            this.CurveSlider.Value = 0;
            this.CurveSlider.Minimum = 0;
            this.CurveSlider.Maximum = 100;

            this.CurveSlider.SliderBrush = this.CurveBrush;

            this.CurveSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        adjustment.CacheCurve();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.CurveSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        float curve = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Curve = curve;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.CurveSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        float curve = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment curve");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingCurve;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is VignetteAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Curve = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Curve = curve;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructColor1()
        {
            this.ColorBorder.Tapped += (s, e) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    this.ColorPicker.Color = adjustment.Color;
                    this.ColorFlyout.ShowAt(this.ColorBorder);
                }
            };

            this.ColorPicker.ColorChanged += (s, value) =>
            {
                Color color = value;
                this.Color = color;

                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment color");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.Color;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is VignetteAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Color = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Color = color;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
        }

        public void ConstructColor2()
        {
            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        adjustment.CacheColor();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                Color color = value;

                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Color = color;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                Color color = value;
                this.Color = color;

                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is VignetteAdjustment adjustment)
                    {
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment color");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingColor;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is VignetteAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Color = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Color = color;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}