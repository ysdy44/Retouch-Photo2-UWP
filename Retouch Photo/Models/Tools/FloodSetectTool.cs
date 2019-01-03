using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class FloodSetectTool : Tool
    {
        public FloodSetectTool()
        {
            base.Type = ToolType.FloodSetect;
            base.Icon = new FloodSetectControl();
            base.WorkIcon = new FloodSetectControl();
            base.Page = new FloodSetectPage();
            base.ViewModel = new FloodSetectViewModel();
        }
    }
}
