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

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }        
    }
}
