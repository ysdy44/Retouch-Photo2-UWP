// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
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
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryPie;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title => this.GeometryPiePage.Title;
        public ControlTemplate Icon => this.GeometryPiePage.Icon;
        public FrameworkElement Page => this.GeometryPiePage;
        readonly GeometryPiePage GeometryPiePage = new GeometryPiePage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryPiePage.IsOpen; set => this.GeometryPiePage.IsOpen = value; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryPieLayer
            {
                SweepAngle = this.SelectionViewModel.GeometryPie_SweepAngle,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryPieTool"/>.
    /// </summary>
    internal partial class GeometryPiePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Converter
        private int SweepAngleToNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryPiePage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryPiePage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryPiePage), new PropertyMetadata(false));


        #endregion


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        //@Construct
        /// <summary>
        /// Initializes a GeometryPiePage. 
        /// </summary>
        public GeometryPiePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSweepAngle1();
            this.ConstructSweepAngle2();

            this.ConvertToCurvesButton.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.TipViewModel.ToolType = ToolType.Node;
            };

            this.MoreCreateButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreCreate?.Invoke(this, this.MoreCreateButton);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Tools_GeometryPie");

            this.SweepAngleTextBlock.Text = resource.GetString("Tools_GeometryPie_SweepAngle");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }
    }


    internal partial class GeometryPiePage : Page
    {
        //SweepAngle
        private void ConstructSweepAngle1()
        {
            this.SweepAnglePicker.Unit = "º";
            this.SweepAnglePicker.Minimum = 0;
            this.SweepAnglePicker.Maximum = 360;
            this.SweepAnglePicker.ValueChanged += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                this.SelectionViewModel.GeometryPie_SweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChanged<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    type: HistoryType.LayersProperty_Set_GeometryPieLayer_SweepAngle,
                    getUndo: (tLayer) => tLayer.SweepAngle,
                    setUndo: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructSweepAngle2()
        {
            this.SweepAngleSlider.Minimum = 0.0d;
            this.SweepAngleSlider.Maximum = FanKit.Math.PiTwice;
            this.SweepAngleSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryPieLayer>(layerType: LayerType.GeometryPie, cache: (tLayer) => tLayer.CacheSweepAngle());
            this.SweepAngleSlider.ValueChangeDelta += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryPie_SweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeDelta<GeometryPieLayer>(layerType: LayerType.GeometryPie, set: (tLayer) => tLayer.SweepAngle = sweepAngle);
            };
            this.SweepAngleSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryPie_SweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    type: HistoryType.LayersProperty_Set_GeometryPieLayer_SweepAngle,
                    getUndo: (tLayer) => tLayer.StartingSweepAngle,
                    setUndo: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}