// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    internal class HistoryTextBlock : ContentControl
    {             
        public HistoryType Type
        {
            set => this.Content = this.StringConverter(value);
        }

        //@String
        private string StringConverter(HistoryType value)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString($"Historys_{value}");
        }
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Historys.IHistory"/>.
    /// </summary>
    public sealed partial class HistoryMenu : UserControl
    {

        //@Content
        /// <summary> ListView's ItemsSource. </summary>
        public object ItemsSource { get => this.ListView.ItemsSource; set => this.ListView.ItemsSource = value; }

        //@Construct
        /// <summary>
        /// Initializes a HistoryMainPage. 
        /// </summary>
        public HistoryMenu()
        {
            this.InitializeComponent();
        }
    }
}