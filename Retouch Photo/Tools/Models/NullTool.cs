using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.Tools.ViewModels;

namespace Retouch_Photo.Tools.Models
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
