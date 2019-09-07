using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a touchbar that can be operated.
    /// </summary>
    public interface ITouchbar
    {
        /// <summary> Gets or Sets ITouchbar's type. </summary>
        TouchbarType Type { get; }

        /// <summary> Gets or Sets ITouchbar's control. </summary>
        UserControl Self { get; }
    }
}