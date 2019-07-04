using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.ILayer;
using Retouch_Photo2.Retouch_Photo2.Tools.Models.BrushTools;
using Retouch_Photo2.Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s BrushTool.
    /// </summary>
    public class BrushTool : Tool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //Brush
        LinearGradientTool LinearGradientTool = new LinearGradientTool();
        RadialGradientTool RadialGradientTool = new RadialGradientTool();
        EllipticalGradientTool EllipticalGradientTool = new EllipticalGradientTool();


        //@Construct
        public BrushTool()
        {
            base.Type = ToolType.Brush;
            base.Icon = new BrushControl();
            base.ShowIcon = new BrushControl();
            base.Page = new BrushPage();
        }


        //@Override        
        public override void ToolOnNavigatedTo()
        {
            //Brush
            this.SelectionViewModel.SetBrushFormSingleMode(this.SelectionViewModel.FillOrStroke);
        }

        public override void Starting(Vector2 point)
        {
        }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return;

            switch (this.SelectionViewModel.BrushType)
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
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return;

            switch (this.SelectionViewModel.BrushType)
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
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Selection
            if (this.SelectionViewModel.Mode == ListViewSelectionMode.None) return;

            this.LinearGradientTool.Type = LinearGradientType.None;//LinearGradientTool
            this.RadialGradientTool.Type = RadialGradientType.None;//RadialGradientTool
            this.EllipticalGradientTool.Type = EllipticalGradientType.None;//EllipticalGradientTool

            if (isSingleStarted == false)
            {
                this.TipViewModel.TransformerTool.SelectLayer(startingPoint);//TransformerTool
            }
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            switch (this.SelectionViewModel.BrushType)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    this.LinearGradientTool.Draw(ds);//LinearGradientTool
                    break;
                case BrushType.RadialGradient:
                    this.RadialGradientTool.Draw(ds);//RadialGradientTool
                    break;
                case BrushType.EllipticalGradient:
                    this.EllipticalGradientTool.Draw(ds);//EllipticalGradientTool
                    break;
                case BrushType.Image:
                    break;
            }
        }
    }
}