using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class PaintBrushTool : Tool
    {
        public PaintBrushTool()
        {
            base.Type = ToolType.PaintBrush;
            base.Icon = new PaintBrushControl();
            base.WorkIcon = new PaintBrushControl();
            base.Page = new PaintBrushPage();
            base.ViewModel = new PaintBrushViewModel();
        }
    }
}
