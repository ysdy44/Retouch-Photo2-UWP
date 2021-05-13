// Core:              
// Referenced:   ★
// Difficult:         
// Only:              
// Complete:      
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    public sealed partial class ColorIcon : UserControl
    {
        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        /// <summary>
        /// Initializes a ColorIcon. 
        /// </summary>
        public ColorIcon()
        {
            this.InitializeComponent();
        }
    }
}