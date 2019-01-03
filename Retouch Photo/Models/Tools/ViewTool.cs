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
            base.Icon = new ViewControl();
            base.WorkIcon = new ViewControl();
            base.Page = new ViewPage();
            base.ViewModel = new ViewViewModel();
        }
    }
}
