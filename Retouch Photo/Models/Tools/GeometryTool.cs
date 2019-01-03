using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    public class GeometryTool : Tool
    {
        public GeometryTool()
        {
            base.Type = ToolType.Geometry;
            base.Icon = new GeometryControl();
            base.WorkIcon = new GeometryControl();
            base.Page = new GeometryPage();
            base.ViewModel = new GeometryViewModel();
        }
    }
}
