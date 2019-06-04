using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class LinePage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        
        public LinePage()
        {
            this.InitializeComponent();
        }
    }
}
