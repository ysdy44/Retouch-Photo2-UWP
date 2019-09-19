using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Buttons
{
    /// <summary>
    /// Button of <see cref = "RectangleTool"/>.
    /// </summary>
    public sealed partial class RectangleButton : UserControl, IToolButton
    {
        //@ViewModel
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content
        public bool IsSelected { set => this.Button.IsSelected = value; }
        public FrameworkElement Self => this;

        //@Construct
        public RectangleButton()
        {
            this.InitializeComponent();
        }
    }
}