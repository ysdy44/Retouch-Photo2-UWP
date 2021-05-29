// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Layerage CurveLayerage => this.SelectionViewModel.CurveLayerage;
        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;


        //@Content 
        public ToolType Type => ToolType.Pen;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "PenTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "PenTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(PenTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a PenTool. 
        /// </summary>
        public PenTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            //Flyout
            this.FillBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowFillColorFlyout?.Invoke(this, this.FillBrushButton);
            this.StrokeBrushButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowStrokeColorFlyout?.Invoke(this, this.StrokeBrushButton);
            this.StrokeShowControl.Tapped += (s, e) => Expander.ShowAt(MenuType.Stroke, this.StrokeShowControl);

            this.ConvertToCurvesButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.ViewModel.ToolType = ToolType.Node;
            };

            this.MoreButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowMoreFlyout?.Invoke(this.MoreButton);
        }


        NodeCollectionMode Mode = NodeCollectionMode.None;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.CurveLayer == null)
                this.Mode = NodeCollectionMode.Preview;
            else
                this.Mode = NodeCollectionMode.Add;

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewStart(startingPoint);
                    break;
                case NodeCollectionMode.Add:
                    this.AddStart();
                    break;
            }
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewDelta(canvasPoint);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddDelta(canvasPoint);
                        }
                    }
                    break;
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewComplete(canvasStartingPoint, canvasPoint, isOutNodeDistance);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddComplete(canvasPoint);
                        }
                    }
                    break;
            }

            this.Mode = NodeCollectionMode.None;
        }
        public void Clicke(Vector2 point)
        {
            if (this.CurveLayer == null) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.Add:
                    this.AddComplete(canvasPoint);
                    break;
            }
        }

        public void Cursor(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            switch (this.Mode)
            {
                case NodeCollectionMode.Preview:
                    this.PreviewDraw(drawingSession);
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddDraw(drawingSession);
                        }
                    }
                    break;
                default:
                    {
                        if (this.CurveLayer != null)
                        {
                            ILayer layer = this.CurveLayer;

                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

                            drawingSession.DrawWireframe(layer, matrix, this.ViewModel.AccentColor);
                            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);
                        }
                    }
                    break;
            }
        }


        private void CreateLayer(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);


            //Transformer
            Transformer transformer = new Transformer(canvasPoint, canvasStartingPoint);

            //Layer
            Layerage curveLayerage = Layerage.CreateByGuid();
            CurveLayer curveLayer = new CurveLayer(canvasStartingPoint, canvasPoint)
            {
                Id = curveLayerage.Id,
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardCurveStyle,
            };
            LayerBase.Instances.Add(curveLayerage.Id, curveLayer);

            //Mezzanine
            LayerManager.Mezzanine(curveLayerage);


            this.SelectionViewModel.SetModeSingle(curveLayerage);//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillTextBlock.Text = resource.GetString("Tools_Fill");
            this.StrokeTextBlock.Text = resource.GetString("Tools_Stroke");

            this.StrokeShowToolTip.Content = resource.GetString("Menus_Stroke");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreToolTip.Content = resource.GetString("Tools_More");
        }
    }
}