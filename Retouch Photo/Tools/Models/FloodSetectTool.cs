using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
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
