using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class RectanglePage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public RectanglePage()
        {
            this.InitializeComponent();

            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.ViewModel.Color;
            };
            this.ColorPicker.ColorChange += (s, value) => 
            {
                this.ViewModel.Color = value;

                Layer layer = this.ViewModel.CurrentLayer;
                if (layer != null)
                {
                    layer.ColorChanged(value);
                    this.ViewModel.Invalidate();
                }
            };
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
            this.ColorPicker.Color = this.ViewModel.Color;
        }
    }
}
