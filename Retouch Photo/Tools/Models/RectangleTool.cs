using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class RectangleTool : IRectangleTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public RectangleTool()
        {
            base.Type = ToolType.Rectangle;
            base.Icon = new RectangleControl();
            base.WorkIcon = new RectangleControl();
            base.Page = new RectanglePage();
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }

        public override Layer GetLayer(VectRect rect)=> RectangularLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
    }
}
