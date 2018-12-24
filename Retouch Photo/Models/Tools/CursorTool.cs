using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class CursorTool : Tool
    {
        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new ToolCursorControl();
            base.WorkIcon = new ToolCursorControl();
            base.Page = new ToolCursorPage();
            base.ViewModel = new ToolCursorViewModel();
        }
    }
}
