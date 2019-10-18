using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "KeyboardViewModel.IsSquare" /> and <see cref = "KeyboardViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreCreateControl : UserControl
    {
        //@ViewModel
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        
        //@Construct
        public MoreCreateControl()
        {
            this.InitializeComponent(); 
        }
    }
}