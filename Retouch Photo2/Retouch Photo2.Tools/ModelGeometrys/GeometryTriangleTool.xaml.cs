// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryTriangleTool.
    /// </summary>
    public partial class GeometryTriangleTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryTriangle;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page => this.GeometryTrianglePage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryTrianglePage.IsOpen; set => this.GeometryTrianglePage.IsOpen = value; }
        readonly GeometryTrianglePage GeometryTrianglePage = new GeometryTrianglePage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryTriangleLayer
            {
                Center = this.SelectionViewModel.GeometryTriangle_Center,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryTriangleTool"/>.
    /// </summary>
    internal partial class GeometryTrianglePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        

        //@Converter
        private int CenterToNumberConverter(float center) => (int)(center * 100.0f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryTrianglePage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryTrianglePage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryTrianglePage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryTrianglePage. 
        /// </summary>
        public GeometryTrianglePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCenter1();
            this.ConstructCenter2();
            this.ConstructMirror();

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

            this.CenterTextBlock.Text = resource.GetString("Tools_GeometryTriangle_Center");
            this.MirrorTextBlock.Text = resource.GetString("Tools_GeometryTriangle_Mirror");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }
    }


    internal partial class GeometryTrianglePage : Page
    {

        //Center
        private void ConstructCenter1()
        {
            this.CenterPicker.Unit = "%";
            this.CenterPicker.Minimum = 0;
            this.CenterPicker.Maximum = 100;
            this.CenterPicker.ValueChanged += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    layerType: LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = center,

                    type: HistoryType.LayersProperty_Set_GeometryTriangleLayer_Center,
                    getUndo: (tLayer) => tLayer.Center,
                    setUndo: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructCenter2()
        {
            this.CenterSlider.Minimum = 0.0d;
            this.CenterSlider.Maximum = 1.0d;
            this.CenterSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TLayerChangeStarted<GeometryTriangleLayer>(layerType: LayerType.GeometryTriangle, cache: (tLayer) => tLayer.CacheCenter());
            this.CenterSlider.ValueChangeDelta += (s, value) =>
            {
                float center = (float)value;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChangeDelta<GeometryTriangleLayer>(layerType: LayerType.GeometryTriangle, set: (tLayer) => tLayer.Center = center);
            };
            this.CenterSlider.ValueChangeCompleted += (s, value) =>
            {
                float center = (float)value;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = center,

                    type: HistoryType.LayersProperty_Set_GeometryTriangleLayer_Center,
                    getUndo: (tLayer) => tLayer.StartingCenter,
                    setUndo: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                float center = 1.0f - this.SelectionViewModel.GeometryTriangle_Center;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = 1.0f - tLayer.Center,

                    type: HistoryType.LayersProperty_Set_GeometryTriangleLayer_Center,
                    getUndo: (tLayer) => tLayer.Center,
                    setUndo: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

    }
}