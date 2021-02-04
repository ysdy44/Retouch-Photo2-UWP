// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Historys.IHistory"/>.
    /// </summary>
    public sealed partial class HistoryMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;


        //@Construct
        /// <summary>
        /// Initializes a HistoryMainPage. 
        /// </summary>
        public HistoryMainPage()
        {
            this.InitializeComponent();

            this.ListView.ItemsSource = this.ViewModel.Historys;
        }
    }
}
