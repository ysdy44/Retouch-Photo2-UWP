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
        public UIElement EffectsPane { get => this.EffectsFlyout.Content; set => this.EffectsFlyout.Content = value; }
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
            this.SelectionToggleButton.Tapped += (sender, e) => FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
            this.SelectionFlyout.Opened += (sender, e) => this.SelectionToggleButton.IsChecked = true;
            this.SelectionFlyout.Closed += (sender, e) => this.SelectionToggleButton.IsChecked = false;
            //Effects
            this.EffectsToggleButton.Tapped += (sender, e) => FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
            this.EffectsFlyout.Opened += (sender, e) => this.EffectsToggleButton.IsChecked = true;
            this.EffectsFlyout.Closed += (sender, e) => this.EffectsToggleButton.IsChecked = false;
            //Others
            this.OthersToggleButton.Tapped += (sender, e) => FlyoutBase.ShowAttachedFlyout((ToggleButton)sender);
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

        // Appbar
        private void BottomBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AppbarRectangleFrameWidth.Value = e.NewSize.Width;
            this.AppbarRectangleStoryboard.Begin();
        }

    }
}
