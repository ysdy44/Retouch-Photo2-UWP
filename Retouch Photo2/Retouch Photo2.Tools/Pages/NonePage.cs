using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "NoneTool"/>.
    /// </summary>
    public class NonePage : IToolPage
    {
        public bool IsSelected { private get; set; }

        public FrameworkElement Self => null;

        public void OnNavigatedFrom() { }
        public void OnNavigatedTo() { }
    }
}