using Retouch_Photo2.Menus.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Icons
{
    /// <summary>
    /// Icon of <see cref="ColorMenu"/>.
    /// </summary>
    public sealed partial class ColorIcon : UserControl
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        
        //@Construct
        public ColorIcon()
        {
            this.InitializeComponent();
        }
    }
}