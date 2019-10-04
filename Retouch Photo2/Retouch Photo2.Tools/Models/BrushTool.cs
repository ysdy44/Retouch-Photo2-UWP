using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public class BrushTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        BrushType BrushType => this.SelectionViewModel.BrushType;


        public bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._brushPage.IsSelected = value;
            }
        }
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon { get; } = new BrushIcon();
        public IToolButton Button { get; } = new BrushButton();
        public Page Page => this._brushPage;
        BrushPage _brushPage { get; } = new BrushPage();


        BrushPoints _startingBrushPoints;
        public BrushOperateMode OperateMode = BrushOperateMode.None;


        public void Starting(Vector2 point)
        {
        }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            if (this.BrushType == BrushType.None|| this.BrushType == BrushType.Color)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 startPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 endPoint = Vector2.Transform(point, inverseMatrix);

                BrushPoints brushPoints = new BrushPoints
                {
                    LinearGradientStartPoint = startPoint,
                    LinearGradientEndPoint = endPoint,
                };
                this._startingBrushPoints = brushPoints;
                this._brushPage.Gradient(GradientBrushType.Linear, brushPoints);

                this.OperateMode = BrushOperateMode.LinearEndPoint;
            }
            else
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                BrushType brushType = this.BrushType;

                BrushPoints brushPoints = this.SelectionViewModel.BrushPoints;
                this._startingBrushPoints = brushPoints;

                this.OperateMode = BrushOperateHelper.ContainsNodeMode(startingPoint, brushType, brushPoints, matrix);
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;
            if (this.OperateMode ==  BrushOperateMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            
            BrushPoints? brushPointsController = BrushOperateHelper.Controller(point, this._startingBrushPoints, this.OperateMode, inverseMatrix);

            if (brushPointsController is BrushPoints brushPoints)
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        //FillOrStroke
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                geometryLayer.FillBrush.Points = brushPoints;
                                break;
                            case FillOrStroke.Stroke:
                                geometryLayer.StrokeBrush.Points = brushPoints;
                                break;
                        }
                    }
                }, true);

                this.SelectionViewModel.BrushPoints = brushPoints;//Selection
                this.ViewModel.Invalidate();//Invalidate
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            this.OperateMode = BrushOperateMode.None;

            if (isSingleStarted == false)
            {
                //TransformerTool
                ITransformerTool transformerTool = this.TipViewModel.TransformerTool;
                transformerTool.SelectSingleLayer(startingPoint);
            }
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            BrushType brushType = this.BrushType;
            BrushPoints brushPoints = this.SelectionViewModel.BrushPoints;
            CanvasGradientStop[] brushArray = this.SelectionViewModel.BrushArray;
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Windows.UI.Color accentColor = this.ViewModel.AccentColor;

            BrushOperateHelper.Draw(drawingSession, brushType, brushPoints, brushArray, matrix, accentColor);
        }


        public void OnNavigatedTo()
        {
            FillOrStroke fillOrStroke = this.SelectionViewModel.FillOrStroke;
            this._brushPage.SetFillOrStroke(fillOrStroke);
        }
        public void OnNavigatedFrom() { }
    }
}