using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class AcrylicTool : IRectangleTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        
        public AcrylicTool()
        {
            base.Type = ToolType.Acrylic;
            base.Icon = new AcrylicControl();
            base.WorkIcon = new AcrylicControl();
            base.Page = new AcrylicPage();
        }
        
        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }

        public override Layer GetLayer(VectRect rect) => AcrylicLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
    }
}
