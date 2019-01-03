using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class SelectionBrushTool : Tool
    {
        public SelectionBrushTool()
        {
            base.Type = ToolType.SelectionBrush;
            base.Icon = new SelectionBrushControl();
            base.WorkIcon = new SelectionBrushControl();
            base.Page = new SelectionBrushPage();
            base.ViewModel = new SelectionBrushViewModel();
        }
    }
}
