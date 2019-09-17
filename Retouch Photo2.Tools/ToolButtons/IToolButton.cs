using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool button.
    /// </summary>
    public interface IToolButton
    {
        /// <summary> Sets IToolButton's IsSelected. </summary>
        bool IsSelected { set; }

        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }
    }
}