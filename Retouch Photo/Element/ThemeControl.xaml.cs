using Retouch_Photo;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Element
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
                ThemeControl.TitleBarColor = value.Color;
            }
        }));

        #endregion

        /// <summary>
        /// Theme of current <see cref="Window"/>.
        /// </summary>
        public static ElementTheme Theme
        {
            get => ThemeControl.theme;
            set
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    frameworkElement.RequestedTheme = value;
                }

                ThemeControl.theme = value;
            }
        }
        private static ElementTheme theme;

        public ElementTheme _Theme
        {
            get => ThemeControl.theme;
            set
            {
                if (value == ElementTheme.Dark)                
                    this.DarkStoryboar.Begin();//Storyboard                
                else                
                    this.LightStoryboard.Begin();//Storyboard                

                ThemeControl.theme = value;
            }
        }
               
        private static ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
        public static Color TitleBarColor
        {
            set
            {
                ThemeControl.TitleBar.BackgroundColor =
                ThemeControl.TitleBar.InactiveBackgroundColor =
                ThemeControl.TitleBar.ButtonBackgroundColor =
                ThemeControl.TitleBar.ButtonInactiveBackgroundColor = value;
            }
        }

        public ThemeControl()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this._Theme = (App.Current.RequestedTheme == ApplicationTheme.Dark) ? ElementTheme.Dark : ElementTheme.Light;
        }
    }
}