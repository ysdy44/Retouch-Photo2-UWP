using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
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
