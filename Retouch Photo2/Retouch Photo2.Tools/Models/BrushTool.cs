using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Retouch_Photo2.Tools.Models.BrushTools;
using Retouch_Photo2.Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
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

        ListViewSelectionMode Mode => this.SelectionViewModel.Mode;
        BrushType BrushType => this.SelectionViewModel.BrushType;

        //Brush
        public readonly LinearGradientTool LinearGradientTool = new LinearGradientTool();
        public readonly RadialGradientTool RadialGradientTool = new RadialGradientTool();
        public readonly EllipticalGradientTool EllipticalGradientTool = new EllipticalGradientTool();

        public bool IsOpen { set { } }
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon { get; } = new BrushControl();
        public FrameworkElement ShowIcon { get; } = new BrushControl();
        public Page Page => this._brushPage;
        BrushPage _brushPage { get; } = new BrushPage();

        public void Starting(Vector2 point)
        {
        }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            switch (this.BrushType)
            {
                case BrushType.None:
                case BrushType.Color:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 startPoint = Vector2.Transform(startingPoint, inverseMatrix);
                        Vector2 endPoint = Vector2.Transform(point, inverseMatrix);

                        this.SelectionViewModel.SetBrushToLinearGradient(startPoint, endPoint);//Initialize

                        this.LinearGradientTool.Type = LinearGradientType.EndPoint;//LinearGradientTool

                        return;
                    }
                case BrushType.LinearGradient:
                    this.LinearGradientTool.Started(startingPoint, point);//LinearGradientTool
                    break;
                case BrushType.RadialGradient:
                    this.RadialGradientTool.Started(startingPoint, point);//RadialGradientTool
                    break;
                case BrushType.EllipticalGradient:
                    this.EllipticalGradientTool.Started(startingPoint, point);//EllipticalGradientTool
                    break;
                case BrushType.Image:
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            switch (this.BrushType)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    this.LinearGradientTool.Delta(startingPoint, point);//LinearGradientTool
                    break;
                case BrushType.RadialGradient:
                    this.RadialGradientTool.Delta(startingPoint, point);//RadialGradientTool
                    break;
                case BrushType.EllipticalGradient:
                    this.EllipticalGradientTool.Delta(startingPoint, point);//EllipticalGradientTool
                    break;
                case BrushType.Image:
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            this.LinearGradientTool.Type = LinearGradientType.None;//LinearGradientTool
            this.RadialGradientTool.Type = RadialGradientType.None;//RadialGradientTool
            this.EllipticalGradientTool.Type = EllipticalGradientType.None;//EllipticalGradientTool

            if (isSingleStarted == false)
            {
                this.TipViewModel.TransformerTool.SelectLayer(startingPoint);//TransformerTool
            }
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            switch (this.BrushType)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    this.LinearGradientTool.Draw(drawingSession);//LinearGradientTool
                    break;
                case BrushType.RadialGradient:
                    this.RadialGradientTool.Draw(drawingSession);//RadialGradientTool
                    break;
                case BrushType.EllipticalGradient:
                    this.EllipticalGradientTool.Draw(drawingSession);//EllipticalGradientTool
                    break;
                case BrushType.Image:
                    break;
            }
        }
        
        public void OnNavigatedTo() => this.SelectionViewModel.SetBrushFormSingleMode(this.SelectionViewModel.FillOrStroke);
        public void OnNavigatedFrom() { }
    }
}