// Core:              
// Referenced:   ★★
// Difficult:         ★
// Only:              
// Complete:      ★
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
                    this.Register();
                    MoreTransformButton.Flyout.ShowAt(placementTarget);
                }
                else
                {
                    this.Register();
                    MoreTransformButton.Flyout.ShowAt(this);
                }
            };
        }

        private void Register()
        {
            if (MoreTransformButton.Flyout == null) return;
            MoreTransformButton.Flyout.Opened += this.Flyout_Opened;
            MoreTransformButton.Flyout.Closed += this.Flyout_Closed;
        }
        private void UnRegister()
        {
            if (MoreTransformButton.Flyout == null) return;
            MoreTransformButton.Flyout.Opened -= this.Flyout_Opened;
            MoreTransformButton.Flyout.Closed -= this.Flyout_Closed;
        }

        private void Flyout_Opened(object sender, object e)
        {
            this.Button.IsEnabled = false;
        }
        private void Flyout_Closed(object sender, object e)
        {
            this.Button.IsEnabled = true;
            this.UnRegister();
        }
    }
}