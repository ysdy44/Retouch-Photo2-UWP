using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
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
