using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "SettingViewModel.IsSquare" /> and <see cref = "SettingViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreTransformButton : UserControl
    {
        //@Static
        /// <summary> Flyout </summary>
        public static Flyout Flyout;

        //@Construct
        /// <summary>
        /// Initializes a MoreTransformButton. 
        /// </summary>
        public MoreTransformButton()
        {
            this.InitializeComponent();

            this.Button.Click += (s, e) =>
            {
                if (MoreTransformButton.Flyout == null) return;

                if (this.Parent is FrameworkElement placementTarget)
                {
                    MoreTransformButton.Flyout.ShowAt(placementTarget);
                }
                else
                {
                    MoreTransformButton.Flyout.ShowAt(this);
                }
            };
        }
    }
}