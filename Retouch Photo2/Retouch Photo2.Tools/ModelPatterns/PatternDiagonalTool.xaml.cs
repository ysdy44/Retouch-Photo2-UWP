﻿// Core:              ★★★
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
    /// <see cref="ITool"/>'s PatternDiagonalTool.
    /// </summary>
    public partial class PatternDiagonalTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;        
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private int OffsetToNumberConverter(float value) => (int)value;
        private int StepToNumberConverter(float value) => (int)value;


        //@Content
        public ToolType Type => ToolType.PatternDiagonal;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }

        public bool IsOpen { get; set; }


        //@Construct
        /// <summary>
        /// Initializes a PatternDiagonalTool. 
        /// </summary>
        public PatternDiagonalTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.OffsetButton.Tapped += (s, e) => TouchbarExtension.Instance = this.OffsetButton;
            this.ConstructOffset1();
            this.ConstructOffset2();

            this.HorizontalStepButton.Tapped += (s, e) => TouchbarExtension.Instance = this.HorizontalStepButton;
            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new PatternDiagonalLayer
            {
                HorizontalStep = this.SelectionViewModel.PatternDiagonal_HorizontalStep,
                Offset = this.SelectionViewModel.PatternDiagonal_Offset,
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


    public partial class PatternDiagonalTool : Page, ITool
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.OffsetTextBlock.Text = resource.GetString("Tools_PatternDiagonal_Offset");
            this.HorizontalStepTextBlock.Text = resource.GetString("Tools_PatternDiagonal_HorizontalStep");
        }

        // Offset
        private void ConstructOffset1()
        {
            this.OffsetPicker.Minimum = -100;
            this.OffsetPicker.Maximum = 100;
            this.OffsetPicker.ValueChanged += (sender, value) =>
            {
                float offset = value;
                this.SelectionViewModel.PatternDiagonal_Offset = offset;

                this.MethodViewModel.TLayerChanged<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.Offset = offset,

                    type: HistoryType.LayersProperty_Set_PatternDiagonalLayer_Offset,
                    getUndo: (tLayer) => tLayer.Offset,
                    setUndo: (tLayer, previous) => tLayer.Offset = previous
                );
            };
        }

        private void ConstructOffset2()
        {
            this.OffsetSlider.Minimum = -100.0d;
            this.OffsetSlider.Maximum = 100.0d;
            this.OffsetSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, cache: (tLayer) => tLayer.CacheOffset());
            this.OffsetSlider.ValueChangeDelta += (sender, value) =>
            {
                float offset = (float)value;
                this.SelectionViewModel.PatternDiagonal_Offset = offset;

                this.MethodViewModel.TLayerChangeDelta<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, set: (tLayer) => tLayer.Offset = offset);
            };
            this.OffsetSlider.ValueChangeCompleted += (sender, value) =>
            {
                float offset = (float)value;
                this.SelectionViewModel.PatternDiagonal_Offset = offset;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.Offset = offset,

                    type: HistoryType.LayersProperty_Set_PatternDiagonalLayer_Offset,
                    getUndo: (tLayer) => tLayer.StartingOffset,
                    setUndo: (tLayer, previous) => tLayer.Offset = previous
                );
            };
        }


        // HorizontalStep
        private void ConstructHorizontalStep1()
        {
            this.HorizontalStepPicker.Minimum = 5;
            this.HorizontalStepPicker.Maximum = 100;
            this.HorizontalStepPicker.ValueChanged += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternDiagonal_HorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChanged<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    type: HistoryType.LayersProperty_Set_PatternDiagonalLayer_HorizontalStep,
                    getUndo: (tLayer) => tLayer.HorizontalStep,
                    setUndo: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

        private void ConstructHorizontalStep2()
        {
            this.HorizontalStepSlider.Minimum = 5.0d;
            this.HorizontalStepSlider.Maximum = 100.0d;
            this.HorizontalStepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, cache: (tLayer) => tLayer.CacheHorizontalStep());
            this.HorizontalStepSlider.ValueChangeDelta += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternDiagonal_HorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, set: (tLayer) => tLayer.HorizontalStep = horizontalStep);
            };
            this.HorizontalStepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternDiagonal_HorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    type: HistoryType.LayersProperty_Set_PatternDiagonalLayer_HorizontalStep,
                    getUndo: (tLayer) => tLayer.StartingHorizontalStep,
                    setUndo: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

    }
}