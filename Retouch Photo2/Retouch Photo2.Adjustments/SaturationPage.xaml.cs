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
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentGenericPage<SaturationAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Generic
        public SaturationAdjustment Adjustment { get; set; }

        //@Construct
        public SaturationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSaturation();
        }
    }

    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentGenericPage<SaturationAdjustment>
    {
        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Saturation");

            this.SaturationTextBlock.Text = resource.GetString("/Adjustments/Saturation_Saturation");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new SaturationAdjustment();

        public void Reset()
        {
            this.SaturationSlider.Value = 100;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is SaturationAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set saturation adjustment");

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Saturation;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is SaturationAdjustment adjustment2)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Saturation = previous1;
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Saturation = 1.0f;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }
        public void Follow(SaturationAdjustment adjustment)
        {
            this.SaturationSlider.Value = adjustment.Saturation * 100.0f;
        }
    }

    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentGenericPage<SaturationAdjustment>
    {

        public void ConstructSaturation()
        {
            this.SaturationSlider.Value = 100;
            this.SaturationSlider.Minimum = 0;
            this.SaturationSlider.Maximum = 200;

            this.SaturationSlider.SliderBrush = this.SaturationBrush;

            this.SaturationSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is SaturationAdjustment adjustment)
                    {
                        adjustment.CacheSaturation();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.SaturationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is SaturationAdjustment adjustment)
                    {
                        float saturation = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Saturation = saturation;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.SaturationSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is SaturationAdjustment adjustment)
                    {
                        float saturation = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set saturation adjustment saturation");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingSaturation;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is SaturationAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Saturation = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Saturation = saturation;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}