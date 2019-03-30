using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class LineTool : Tool
    {
        public LineTool()
        {
            base.Type = ToolType.Line;
            base.Icon = new LineControl();
            base.WorkIcon = new LineControl();
            base.Page = new LinePage();
            base.ViewModel = new LineViewModel();
        }
    }
}
