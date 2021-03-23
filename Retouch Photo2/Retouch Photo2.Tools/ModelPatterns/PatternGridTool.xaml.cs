﻿// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    internal enum PatternGridMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Horizontal Step. </summary>
        HorizontalStep,

        /// <summary> Vertical Step. </summary>
        VerticalStep
    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public partial class PatternGridTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.PatternGrid;
        public ToolGroupType GroupType => ToolGroupType.Pattern; 
        public string Title => this.PatternGridPage.Title;
        public ControlTemplate Icon => this.PatternGridPage.Icon;
        public FrameworkElement Page => this.PatternGridPage;
        readonly PatternGridPage PatternGridPage = new PatternGridPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }
                

        public override ILayer CreateLayer(Transformer transformer)
        {
            return new PatternGridLayer
            {
                HorizontalStep = this.SelectionViewModel.PatternGrid_HorizontalStep,
                VerticalStep = this.SelectionViewModel.PatternGrid_VerticalStep,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="PatternGridTool"/>.
    /// </summary>
    internal partial class PatternGridPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int StepToNumberConverter(float value) => (int)value;
        private Visibility HorizontalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Vertical ? Visibility.Collapsed : Visibility.Visible;
        private Visibility VerticalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Horizontal ? Visibility.Collapsed : Visibility.Visible;


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        //@Construct
        /// <summary>
        /// Initializes a PatternGridPage. 
        /// </summary>
        public PatternGridPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructGridType();

            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();

            this.ConstructVerticalStep1();
            this.ConstructVerticalStep2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Tools_PatternGrid");

            this.TypeTextBlock.Text = resource.GetString("Tools_PatternGrid_Type");
            this.HorizontalStepTextBlock.Text = resource.GetString("Tools_PatternGrid_HorizontalStep");
            this.VerticalStepTextBlock.Text = resource.GetString("Tools_PatternGrid_VerticalStep");
        }
    }

    /// <summary>
    /// Page of <see cref="PatternGridTool"/>.
    /// </summary>
    internal partial class PatternGridPage : Page
    {

        //GridType
        private void ConstructGridType()
        {
            this.TypeComboBox.TypeChanged += (s, type) =>
            {
                PatternGridType gridType = (PatternGridType)type;
                this.SelectionViewModel.PatternGrid_Type = gridType;

                this.MethodViewModel.TLayerChanged<PatternGridType, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.GridType = gridType,

                    type: HistoryType.LayersProperty_Set_PatternGridLayer_GridType,
                    getUndo: (tLayer) => tLayer.GridType,
                    setUndo: (tLayer, previous) => tLayer.GridType = previous
                );
            };
        }


        //HorizontalStep
        private void ConstructHorizontalStep1()
        {
            this.HorizontalStepPicker.Unit = null;
            this.HorizontalStepPicker.Minimum = 5;
            this.HorizontalStepPicker.Maximum = 100;
            this.HorizontalStepPicker.ValueChanged += (sender, value) =>
            {
                float horizontalStep = value;
                this.SelectionViewModel.PatternGrid_HorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChanged<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    type: HistoryType.LayersProperty_Set_PatternGridLayer_HorizontalStep,
                    getUndo: (tLayer) => tLayer.HorizontalStep,
                    setUndo: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

        private void ConstructHorizontalStep2()
        {
            this.HorizontalStepSlider.Minimum = 5.0d;
            this.HorizontalStepSlider.Maximum = 100.0d;
            this.HorizontalStepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>(layerType: LayerType.PatternGrid, cache: (tLayer) => tLayer.CacheHorizontalStep());
            this.HorizontalStepSlider.ValueChangeDelta += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternGrid_HorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>(layerType: LayerType.PatternGrid, set: (tLayer) => tLayer.HorizontalStep = horizontalStep);
            };
            this.HorizontalStepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternGrid_HorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    type: HistoryType.LayersProperty_Set_PatternGridLayer_HorizontalStep,
                    getUndo: (tLayer) => tLayer.StartingHorizontalStep,
                    setUndo: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }
        

        //VerticalStep
        private void ConstructVerticalStep1()
        {
            this.VerticalStepPicker.Unit = null;
            this.VerticalStepPicker.Minimum = 5;
            this.VerticalStepPicker.Maximum = 100;
            this.VerticalStepPicker.ValueChanged += (sender, value) =>
            {
                float verticalStep = (float)value;
                this.SelectionViewModel.PatternGrid_VerticalStep = verticalStep;

                this.MethodViewModel.TLayerChanged<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.VerticalStep = verticalStep,

                    type: HistoryType.LayersProperty_Set_PatternGridLayer_VerticalStep,
                    getUndo: (tLayer) => tLayer.VerticalStep,
                    setUndo: (tLayer, previous) => tLayer.VerticalStep = previous
                );
            };
        }

        private void ConstructVerticalStep2()
        {
            this.VerticalStepSlider.Minimum = 5.0d;
            this.VerticalStepSlider.Maximum = 100.0d;
            this.VerticalStepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>(layerType: LayerType.PatternGrid, cache: (tLayer) => tLayer.CacheVerticalStep());
            this.VerticalStepSlider.ValueChangeDelta += (sender, value) =>
            {
                float verticalStep = (float)value;
                this.SelectionViewModel.PatternGrid_VerticalStep = verticalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>(layerType: LayerType.PatternGrid, set: (tLayer) => tLayer.VerticalStep = verticalStep);
            };
            this.VerticalStepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float verticalStep = (float)value;
                this.SelectionViewModel.PatternGrid_VerticalStep = verticalStep;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.VerticalStep = verticalStep,

                    type: HistoryType.LayersProperty_Set_PatternGridLayer_VerticalStep,
                    getUndo: (tLayer) => tLayer.StartingVerticalStep,
                    setUndo: (tLayer, previous) => tLayer.VerticalStep = previous
                );
            };
        }

    }
}