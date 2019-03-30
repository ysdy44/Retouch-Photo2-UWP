using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class RectangleTool : Tool
    {
        public RectangleTool()
        {
            base.Type = ToolType.Rectangle;
            base.Icon = new RectangleControl();
            base.WorkIcon = new RectangleControl();
            base.Page = new RectanglePage();
            base.ViewModel = new RectangleViewModel();
        }
    }
}
