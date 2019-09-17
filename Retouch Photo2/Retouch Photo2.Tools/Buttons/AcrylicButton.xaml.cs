using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Buttons
{
    /// <summary> 
    /// Button of <see cref = "AcrylicTool"/>.
    /// </summary>
    public sealed partial class AcrylicButton : UserControl, IToolButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content
        public bool IsSelected { set => this.Button.IsSelected = value; }
        public FrameworkElement Self => this;

        //@Construct
        public AcrylicButton()
        {
            this.InitializeComponent();
        }
    }
}