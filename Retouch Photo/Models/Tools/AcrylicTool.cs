using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class AcrylicTool : Tool
    {
        public AcrylicTool()
        {
            base.Type = ToolType.Acrylic;
            base.Icon = new ToolAcrylicControl();
            base.WorkIcon = new ToolAcrylicControl();
            base.Page = new ToolAcrylicPage();
            base.ViewModel = new ToolAcrylicViewModel();
        }
    }
}
