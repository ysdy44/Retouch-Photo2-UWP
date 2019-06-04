using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class EllipsePage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public EllipsePage()
        {
            this.InitializeComponent();
        }
    }
}
