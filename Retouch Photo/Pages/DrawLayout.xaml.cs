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

            if (e.NewValue is Tool newTool)
            {
                con.WorkLeftBorder.Content = newTool.WorkIcon;
                con.BottomBarFrame.Content = newTool.Page;

                if (e.OldValue is Tool oldTool)
                {
                    if (newTool.Type != oldTool.Type)
                    {
                        if (con.WorkDismissOverlay.Visibility == Visibility.Visible) con.WorkOverlay();

                        //当前页面不再成为活动页面
                        oldTool.Page.ToolOnNavigatedFrom();

                        //当前页面成为活动页面
                        newTool.Page.ToolOnNavigatedTo();
                    }
                }
            }
        }));


        #endregion


        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }

        public UIElement CenterContent { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        public UIElement TopLeftPane { get => this.TopLeftBorder.Child; set => this.TopLeftBorder.Child = value; }
        public UIElement TopRightPane { get => this.TopRightBorder.Child; set => this.TopRightBorder.Child = value; }
        public UIElement TopLeftStackBar { get => this.TopLeftStackPanel.Child; set => this.TopLeftStackPanel.Child = value; }


        /// <summary> 左工作区选定 </summary>
        private bool WorkLeftChecked
        {
            set
            {
                this.WorkLeftRectangle.Fill = value ? this.AccentColor : this.UnAccentColor;
                this.WorkLeftBorder.Foreground = value ? this.CheckColor : this.UnCheckColor;
            }
        }
        /// <summary> 右工作区选定 </summary>
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
        }

        /// <summary> 左工作区出现 </summary>
        private void WorkLeft(PointerRoutedEventArgs e)
        {
            if (e != null)
                if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
                    return;

            this.LeftBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Visible;
            this.WorkLeftChecked = true;
        }
        /// <summary> 右工作区出现 </summary>
        private void WorkRight(PointerRoutedEventArgs e)
        {
            if (e != null)
                if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
                    return;

            this.RightBorder.Visibility = this.WorkDismissOverlay.Visibility = Visibility.Visible;
            this.WorkRightChecked = true;
        }

        /// <summary> 覆盖层消失 </summary>
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
