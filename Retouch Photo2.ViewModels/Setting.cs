using Retouch_Photo2.Elements;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Retouch_Photo2 's Setting
    /// </summary>
    public class Setting
    {
        /// <summary> <see cref = "Setting" />'s theme. </summary>
        public ElementTheme Theme = ElementTheme.Default;

        /// <summary> <see cref = "Setting" />'s device layout. </summary>
        public DeviceLayout DeviceLayout = DeviceLayout.Default;
    }
}