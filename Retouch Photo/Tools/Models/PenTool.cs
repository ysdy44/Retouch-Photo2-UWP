using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class PenTool : Tool
    {
        public PenTool()
        {
            base.Type = ToolType.Pen;
            base.Icon = new PenControl();
            base.WorkIcon = new PenControl();
            base.Page = new PenPage();
            base.ViewModel = new PenViewModel();
        }
    }
}
