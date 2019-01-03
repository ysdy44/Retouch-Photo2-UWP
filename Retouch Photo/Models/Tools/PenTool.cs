using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class PenTool : Tool
    {
        public PenTool()
        {
            base.Type = ToolType.Pen;
            base.Icon = new PenControl();
            base.WorkIcon = new PenControl();
            base.Page = new PenPage();
            base.ViewModel = new PenViewModel();
        }
    }
}
