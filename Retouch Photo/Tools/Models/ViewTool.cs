using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class ViewTool : Tool
    {
        public ViewTool()
        {
            base.Type = ToolType.View;
            base.Icon = new ViewControl();
            base.WorkIcon = new ViewControl();
            base.Page = new ViewPage();
            base.ViewModel = new ViewViewModel();
        }
    }
}
