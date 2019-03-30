using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
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
