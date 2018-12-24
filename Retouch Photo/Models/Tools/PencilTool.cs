using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class PencilTool : Tool
    {
        public PencilTool()
        {
            base.Type = ToolType.Pencil;
            base.Icon = new ToolPencilControl();
            base.WorkIcon = new ToolPencilControl();
            base.Page = new ToolPencilPage();
            base.ViewModel = new ToolPencilViewModel();
        }
    }
}
