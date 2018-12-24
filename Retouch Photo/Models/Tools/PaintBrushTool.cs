using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class PaintBrushTool : Tool
    {
        public PaintBrushTool()
        {
            base.Type = ToolType.PaintBrush;
            base.Icon = new ToolPaintBrushControl();
            base.WorkIcon = new ToolPaintBrushControl();
            base.Page = new ToolPaintBrushPage();
            base.ViewModel = new ToolPaintBrushViewModel();
        }
    }
}
