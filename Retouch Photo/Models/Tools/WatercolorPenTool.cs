using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class WatercolorPenTool : Tool
    {
        public WatercolorPenTool()
        {
            base.Type = ToolType.WatercolorPen;
            base.Icon = new WatercolorPenControl();
            base.WorkIcon = new WatercolorPenControl();
            base.Page = new WatercolorPenPage();
            base.ViewModel = new WatercolorPenViewModel();
        }
    }
}
