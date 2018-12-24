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
            base.Icon = new ToolFloodSetectControl();
            base.WorkIcon = new ToolFloodSetectControl();
            base.Page = new ToolFloodSetectPage();
            base.ViewModel = new ToolFloodSetectViewModel();
        }
    }
}
