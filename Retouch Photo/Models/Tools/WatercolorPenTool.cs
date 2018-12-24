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
            base.Icon = new ToolWatercolorPenControl();
            base.WorkIcon = new ToolWatercolorPenControl();
            base.Page = new ToolWatercolorPenPage();
            base.ViewModel = new ToolWatercolorPenViewModel();
        }
    }
}
