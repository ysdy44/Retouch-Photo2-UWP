using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "KeyboardViewModel.IsSquare" /> and <see cref = "KeyboardViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreTransformButton : UserControl
    {
        //@Static
        public static Flyout Flyout;

        //@Construct
        public MoreTransformButton()
        {
            this.InitializeComponent();

            this.Button.Tapped += (s, e) =>
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