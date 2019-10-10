using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Buttons
{
    /// <summary> 
    /// Button of <see cref = "TransparencyTool"/>.
    /// </summary>
    public sealed partial class TransparencyButton : UserControl, IToolButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        
        //@Content
        public bool IsSelected { set => this.Button.IsSelected = value; }
        public FrameworkElement Self => this;
        public ToolButtonType Type => ToolButtonType.None;

        //@Construct
        public TransparencyButton()
        {
            this.InitializeComponent();
        }
    }
}