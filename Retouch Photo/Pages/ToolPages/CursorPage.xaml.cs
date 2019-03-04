using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Pages.ToolPages
{
    public sealed partial class CursorPage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public CursorPage()
        {
            this.InitializeComponent();
            this.StepFrequencyToggleControl.CheckedChanged += (c) => this.ViewModel.Invalidate();
            this.SkewToggleControl.CheckedChanged += (c) => this.ViewModel.Invalidate();
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
