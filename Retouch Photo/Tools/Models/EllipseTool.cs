using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class EllipseTool : IRectangleTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        
        public EllipseTool()
        {
            base.Type = ToolType.Ellipse;
            base.Icon = new EllipseControl();
            base.WorkIcon = new EllipseControl();
            base.Page = new EllipsePage();
        }

        
        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }

        public override Layer GetLayer(VectRect rect) => EllipseLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
    }
}
