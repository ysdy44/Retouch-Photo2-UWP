using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "PenTool"/>.
    /// </summary>
    public sealed partial class PenPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Converter
        private Visibility FalseToVisibilityConverter(bool value) => value ? Visibility.Collapsed : Visibility.Visible;
        private Visibility TrueToVisibilityConverter(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
        
        //@Construct
        public PenPage()
        {
            this.InitializeComponent();
        }
    }
}