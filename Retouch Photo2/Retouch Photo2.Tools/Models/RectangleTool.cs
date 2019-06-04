using Retouch_Photo2.Models;
using Retouch_Photo2.Models.Layers.GeometryLayers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Models
{
    public class RectangleTool : LayerTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public RectangleTool()
        {
            base.Type = ToolType.Rectangle;
            base.Icon = new RectangleControl();
            base.WorkIcon = new RectangleControl();
            base.Page = new RectanglePage();
        }


        public override Layer GetLayer(VectRect rect) => RectangularLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color, this.ViewModel.StrokeColor, this.ViewModel.StrokeWidth);
    }
}
