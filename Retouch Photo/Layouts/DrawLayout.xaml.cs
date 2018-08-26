using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Layouts
{
    public sealed partial class DrawLayout : UserControl
    {


        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }

        public UIElement CenterContent { get => this.CenterBorder.Child;set => this.CenterBorder.Child = value;}
        public UIElement BottomBar { get => this.BottomBorder.Child; set => this.BottomBorder.Child = value; }


        public UIElement TopLeftPane { get => this.TopLeftBorder.Child; set => this.TopLeftBorder.Child = value; }
        public UIElement TopRightPane { get => this.TopRightBorder.Child; set => this.TopRightBorder.Child = value; }
        public UIElement TopLeftStackBar { get => this.TopLeftStackPanel.Child; set => this.TopLeftStackPanel.Child = value; }

        public UIElement SelectionPane { get => this.SelectionFlyout.Content; set => this.SelectionFlyout.Content = value; }
        public UIElement EffectsPane { get => this.EffectsFlyout.Content; set => this.EffectsFlyout.Content = value; }
        public UIElement OthersPane { get => this.OthersFlyout.Content; set => this.OthersFlyout.Content = value; }



        public DrawLayout()
        {
            this.InitializeComponent();


            this.WorkLeftBorder.Tapped +=(sender,e)=> Left();
            this.WorkLeftBorder.PointerEntered += (sender, e) => Left();

            this.WorkRightBorder.Tapped += (sender, e) => Right();
            this.WorkRightBorder.PointerEntered += (sender, e) => Right();

            this.WorkDismissOverlay.Tapped += (sender, e) =>
            {
                this.LeftBorder.Visibility = this.RightBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Collapsed;
                this.WorkLeftBorder.IsChecked = this.WorkRightBorder.IsChecked = false;
            };


            this.SelectionToggleButton.Tapped+= (sender, e) => FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
            this.SelectionFlyout.Opened+= (sender, e) => this.SelectionToggleButton.IsChecked = true;
            this.SelectionFlyout.Closed+= (sender, e) => this.SelectionToggleButton.IsChecked = false;

            this.EffectsToggleButton.Tapped+= (sender, e) => FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
            this.EffectsFlyout.Opened+= (sender, e) => this.EffectsToggleButton.IsChecked = true;
            this.EffectsFlyout.Closed+= (sender, e) => this.EffectsToggleButton.IsChecked = false;

            this.OthersToggleButton.Tapped+= (sender, e) => FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
            this.OthersFlyout.Opened+= (sender, e) => this.OthersToggleButton.IsChecked = true;
            this.OthersFlyout.Closed+= (sender, e) => this.OthersToggleButton.IsChecked = false;
        }

        private void Left()
        {
            this.LeftBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Visible;
           this. WorkLeftBorder.IsChecked = true;
        }
        private void Right()
        {
            this.RightBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Visible;
            this.WorkRightBorder.IsChecked = true;
        }

    }
}
