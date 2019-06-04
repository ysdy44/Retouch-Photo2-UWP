using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class CursorPage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public CursorPage()
        {
            this.InitializeComponent();
            this.MoreButton.Tapped += (s,e) => this.MoreFlyout.ShowAt(this.MoreButton);
        }   
    }
}
