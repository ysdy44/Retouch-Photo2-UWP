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
            base.Icon = new ToolGeometryControl();
            base.WorkIcon = new ToolGeometryControl();
            base.Page = new ToolGeometryPage();
            base.ViewModel = new ToolGeometryViewModel();
        }
    }
}
