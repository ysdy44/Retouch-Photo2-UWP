using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Brushs;
using Retouch_Photo.ViewModels;
using Windows.UI;

namespace Retouch_Photo.Models.Layers
{
    public abstract class GeometryLayer : Layer
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public Brush FillBrush = new Brush();

        public float StrokeWidth = 1.0f;
        public Brush StrokeBrush;
        public CanvasStrokeStyle StrokeStyle;

        public override void ColorChanged(Color color, bool fillOrStroke)
        {
       //     if (fillOrStroke)
       //     {
       //         if (this.FillBrush is CanvasSolidColorBrush brush) brush.Color = color;
        //        else this.FillBrush = new CanvasSolidColorBrush(this.ViewModel.CanvasDevice, color);
        //    }
       //     else
          //  {
          //      if (this.StrokeBrush is CanvasSolidColorBrush brush) brush.Color = color;
          //      else this.StrokeBrush = new CanvasSolidColorBrush(this.ViewModel.CanvasDevice, color);
         //   }
        }
        public override void BrushChanged(ICanvasBrush brush, bool fillOrStroke)
        {
        //    if (fillOrStroke)
         //   {
           //     this.FillBrush = brush;
          //  }
        //    else
        //    {
             //   this.StrokeBrush = brush;
           // }
        }
    }
}
