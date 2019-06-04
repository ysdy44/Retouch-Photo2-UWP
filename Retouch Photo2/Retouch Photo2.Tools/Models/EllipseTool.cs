using Retouch_Photo2.Models;
using Retouch_Photo2.Models.Layers.GeometryLayers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Models
{
    public class EllipseTool : LayerTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public EllipseTool()
        {
            base.Type = ToolType.Ellipse;
            base.Icon = new EllipseControl();
            base.WorkIcon = new EllipseControl();
            base.Page = new EllipsePage();
        }
        

        public override Layer GetLayer(VectRect rect) => EllipseLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
    }
}
