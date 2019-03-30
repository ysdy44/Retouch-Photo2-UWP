using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
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
