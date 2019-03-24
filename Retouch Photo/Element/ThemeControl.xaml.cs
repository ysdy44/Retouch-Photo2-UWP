using Retouch_Photo;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace 配色pro.Controls
{
    /// <summary> Click the button to toggle sensitive themes for your application. </summary>
    public sealed partial class ThemeControl : UserControl
    {
        #region DependencyProperty

        /// <summary>
        /// Brush of <see cref="ApplicationViewTitleBar"/>.
        /// </summary>
        public SolidColorBrush Brush
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Brush), typeof(SolidColorBrush), typeof(ThemeControl), new PropertyMetadata(new SolidColorBrush(Colors.White), (sender, e) =>
        {
            ThemeControl con = (ThemeControl)sender;

            if (e.NewValue is SolidColorBrush value)
            {
                con.TitleBar.BackgroundColor =
                con.TitleBar.InactiveBackgroundColor =
                con.TitleBar.ButtonBackgroundColor =
                con.TitleBar.ButtonInactiveBackgroundColor = value.Color;
            }
        }));

        #endregion

        /// <summary>
        /// Theme of current <see cref="Window"/>.
        /// </summary>
        public ElementTheme Theme
        {
            get => this.theme;
            set
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = value;
                }

                if (value == ElementTheme.Dark)
                {
                    this.DarkStoryboar.Begin();//Storyboard
                }
                else
                {
                    this.LightStoryboard.Begin();//Storyboard
                }

                this.theme = value;
            }
        }
        private ElementTheme theme;

        ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

        public ThemeControl()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.Theme = (App.Current.RequestedTheme == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
        }
    }
}