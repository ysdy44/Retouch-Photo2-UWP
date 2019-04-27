using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class AcrylicPage : ToolPage
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public AcrylicPage()
        {
            this.InitializeComponent();
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
            this.ColorPicker.Color = this.ViewModel.Color;
        }

        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ColorFlyout.ShowAt(this.ColorButton);
            this.ColorPicker.Color = this.ViewModel.Color;
        }
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.ViewModel.Color = value;

            Layer layer = this.ViewModel.CurrentLayer;
            if (layer != null)
            {
                layer.ColorChanged(value);
                this.ViewModel.Invalidate();
            }
        }

    }
}
