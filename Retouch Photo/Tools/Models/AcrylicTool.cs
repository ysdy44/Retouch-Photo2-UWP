using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class AcrylicTool : Tool
    {
        public AcrylicTool()
        {
            base.Type = ToolType.Acrylic;
            base.Icon = new AcrylicControl();
            base.WorkIcon = new AcrylicControl();
            base.Page = new AcrylicPage();
            base.ViewModel = new AcrylicViewModel();
        }
    }
}
