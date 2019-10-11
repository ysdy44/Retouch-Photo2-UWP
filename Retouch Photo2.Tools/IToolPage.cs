using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool page.
    /// </summary>
    public interface IToolPage
    {
        /// <summary> Sets IToolButton's IsSelected. </summary>
        bool IsSelected { set; }
        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }
        
        /// <summary> The current tool becomes the active tool. </summary>
        void OnNavigatedTo();
        /// <summary> The current page does not become an active page. </summary>
        void OnNavigatedFrom();
    }
}