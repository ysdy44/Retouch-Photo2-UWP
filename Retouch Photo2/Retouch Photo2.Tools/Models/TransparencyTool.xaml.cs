// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TransparencyTool.
    /// </summary>
    public partial class TransparencyTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;
        public bool GradientToTrueConverter(BrushType type)
        {
            switch (type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    return true;
                default:
                    return false;
            }
        }


        //@Content 
        public ToolType Type => ToolType.Transparency;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        //@Construct
        /// <summary>
        /// Initializes a TransparencyTool. 
        /// </summary>
        public TransparencyTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructStopsPicker();

            //Type
            this.ConstructTransparency();
        }


        BrushHandleMode HandleMode = BrushHandleMode.None;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.Transformer);

            this.TransparencyStarted(startingPoint, point);

            CoreCursorExtension.Tool_ManipulationStarted();
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

            this.TransparencyDelta(canvasStartingPoint, canvasPoint);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            //Snap
            if (this.IsSnap)
            {
                canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }

            this.TransparencyComplete(canvasStartingPoint, canvasPoint);

            this.HandleMode = BrushHandleMode.None;
            CoreCursorExtension.None_ManipulationStarted();
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;
                    drawingSession.DrawLayerBound(layer2, matrix, this.ViewModel.AccentColor);
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }


            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                case ListViewSelectionMode.Multiple:
                    {
                        //Snapping
                        if (this.IsSnap) this.Snap.Draw(drawingSession, matrix);

                        this.Transparency.Draw(drawingSession, matrix, this.ViewModel.AccentColor);
                    }
                    break;
            }
        }


        public void OnNavigatedTo()
        {
            Layerage layerage = this.SelectionViewModel.GetFirstSelectedLayerage();
            if (layerage != null)
            {
                ILayer layer = layerage.Self;
                this.SelectionViewModel.SetStyle(layer.Style);
            }
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom() { }

    }


    public partial class TransparencyTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TypeTextBlock.Text = resource.GetString("Tools_Brush_Type");
        }

        private void ConstructStopsPicker()
        {
            //@Focus
            this.StopsPicker.ColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.StopsPicker.ColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.StopsPicker.ColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.StopsPicker.ColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.StopsPicker.StopsChanged += (s, array) => this.TransparencyStopsChanged(array);

            this.StopsPicker.StopsChangeStarted += (s, array) => this.TransparencyStopsChangeStarted(array);
            this.StopsPicker.StopsChangeDelta += (s, array) => this.TransparencyStopsChangeDelta(array);
            this.StopsPicker.StopsChangeCompleted += (s, array) => this.TransparencyStopsChangeCompleted(array);
        }

    }
}