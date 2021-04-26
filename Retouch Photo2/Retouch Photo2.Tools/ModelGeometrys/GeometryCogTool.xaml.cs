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
    internal enum GeometryCogMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Count. </summary>
        Count,

        /// <summary> Inner-radius. </summary>
        InnerRadius,

        /// <summary> Tooth. </summary>
        Tooth,

        /// <summary> Notch. </summary>
        Notch
    }


    /// <summary>
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public partial class GeometryCogTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private string InnerRadiusToStringConverter(float innerRadius) => $"{this.InnerRadiusToNumberConverter(innerRadius)}%";
        private int InnerRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private string ToothToStringConverter(float tooth) => $"{this.ToothToNumberConverter(tooth)}%";
        private int ToothToNumberConverter(float tooth) => (int)(tooth * 100.0f);
        private string NotchToStringConverter(float notch) => $"{this.NotchToNumberConverter(notch)}%";
        private int NotchToNumberConverter(float notch) => (int)(notch * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryCog;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryCogTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryCogTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryCogTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryCogTool. 
        /// </summary>
        public GeometryCogTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.CountButton.Tapped += (s, e) => TouchbarExtension.Instance = this.CountButton;
            this.ConstructCount1();
            this.ConstructCount2();

            this.InnerRadiusButton.Tapped += (s, e) => TouchbarExtension.Instance = this.InnerRadiusButton;
            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();

            this.ToothButton.Tapped += (s, e) => TouchbarExtension.Instance = this.ToothButton;
            this.ConstructTooth1();
            this.ConstructTooth2();

            this.NotchButton.Tapped += (s, e) => TouchbarExtension.Instance = this.NotchButton;
            this.ConstructNotch1();
            this.ConstructNotch2();

            this.ConvertToCurvesButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.ViewModel.ToolType = ToolType.Node;
            };

            this.MoreButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowMoreFlyout?.Invoke(this.MoreButton);
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCogLayer
            {
                Count = this.SelectionViewModel.GeometryCog_Count,
                InnerRadius = this.SelectionViewModel.GeometryCog_InnerRadius,
                Tooth = this.SelectionViewModel.GeometryCog_Tooth,
                Notch = this.SelectionViewModel.GeometryCog_Notch,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.ClickeTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }
    }


    public partial class GeometryCogTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CountTextBlock.Text = resource.GetString("Tools_GeometryCog_Count");
            this.InnerRadiusTextBlock.Text = resource.GetString("Tools_GeometryCog_InnerRadius");
            this.ToothTextBlock.Text = resource.GetString("Tools_GeometryCog_Tooth");
            this.NotchTextBlock.Text = resource.GetString("Tools_GeometryCog_Notch");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_More");
        }


        //Count
        private void ConstructCount1()
        {
            this.CountPicker.Unit = null;
            this.CountPicker.Minimum = 4;
            this.CountPicker.Maximum = 36;
            this.CountPicker.ValueChanged += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCog_Count = count;

                this.MethodViewModel.TLayerChanged<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Count = count,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Count,
                    getUndo: (tLayer) => tLayer.Count,
                    setUndo: (tLayer, previous) => tLayer.Count = previous
                );
            };
        }

        private void ConstructCount2()
        {
            this.CountSlider.Minimum = 4;
            this.CountSlider.Maximum = 36;
            this.CountSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheCount());
            this.CountSlider.ValueChangeDelta += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCog_Count = count;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Count = count);
            };
            this.CountSlider.ValueChangeCompleted += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCog_Count = count;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Count = count,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Count,
                    getUndo: (tLayer) => tLayer.StartingCount,
                    setUndo: (tLayer, previous) => tLayer.Count = previous
                );
            };
        }


        //InnerRadius
        private void ConstructInnerRadius1()
        {
            this.InnerRadiusPicker.Unit = "%";
            this.InnerRadiusPicker.Minimum = 0;
            this.InnerRadiusPicker.Maximum = 100;
            this.InnerRadiusPicker.ValueChanged += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCog_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.InnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusSlider.Minimum = 0.0d;
            this.InnerRadiusSlider.Maximum = 1.0d;
            this.InnerRadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheInnerRadius());
            this.InnerRadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCog_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.InnerRadius = innerRadius);
            };
            this.InnerRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCog_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.StartingInnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }


        //Tooth
        private void ConstructTooth1()
        {
            this.ToothPicker.Unit = "%";
            this.ToothPicker.Minimum = 0;
            this.ToothPicker.Maximum = 50;
            this.ToothPicker.ValueChanged += (sender, value) =>
            {
                float tooth = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCog_Tooth = tooth;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Tooth,
                    getUndo: (tLayer) => tLayer.Tooth,
                    setUndo: (tLayer, previous) => tLayer.Tooth = previous
                );
            };
        }

        private void ConstructTooth2()
        {
            this.ToothSlider.Minimum = 0.0d;
            this.ToothSlider.Maximum = 0.5d;
            this.ToothSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheTooth());
            this.ToothSlider.ValueChangeDelta += (sender, value) =>
            {
                float tooth = (float)value;
                this.SelectionViewModel.GeometryCog_Tooth = tooth;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Tooth = tooth);
            };
            this.ToothSlider.ValueChangeCompleted += (sender, value) =>
            {
                float tooth = (float)value;
                this.SelectionViewModel.GeometryCog_Tooth = tooth;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Tooth,
                    getUndo: (tLayer) => tLayer.StartingTooth,
                    setUndo: (tLayer, previous) => tLayer.Tooth = previous
                );
            };
        }


        //Notch
        private void ConstructNotch1()
        {
            this.NotchPicker.Unit = "%";
            this.NotchPicker.Minimum = 0;
            this.NotchPicker.Maximum = 60;
            this.NotchPicker.ValueChanged += (sender, value) =>
            {
                float notch = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCog_Notch = notch;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Notch = notch,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Notch,
                    getUndo: (tLayer) => tLayer.Notch,
                    setUndo: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

        private void ConstructNotch2()
        {
            this.NotchSlider.Minimum = 0.0d;
            this.NotchSlider.Maximum = 0.6d;
            this.NotchSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheNotch());
            this.NotchSlider.ValueChangeDelta += (sender, value) =>
            {
                float notch = (float)value;
                this.SelectionViewModel.GeometryCog_Notch = notch;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Notch = notch);
            };
            this.NotchSlider.ValueChangeCompleted += (sender, value) =>
            {
                float notch = (float)value;
                this.SelectionViewModel.GeometryCog_Notch = notch;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Notch = notch,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Notch,
                    getUndo: (tLayer) => tLayer.StartingNotch,
                    setUndo: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

    }
}