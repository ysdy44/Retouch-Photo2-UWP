using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class ViewTool : Tool
    {
        public ViewTool()
        {
            base.Type = ToolType.View;
            base.Icon = new ToolViewControl();
            base.WorkIcon = new ToolViewControl();
            base.Page = new ToolViewPage();
            base.ViewModel = new ToolViewViewModel();
        }
    }
}
