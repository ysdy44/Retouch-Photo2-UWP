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
            get  => (DeviceLayoutType)base.GetValue(TypeProperty);
            set => base.SetValue(TypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthLayout.Type" /> dependency property. </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(DeviceLayoutType), typeof(AdaptiveWidthLayout), new PropertyMetadata(DeviceLayoutType.PC, (sender, e) =>
        {
            AdaptiveWidthLayout control = (AdaptiveWidthLayout)sender;

            if (e.NewValue is DeviceLayoutType value)
            {
                switch (value)
                {
                    case DeviceLayoutType.Phone: control.Content = control.PhoneLayout; break;
                    case DeviceLayoutType.Pad: control.Content = control.PadLayout; break;
                    case DeviceLayoutType.PC: control.Content = control.PCLayout; break;
                }
            }
        }));

        #endregion

    }
}