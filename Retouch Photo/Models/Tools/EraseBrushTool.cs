using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class EraseBrushTool : Tool
    {
        public EraseBrushTool()
        {
            base.Type = ToolType.EraseBrush;
            base.Icon = new EraseBrushControl();
            base.WorkIcon = new EraseBrushControl();
            base.Page = new EraseBrushPage();
            base.ViewModel = new EraseBrushViewModel();
        }
    }
}
