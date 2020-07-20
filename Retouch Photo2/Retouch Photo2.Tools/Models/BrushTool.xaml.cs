using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public partial class BrushTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        FillOrStroke FillOrStroke { get => this.SelectionViewModel.FillOrStroke; set => this.SelectionViewModel.FillOrStroke = value; }

        VectorBorderSnap Snap => this.ViewModel.VectorBorderSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Content
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon { get; } = new BrushIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new BrushIcon()
        };
        public FrameworkElement Page => this.BrushPage;
        BrushPage BrushPage = new BrushPage();


        //@Construct
        /// <summary>
        /// Initializes a BrushTool. 
        /// </summary>
        public BrushTool()
        {
            this.ConstructStrings();
        }


        BrushHandleMode HandleMode = BrushHandleMode.None;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            //Snap
            if (this.IsSnap) this.ViewModel.VectorBorderSnapInitiate(this.SelectionViewModel.Transformer);

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillStarted(startingPoint, point);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeStarted(startingPoint, point);
                    break;
            }

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

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillDelta(canvasStartingPoint, canvasPoint);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeDelta(canvasStartingPoint, canvasPoint);
                    break;
            }

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

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillComplete(canvasStartingPoint, canvasPoint);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeComplete(canvasStartingPoint, canvasPoint);
                    break;
            }

            this.HandleMode = BrushHandleMode.None;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

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

                        switch (this.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                this.Fill.Draw(drawingSession, matrix, this.ViewModel.AccentColor);
                                break;
                            case FillOrStroke.Stroke:
                                this.Stroke.Draw(drawingSession, matrix, this.ViewModel.AccentColor);
                                break;
                        }
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
        }
        public void OnNavigatedFrom() { }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Brush");
        }

    }


    /// <summary>
    /// Page of <see cref="BrushTool"/>.
    /// </summary>
    internal partial class BrushPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        FillOrStroke FillOrStroke { get => this.SelectionViewModel.FillOrStroke; set => this.SelectionViewModel.FillOrStroke = value; }

        
        //@Construct
        /// <summary>
        /// Initializes a BrushPage. 
        /// </summary>
        public BrushPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructShowControl();

            //FillOrStroke
            this.FillOrStrokeComboBox.FillOrStrokeChanged += (s, fillOrStroke) =>
            {
                this.FillOrStroke = fillOrStroke;
                this.ViewModel.Invalidate(); //Invalidate
            };

            //Type
            this.ConstructFillType();
            this.ConstructStrokeType();
        }

    }

    /// <summary>
    /// Page of <see cref="BrushTool"/>.
    /// </summary>
    internal partial class BrushPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillOrStrokeTextBlock.Text = resource.GetString("/Tools/Brush_FillOrStroke");
            this.TypeTextBlock.Text = resource.GetString("/Tools/Brush_Type");
            this.ShowTextBlock.Text = resource.GetString("/Tools/Brush_Brush");

            this.ExtendTextBlock.Text = resource.GetString("/Tools/Brush_Extend");
        }

        private void ConstructShowControl()
        {
            this.ShowControl.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillShow();
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeShow();
                        break;
                }
            };


            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.StopsPicker.ColorPicker.HexPicker is TextBox textBox)
            {
                textBox.IsEnabled = false;
                this.StopsPicker.ColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                this.StopsPicker.ColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = false;
                textBox.LostFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = true;
            }

            this.StopsPicker.StopsChanged += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChanged(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChanged(array);
                        break;
                }
            };

            this.StopsPicker.StopsChangeStarted += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeStarted(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeStarted(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeDelta += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeDelta(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeDelta(array);
                        break;
                }
            };
            this.StopsPicker.StopsChangeCompleted += (s, array) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillStopsChangeCompleted(array);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeStopsChangeCompleted(array);
                        break;
                }
            };


            this.ExtendComboBox.ExtendChanged += (s, extend) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.FillExtendChanged(extend);
                        break;
                    case FillOrStroke.Stroke:
                        this.StrokeExtendChanged(extend);
                        break;
                }
            };
        }

    }
}