using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class PencilTool : Tool
    {
        public PencilTool()
        {
            base.Type = ToolType.Pencil;
            base.Icon = new PencilControl();
            base.WorkIcon = new PencilControl();
            base.Page = new PencilPage();
            base.ViewModel = new PencilViewModel();
        }
    }
}
