using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    public class ApplicationViewTitleBarBackgroundExtension : DependencyObject
    {
        private static readonly ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
        public static System.Action<string> ssss;

        #region DependencyProperty

        /// <summary> Color of <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get { return (Color)GetValue(TitleBarColorProperty); }
            set
            {
                ssss?.Invoke(value.ToString()); //Delegate
                SetValue(TitleBarColorProperty, value); }
            }
        /// <summary> Identifies the <see cref = "ApplicationViewTitleBarBackgroundExtension.TitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(ApplicationViewTitleBarBackgroundExtension), new PropertyMetadata(Colors.Gray, (sender, e) =>
        {
            ApplicationViewTitleBarBackgroundExtension con = (ApplicationViewTitleBarBackgroundExtension)sender;

            if (e.NewValue is Color value)
            {
                Color color = value;
                ApplicationViewTitleBarBackgroundExtension.TitleBar.BackgroundColor = color;
                ApplicationViewTitleBarBackgroundExtension.TitleBar.InactiveBackgroundColor = color;
                ApplicationViewTitleBarBackgroundExtension.TitleBar.ButtonBackgroundColor = color;
                ApplicationViewTitleBarBackgroundExtension.TitleBar.ButtonInactiveBackgroundColor = color;
            }
        }));

        #endregion

                    
        //@Static
        /// <summary>
        /// Sets the application theme.
        /// </summary>
        /// <param name="value"> The destination theme. </param>
        public static void SetTheme(ElementTheme value)
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme != value)
                {
                    frameworkElement.RequestedTheme = value;
                }
            }
        }
    }
}