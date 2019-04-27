using Retouch_Photo2.Models;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Models
{
    public class AcrylicTool : LayerTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        
        public AcrylicTool()
        {
            base.Type = ToolType.Acrylic;
            base.Icon = new AcrylicControl();
            base.WorkIcon = new AcrylicControl();
            base.Page = new AcrylicPage();
        }
                
        public override Layer GetLayer(VectRect rect)=> AcrylicLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
    }
}
