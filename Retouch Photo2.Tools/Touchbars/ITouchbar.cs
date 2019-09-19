using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a touchbar that can be operated.
    /// </summary>
    public interface ITouchbar
    {
        /// <summary> Gets ITouchbar's type. </summary>
        TouchbarType Type { get; }

        /// <summary> Gets it yourself. </summary>
        UserControl Self { get; }
    }
}