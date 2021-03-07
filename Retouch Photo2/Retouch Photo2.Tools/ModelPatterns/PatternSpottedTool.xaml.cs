// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
    public class PatternSpottedTool : ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.PatternSpotted;
        public ToolGroupType GroupType => ToolGroupType.Pattern;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page { get; } = new PatternSpottedPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        private ILayer CreateLayer(Transformer transformer)
        {
            return new PatternSpottedLayer
            {
                Step = this.SelectionViewModel.PatternSpotted_Step,
                Radius = this.SelectionViewModel.PatternSpotted_Radius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolManager.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolManager.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolManager.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolManager.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolManager.CreateTool.Draw(drawingSession);

        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }

    /// <summary>
    /// Page of <see cref="PatternSpottedTool"/>.
    /// </summary>
    internal sealed partial class PatternSpottedPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int Converter(float value) => (int)value;


        //@Construct
        /// <summary>
        /// Initializes a PatternSpottedPage. 
        /// </summary>
        public PatternSpottedPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructRadius1();
            this.ConstructRadius2();

            this.ConstructStep1();
            this.ConstructStep2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RadiusButton.Content = resource.GetString("Tools_PatternSpotted_Radius");
            this.StepButton.Content = resource.GetString("Tools_PatternSpotted_Step");
        }

    }

    /// <summary>
    /// Page of <see cref="PatternSpottedTool"/>.
    /// </summary>
    internal sealed partial class PatternSpottedPage : Page
    {

        //Radius
        private void ConstructRadius1()
        {
            this.RadiusPicker.Unit = null;
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


        //Step
        private void ConstructStep1()
        {
            this.StepPicker.Unit = null;
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