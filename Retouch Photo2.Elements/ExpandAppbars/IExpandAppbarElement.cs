using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    public interface IExpandAppbarElement
    {
        /// <summary> Gets element width. </summary>
        double ExpandWidth { get; }

        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }
    }
}