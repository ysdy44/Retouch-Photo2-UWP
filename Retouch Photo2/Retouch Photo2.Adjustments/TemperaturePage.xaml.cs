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
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentGenericPage<TemperatureAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Generic
        /// <summary> Gets IAdjustment's adjustment. </summary>
        public TemperatureAdjustment Adjustment { get; set; }

        //@Construct
        /// <summary>
        /// Initializes a TemperaturePage. 
        /// </summary>
        public TemperaturePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructTemperature();
            this.ConstructTint();
        }
    }

    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentGenericPage<TemperatureAdjustment>
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Temperature");

            this.TemperatureTextBlock.Text = resource.GetString("/Adjustments/Temperature_Temperature");
            this.TintTextBlock.Text = resource.GetString("/Adjustments/Temperature_Tint");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Temperature;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new TemperatureAdjustment();

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.TemperatureSlider.Value = 0;
            this.TintSlider.Value = 0;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set temperature adjustment");

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Temperature;
                    var previous2 = adjustment.Tint;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is TemperatureAdjustment adjustment2)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Temperature = previous1;
                            adjustment2.Tint = previous2;
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Temperature = 0.0f;
                    adjustment.Tint = 0.0f;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        /// <param name="adjustment"> The adjustment. </param>
        public void Follow(TemperatureAdjustment adjustment)
        {
            this.TemperatureSlider.Value = adjustment.Temperature * 100;
            this.TintSlider.Value = adjustment.Tint * 100;
        }
    }

    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentGenericPage<TemperatureAdjustment>
    {

        private void ConstructTemperature()
        {
            this.TemperatureSlider.Value = 0;
            this.TemperatureSlider.Minimum = -100;
            this.TemperatureSlider.Maximum = 100;

            this.TemperatureSlider.SliderBrush = this.TemperatureBrush;

            this.TemperatureSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is TemperatureAdjustment adjustment)
                    {
                        adjustment.CacheTemperature();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.TemperatureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is TemperatureAdjustment adjustment)
                    {
                        float temperature = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Temperature = temperature;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.TemperatureSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is TemperatureAdjustment adjustment)
                    {
                        float temperature = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set temperature adjustment temperature");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingTemperature;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is TemperatureAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Temperature = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Temperature = temperature;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        private void ConstructTint()
        {
            this.TintSlider.Value = 0;
            this.TintSlider.Minimum = -100;
            this.TintSlider.Maximum = 100;

            this.TintSlider.SliderBrush = this.TintBrush;

            this.TintSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is TemperatureAdjustment adjustment)
                    {
                        adjustment.CacheTint();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.TintSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is TemperatureAdjustment adjustment)
                    {
                        float tint = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Tint = tint;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.TintSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is TemperatureAdjustment adjustment)
                    {
                        float tint = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set temperature adjustment tint");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingTint;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is TemperatureAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Tint = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Tint = tint;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}