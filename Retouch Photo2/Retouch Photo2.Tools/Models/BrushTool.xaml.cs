using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        FillOrStroke FillOrStroke { get => this.SelectionViewModel.FillOrStroke; set => this.SelectionViewModel.FillOrStroke = value; }
      

        //@Construct
        public BrushTool()
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

            //BrushType
            this.BrushTypeComboBox.FillTypeChanged += (s, brushType) => this.FillTypeChanged(brushType);
            this.BrushTypeComboBox.StrokeTypeChanged += (s, brushType) => this.StrokeTypeChanged(brushType);
        }

        
        //ShowControl
        private bool _isStopsFlyoutShowed;
        private void ConstructShowControl()
        {
            this.StopsFlyout.Opened += (s, e) => this._isStopsFlyoutShowed = true;
            this.StopsFlyout.Closed += (s, e) =>
            {
                this._isStopsFlyoutShowed = false;
                this.ShowControl.Invalidate();
            };
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


            this.ReplaceButton.Tapped += (s, e) =>
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        BrushTool.FillImage?.Invoke();
                        break;
                    case FillOrStroke.Stroke:
                        BrushTool.StrokeImage?.Invoke();
                        break;
                }
            };
        }


        public void OnNavigatedTo() => this.SelectionViewModel.SetModeStyleManager();
        public void OnNavigatedFrom() { }

    }

    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Brush");

            this.FillOrStrokeTextBlock.Text = resource.GetString("/Tools/Brush_FillOrStroke");
            this.BrushTypeTextBlock.Text = resource.GetString("/Tools/Brush_Type");
            this.BrushTextBlock.Text = resource.GetString("/Tools/Brush_Brush");
            
            this.ReplaceTextBlock.Text = resource.GetString("/Tools/Brush_Image_Replace");
            this.ExtendTextBlock.Text = resource.GetString("/Tools/Brush_Image_Extend");
        }


        //@Content
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new BrushIcon();
        readonly ToolButton _button = new ToolButton(new BrushIcon());


        BrushOperateMode _operateMode = BrushOperateMode.None;


        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillStarted(startingPoint, point);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeStarted(startingPoint, point);
                    break;
            }
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillDelta(startingPoint, point);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeDelta(startingPoint, point);
                    break;
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            this._operateMode = BrushOperateMode.None;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point)
        {
            //Select single layer
            this.TipViewModel.TransformerTool.SelectSingleLayer(point);
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            //Draw
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Color accentColor = this.ViewModel.AccentColor;

            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillBrush.Draw(drawingSession, matrix, accentColor);
                    break;
                case FillOrStroke.Stroke:
                    this.StrokeBrush.Draw(drawingSession, matrix, accentColor);
                    break;
            }
        }

    }
}