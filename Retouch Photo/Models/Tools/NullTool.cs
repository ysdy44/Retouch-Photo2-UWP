using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Pages.ToolPages;
using Retouch_Photo.ViewModels.ToolViewModels;

namespace Retouch_Photo.Models.Tools
{
    class NullTool:Tool
    {
        public NullTool()
        {
           // base.Type = null;
            base.Icon = null;
            base.WorkIcon = null;
            base.Page = null;
            base.ViewModel = new NullViewModel();
        }
    }
}
