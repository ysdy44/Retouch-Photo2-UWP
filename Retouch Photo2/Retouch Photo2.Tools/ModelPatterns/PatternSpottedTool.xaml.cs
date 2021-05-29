// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PatternSpottedTool.
    /// </summary>
    public partial class PatternSpottedTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;
        private int Converter(float value) => (int)value;


        //@Content
        public ToolType Type => ToolType.PatternSpotted;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        //@Construct
        /// <summary>
        /// Initializes a PatternSpottedTool. 
        /// </summary>
        public PatternSpottedTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.RadiusButton.Tapped += (s, e) => TouchbarExtension.Instance = this.RadiusButton;
            this.ConstructRadius1();
            this.ConstructRadius2();

            this.StepButton.Tapped += (s, e) => TouchbarExtension.Instance = this.StepButton;
            this.ConstructStep1();
            this.ConstructStep2();
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new PatternSpottedLayer
            {
                Step = this.SelectionViewModel.PatternSpotted_Step,
                Radius = this.SelectionViewModel.PatternSpotted_Radius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardCurveStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.CreateTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate(); // Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }
    }


    public partial class PatternSpottedTool : Page, ITool
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RadiusTextBlock.Text = resource.GetString("Tools_PatternSpotted_Radius");
            this.StepTextBlock.Text = resource.GetString("Tools_PatternSpotted_Step");
        }


        // Radius
        private void ConstructRadius1()
        {
            this.RadiusPicker.Minimum = 5;
            this.RadiusPicker.Maximum = 100;
            this.RadiusPicker.ValueChanged += (sender, value) =>
            {
                float radius = value;
                this.SelectionViewModel.PatternSpotted_Radius = radius;

                this.MethodViewModel.TLayerChanged<float, PatternSpottedLayer>
                (
                    layerType: LayerType.PatternSpotted,
                    set: (tLayer) => tLayer.Radius = radius,

                    type: HistoryType.LayersProperty_Set_PatternSpottedLayer_Radius,
                    getUndo: (tLayer) => tLayer.Radius,
                    setUndo: (tLayer, previous) => tLayer.Radius = previous
                );
            };
        }

        private void ConstructRadius2()
        {
            this.RadiusSlider.Minimum = 5.0d;
            this.RadiusSlider.Maximum = 100.0d;
            this.RadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternSpottedLayer>(layerType: LayerType.PatternSpotted, cache: (tLayer) => tLayer.CacheRadius());
            this.RadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternSpotted_Radius = horizontalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternSpottedLayer>(layerType: LayerType.PatternSpotted, set: (tLayer) => tLayer.Radius = horizontalStep);
            };
            this.RadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float radius = (float)value;
                this.SelectionViewModel.PatternSpotted_Radius = radius;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternSpottedLayer>
                (
                    layerType: LayerType.PatternSpotted,
                    set: (tLayer) => tLayer.Radius = radius,

                    type: HistoryType.LayersProperty_Set_PatternSpottedLayer_Radius,
                    getUndo: (tLayer) => tLayer.StartingRadius,
                    setUndo: (tLayer, previous) => tLayer.Radius = previous
                );
            };
        }


        // Step
        private void ConstructStep1()
        {
            this.StepPicker.Minimum = 5;
            this.StepPicker.Maximum = 100;
            this.StepPicker.ValueChanged += (sender, value) =>
            {
                float step = (float)value;
                this.SelectionViewModel.PatternSpotted_Step = step;

                this.MethodViewModel.TLayerChanged<float, PatternSpottedLayer>
                (
                    layerType: LayerType.PatternSpotted,
                    set: (tLayer) => tLayer.Step = step,

                    type: HistoryType.LayersProperty_Set_PatternSpottedLayer_Step,
                    getUndo: (tLayer) => tLayer.Step,
                    setUndo: (tLayer, previous) => tLayer.Step = previous
                );
            };
        }

        private void ConstructStep2()
        {
            this.StepSlider.Minimum = 5.0d;
            this.StepSlider.Maximum = 100.0d;
            this.StepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternSpottedLayer>(layerType: LayerType.PatternSpotted, cache: (tLayer) => tLayer.CacheStep());
            this.StepSlider.ValueChangeDelta += (sender, value) =>
            {
                float step = (float)value;
                this.SelectionViewModel.PatternSpotted_Step = step;

                this.MethodViewModel.TLayerChangeDelta<PatternSpottedLayer>(layerType: LayerType.PatternSpotted, set: (tLayer) => tLayer.Step = step);
            };
            this.StepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float step = (float)value;
                this.SelectionViewModel.PatternSpotted_Step = step;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternSpottedLayer>
                (
                    layerType: LayerType.PatternSpotted,
                    set: (tLayer) => tLayer.Step = step,

                    type: HistoryType.LayersProperty_Set_PatternSpottedLayer_Step,
                    getUndo: (tLayer) => tLayer.StartingStep,
                    setUndo: (tLayer, previous) => tLayer.Step = previous
                );
            };
        }


    }
}