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
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentGenericPage<ExposureAdjustment>
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Generic
        /// <summary> Gets IAdjustment's adjustment. </summary>
        public ExposureAdjustment Adjustment { get; set; }

        //@Construct
        /// <summary>
        /// Initializes a BrightnessPage. 
        /// </summary>
        public ExposurePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructExposure();
        }
    }

    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentGenericPage<ExposureAdjustment>
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Exposure");

            this.ExposureTextBlock.Text = resource.GetString("/Adjustments/Exposure_Exposure");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Exposure;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new ExposureIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new ExposureAdjustment();

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.ExposureSlider.Value = 0;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is ExposureAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set contrast adjustment");

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Exposure;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is ExposureAdjustment adjustment2)
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Exposure = previous1;
                        }
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Exposure = 0.0f;

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
        public void Follow(ExposureAdjustment adjustment)
        {
            this.ExposureSlider.Value = adjustment.Exposure * 100;
        }
    }

    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentGenericPage<ExposureAdjustment>
    {

        private void ConstructExposure()
        {
            this.ExposureSlider.Value = 0;
            this.ExposureSlider.Minimum = -200;
            this.ExposureSlider.Maximum = 200;

            this.ExposureSlider.SliderBrush = this.ExposureBrush;

            this.ExposureSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is ExposureAdjustment adjustment)
                    {
                        adjustment.CacheExposure();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.ExposureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is ExposureAdjustment adjustment)
                    {
                        float exposure = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.Exposure = exposure;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.ExposureSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is ExposureAdjustment adjustment)
                    {
                        float exposure = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set exposure adjustment exposure");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingExposure;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is ExposureAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.Exposure = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.Exposure = exposure;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}