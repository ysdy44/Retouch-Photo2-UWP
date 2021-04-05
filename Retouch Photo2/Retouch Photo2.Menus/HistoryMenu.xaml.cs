// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Historys;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus
{
    internal class HistoryTypeConverter : IValueConverter
    {
        //@String
        static readonly ResourceLoader resource = ResourceLoader.GetForCurrentView();

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value is HistoryType type)
            {
                return HistoryTypeConverter.resource.GetString($"Historys_{type}");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Historys.IHistory"/>.
    /// </summary>
    public sealed partial class HistoryMenu : UserControl
    {
        //@Construct
        /// <summary>
        /// Initializes a HistoryMainPage. 
        /// </summary>
        public HistoryMenu()
        {
            this.InitializeComponent();
            this.ListView.ItemsSource = HistoryBase.Instances;
        }
    }
}