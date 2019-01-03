using Retouch_Photo.Controls.ToolControls;
using Retouch_Photo.Models;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Pages
{
    public sealed partial class DrawLayout : UserControl
    {

        //@Delegate
        public event TappedEventHandler SelectionButtonTapped;
        public event TappedEventHandler ArrangeButtonTapped;
        public event TappedEventHandler OthersButtonTapped;


        #region DependencyProperty


        public Tool Tool
        {
            get { return (Tool)GetValue(ToolProperty); }
            set { SetValue(ToolProperty, value); }
        }
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(Tool), typeof(DrawLayout), new PropertyMetadata(null, (sender, e) =>
        {
            DrawLayout con = (DrawLayout)sender;

            if (e.NewValue is Tool tool)
            {
                con.WorkLeftBorder.Content = tool.WorkIcon;
                con.BottomBarFrame.Content = tool.Page;

                if (con.WorkDismissOverlay.Visibility == Visibility.Visible)
                    if (e.OldValue is Tool oldIndex)
                        if (tool.Type != oldIndex.Type)
                            con.WorkOverlay();
            }
        }));


        #endregion


        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }

        public UIElement CenterContent { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        public UIElement TopLeftPane { get => this.TopLeftBorder.Child; set => this.TopLeftBorder.Child = value; }
        public UIElement TopRightPane { get => this.TopRightBorder.Child; set => this.TopRightBorder.Child = value; }
        public UIElement TopLeftStackBar { get => this.TopLeftStackPanel.Child; set => this.TopLeftStackPanel.Child = value; }

        public UIElement SelectionPane { get => this.SelectionFlyout.Content; set => this.SelectionFlyout.Content = value; }
        public UIElement EffectsPane { get => this.ArrangeFlyout.Content; set => this.ArrangeFlyout.Content = value; }
        public UIElement OthersPane { get => this.OthersFlyout.Content; set => this.OthersFlyout.Content = value; }


        private bool WorkLeftChecked
        {
            set
            {
                this.WorkLeftRectangle.Fill = value ? this.AccentColor : this.UnAccentColor;
                this.WorkLeftBorder.Foreground = value ? this.CheckColor : this.UnCheckColor;
            }
        }

        private bool WorkRightChecked
        {
            set
            {
                this.WorkRightRectangle.Fill = value ? this.AccentColor : this.UnAccentColor;
                this.WorkRightBorder.Foreground = value ? this.CheckColor : this.UnCheckColor;
            }
        }


        public DrawLayout()
        {
            this.InitializeComponent();

            //WorkLeft
            this.WorkLeftGrid.Tapped += (sender, e) => this.WorkLeft(null);
            this.WorkLeftGrid.PointerEntered += (sender, e) => this.WorkLeft(e);
            //WorkRight
            this.WorkRightGrid.Tapped += (sender, e) => this.WorkRight(null);
            this.WorkRightGrid.PointerEntered += (sender, e) => this.WorkRight(e);
            //DismissOverlay
            this.WorkDismissOverlay.Tapped += (sender, e) => this.WorkOverlay();

            //Selection
            this.SelectionToggleButton.Tapped += (sender, e) => this.ButtonTapped(this.SelectionButtonTapped, sender, e);
            this.SelectionFlyout.Opened += (sender, e) => this.SelectionToggleButton.IsChecked = true;
            this.SelectionFlyout.Closed += (sender, e) => this.SelectionToggleButton.IsChecked = false;
            //Arrange
            this.ArrangeToggleButton.Tapped += (sender, e) => this.ButtonTapped(this.ArrangeButtonTapped, sender, e);
            this.ArrangeFlyout.Opened += (sender, e) => this.ArrangeToggleButton.IsChecked = true;
            this.ArrangeFlyout.Closed += (sender, e) => this.ArrangeToggleButton.IsChecked = false;
            //Others
            this.OthersToggleButton.Tapped += (sender, e) => this.ButtonTapped(this.OthersButtonTapped, sender, e);
            this.OthersFlyout.Opened += (sender, e) => this.OthersToggleButton.IsChecked = true;
            this.OthersFlyout.Closed += (sender, e) => this.OthersToggleButton.IsChecked = false;
        }

        private void WorkLeft(PointerRoutedEventArgs e)
        {
            if (e != null)
                if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
                    return;

            this.LeftBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Visible;
            this.WorkLeftChecked = true;
        }
        private void WorkRight(PointerRoutedEventArgs e)
        {
            if (e != null)
                if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
                    return;

            this.RightBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Visible;
            this.WorkRightChecked = true;
        }

        private void WorkOverlay()
        {
            this.LeftBorder.Visibility = this.RightBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Collapsed;
            this.WorkLeftChecked = this.WorkRightChecked = false;
        }

        //S & A & O
        private void ButtonTapped(TappedEventHandler buttonTapped, object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
            buttonTapped?.Invoke(sender, e);
        }


        // Appbar
        private void BottomBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AppbarRectangleFrameWidth.Value = e.NewSize.Width;
            this.AppbarRectangleStoryboard.Begin();
        }

    }
}
