// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Windows.UI.Xaml.Controls;
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
    public sealed partial class TemperaturePage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Temperature;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title => this.TextBlock.Text;

        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        private float Temperature
        {
            set
            {
                this.TemperaturePicker.Value = (int)(value * 100.0f);
                this.TemperatureSlider.Value = value;
            }
        }
        private float Tint
        {
            set
            {
                this.TintPicker.Value = (int)(value * 100.0f);
                this.TintSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a TemperaturePage. 
        /// </summary>
        public TemperaturePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructTemperature1();
            this.ConstructTemperature2();

            this.ConstructTint1();
            this.ConstructTint2();
        }
    }

    public sealed partial class TemperaturePage : IAdjustmentPage
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TextBlock.Text = resource.GetString("Adjustments_Temperature");

            this.TemperatureTextBlock.Text = resource.GetString("Adjustments_Temperature_Temperature");
            this.TintTextBlock.Text = resource.GetString("Adjustments_Temperature_Tint");
        }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Temperature = 0.0f;
            this.Tint = 0.0f;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is TemperatureAdjustment adjustment)
                {
                    // History
                    LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_ResetAdjustment_Temperature);

                    var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                    var previous1 = adjustment.Temperature;
                    var previous2 = adjustment.Tint;
                    history.UndoAction += () =>
                    {
                        if (previous < 0) return;
                        if (previous > layer.Filter.Adjustments.Count - 1) return;
                        if (layer.Filter.Adjustments[previous] is TemperatureAdjustment adjustment2)
                        {
                            // Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment2.Temperature = previous1;
                            adjustment2.Tint = previous2;
                        }
                    };

                    // Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    adjustment.Temperature = 0.0f;
                    adjustment.Tint = 0.0f;

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

                if (layer.Filter.Adjustments[this.Index] is TemperatureAdjustment adjustment)
                {
                    this.Temperature = adjustment.Temperature;
                    this.Tint = adjustment.Tint;
                }
            }
        }

    }

    public sealed partial class TemperaturePage : IAdjustmentPage
    {

        // Temperature
        private void ConstructTemperature1()
        {
            this.TemperaturePicker.Unit = "%";
            this.TemperaturePicker.Minimum = -100;
            this.TemperaturePicker.Maximum = 100;
            this.TemperaturePicker.ValueChanged += (s, value) =>
            {
                float temperature = (float)value / 100.0f;
                this.Temperature = temperature;

                this.MethodViewModel.TAdjustmentChanged<float, TemperatureAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Temperature = temperature,

                    type: HistoryType.LayersProperty_SetAdjustment_Temperature_Temperature,
                    getUndo: (tAdjustment) => tAdjustment.Temperature,
                    setUndo: (tAdjustment, previous) => tAdjustment.Temperature = previous
                );
            };
        }

        private void ConstructTemperature2()
        {
            this.TemperatureSlider.SliderBrush = this.TemperatureBrush;
            this.TemperatureSlider.Minimum = -1.0d;
            this.TemperatureSlider.Maximum = 1.0d;
            this.TemperatureSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<TemperatureAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheTemperature());
            this.TemperatureSlider.ValueChangeDelta += (s, value) =>
            {
                float temperature = (float)value;
                this.Temperature = temperature;

                this.MethodViewModel.TAdjustmentChangeDelta<TemperatureAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Temperature = temperature);
            };
            this.TemperatureSlider.ValueChangeCompleted += (s, value) =>
            {
                float temperature = (float)value;
                this.Temperature = temperature;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, TemperatureAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Temperature = temperature,

                    type: HistoryType.LayersProperty_SetAdjustment_Temperature_Temperature,
                    getUndo: (tAdjustment) => tAdjustment.StartingTemperature,
                    setUndo: (tAdjustment, previous) => tAdjustment.Temperature = previous
                );
            };
        }


        // Tint
        private void ConstructTint1()
        {
            this.TintPicker.Unit = "%";
            this.TintPicker.Minimum = -100;
            this.TintPicker.Maximum = 100;
            this.TintPicker.ValueChanged += (s, value) =>
            {
                float tint = (float)value / 100.0f;
                this.Tint = tint;

                this.MethodViewModel.TAdjustmentChanged<float, TemperatureAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Temperature = tint,

                    type: HistoryType.LayersProperty_SetAdjustment_Temperature_Tint,
                    getUndo: (tAdjustment) => tAdjustment.Tint,
                    setUndo: (tAdjustment, previous) => tAdjustment.Tint = previous
                );
            };
        }

        private void ConstructTint2()
        {
            this.TintSlider.SliderBrush = this.TintBrush;
            this.TintSlider.Minimum = -1.0d;
            this.TintSlider.Maximum = 1.0d;
            this.TintSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<TemperatureAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheTint());
            this.TintSlider.ValueChangeDelta += (s, value) =>
            {
                float tint = (float)value;
                this.Tint = tint;

                this.MethodViewModel.TAdjustmentChangeDelta<TemperatureAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Tint = tint);
            };
            this.TintSlider.ValueChangeCompleted += (s, value) =>
            {
                float tint = (float)value;
                this.Tint = tint;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, TemperatureAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Tint = tint,

                    type: HistoryType.LayersProperty_SetAdjustment_Temperature_Tint,
                    getUndo: (tAdjustment) => tAdjustment.StartingTint,
                    setUndo: (tAdjustment, previous) => tAdjustment.Tint = previous
                );
            };
        }

    }
}