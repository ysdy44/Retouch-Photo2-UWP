using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements.SettingPages
{
    /// <summary>
    /// Grid control that display layouts for adaptive width.
    /// </summary>
    public sealed partial class AdaptiveWidthLayout : UserControl
    {

        PhoneLayout PhoneLayout = new PhoneLayout();
        PadLayout PadLayout = new PadLayout();
        PCLayout PCLayout = new PCLayout();

        #region DependencyProperty


        /// <summary> Type of <see cref = "AdaptiveWidthLayout" />. </summary>
        public DeviceLayoutType Type
        {
            get { return (DeviceLayoutType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthLayout.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(DeviceLayoutType), typeof(AdaptiveWidthLayout), new PropertyMetadata(DeviceLayoutType.PC, (sender, e) =>
        {
            AdaptiveWidthLayout con = (AdaptiveWidthLayout)sender;

            if (e.NewValue is DeviceLayoutType value)
            {
                switch (value)
                {
                    case DeviceLayoutType.Phone: con.Content = con.PhoneLayout; break;
                    case DeviceLayoutType.Pad: con.Content = con.PadLayout; break;
                    case DeviceLayoutType.PC: con.Content = con.PCLayout; break;
                }
            }
        }));

        #endregion

    }
}