using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Button of <see cref = "KeyboardViewModel.IsSquare" /> and <see cref = "KeyboardViewModel.IsCenter" />.
    /// </summary>
    public sealed partial class MoreCreateButton : UserControl
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
       
        //@Static
        public static MoreCreateControl MoreCreateControl = new MoreCreateControl();
        Flyout Flyout => MoreCreateButton.MoreCreateControl.Flyout;

        //@Construct
        public MoreCreateButton()
        {
            this.InitializeComponent();
            this.Button.Tapped += (s, e) =>
            {
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