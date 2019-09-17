using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Buttons
{
    /// <summary>
    /// Button of <see cref = "EllipseTool"/>.
    /// </summary>
    public sealed partial class EllipseButton : UserControl, IToolButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@Content
        public bool IsSelected { set => this.Button.IsSelected = value; }
        public FrameworkElement Self => this;

        //@Construct
        public EllipseButton()
        {
            this.InitializeComponent();
        }
    }
}