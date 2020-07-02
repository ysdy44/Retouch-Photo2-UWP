﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
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
    public sealed partial class PatternGridTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar
        private PatternGridMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case PatternGridMode.None:
                            this.HorizontalStepTouchbarButton.IsSelected = false;
                            this.VerticalStepTouchbarButton.IsSelected = false;
                            this.TipViewModel.TouchbarControl = null;
                        break;
                    case PatternGridMode.HorizontalStep:
                            this.HorizontalStepTouchbarButton.IsSelected = true; 
                            this.VerticalStepTouchbarButton.IsSelected = false;
                            this.TipViewModel.TouchbarControl = this.HorizontalStepTouchbarSlider;
                        break;
                    case PatternGridMode.VerticalStep:
                            this.HorizontalStepTouchbarButton.IsSelected = false;
                            this.VerticalStepTouchbarButton.IsSelected = true;
                            this.TipViewModel.TouchbarControl = this.VerticalStepTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int Converter(float value) => (int)value; 
        private Visibility HorizontalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Vertical ? Visibility.Collapsed : Visibility.Visible;
        private Visibility VerticalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Horizontal ? Visibility.Collapsed : Visibility.Visible;


        //@Construct
        /// <summary>
        /// Initializes a PatternGridTool. 
        /// </summary>
        public PatternGridTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructGridType();

            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();

            this.ConstructVerticalStep1();
            this.ConstructVerticalStep2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = PatternGridMode.None;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public partial class PatternGridTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/PatternGrid");
            this._button.Style = this.IconSelectedButtonStyle;

            this.TypeTextBlock.Text = resource.GetString("/ToolsSecond/PatternGrid_Type");
            this.HorizontalStepTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/PatternGrid_Horizontal");
            this.VerticalStepTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/PatternGrid_Vertical");
        }


        //@Content
        public ToolType Type => ToolType.PatternGrid;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new PatternGridIcon();
        readonly Button _button = new Button { Tag = new PatternGridIcon() };

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new PatternGridLayer(customDevice)
            {
                HorizontalStep = this.SelectionViewModel.PatternGridHorizontalStep,
                VerticalStep = this.SelectionViewModel.PatternGridVerticalStep,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public partial class PatternGridTool : Page, ITool
    {
        
        //GridType
        private void ConstructGridType()
        {
            this.PatternGridTypeComboBox.TypeChanged += (s, gridType) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set grid layer type");

                //Selection
                this.SelectionViewModel.PatternGridType = gridType;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer geometryArrowLayer = (PatternGridLayer)layer;

                        var previous = geometryArrowLayer.GridType;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            geometryArrowLayer.IsRefactoringRender = true;
                            geometryArrowLayer.IsRefactoringIconRender = true;
                            geometryArrowLayer.GridType = previous;
                        };

                        //Refactoring
                        geometryArrowLayer.IsRefactoringRender = true;
                        geometryArrowLayer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        geometryArrowLayer.GridType = gridType;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        
    }
}