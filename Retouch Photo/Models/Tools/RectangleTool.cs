using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class RectangleTool : Tool
    {
        public RectangleTool()
        {
            base.Type = ToolType.Rectangle;
            base.Icon = new RectangleControl();
            base.WorkIcon = new RectangleControl();
            base.Page = new RectanglePage();
            base.ViewModel = new RectangleViewModel();
        }
    }
}
