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
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
       
        //@Static
        public static MoreTransformControl MoreTransformControl = new MoreTransformControl();
        Flyout Flyout => MoreTransformButton.MoreTransformControl.Flyout;

        //@Construct
        public MoreTransformButton()
        {
            this.InitializeComponent();
            this.Button.Tapped += (s, e) =>
            {
                this.TipViewModel.TouchbarType = TouchbarType.None;//Touchbar

                if (this.Parent is FrameworkElement placementTarget)
                {
                   this.Flyout.ShowAt(placementTarget);
                }
                else
                {
                    this.Flyout.ShowAt(this);
                }
            };
        }
    }
}