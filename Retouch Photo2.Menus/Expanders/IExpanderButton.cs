using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Button of <see cref="Expander"/>.
    /// </summary>
    public interface IExpanderButton
    {
        /// <summary> Gets or sets the title. </summary>
        string Title { get; set; }
        /// <summary> Sets the content. </summary>
        object Content { set; }
        /// <summary> Sets the IsOpen. </summary>
        bool IsOpen { set; }
        
        /// <summary> Sets the state. </summary>
        ExpanderState ExpanderState { set; }
        /// <summary> Get the self. </summary>
        FrameworkElement Self { get; }
    }
}