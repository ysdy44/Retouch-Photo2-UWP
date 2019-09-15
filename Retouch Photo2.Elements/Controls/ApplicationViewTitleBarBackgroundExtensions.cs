using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    public class ApplicationViewTitleBarBackgroundExtensions: DependencyObject
    {
        private static readonly ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
        
        #region DependencyProperty
        
        /// <summary> Color of <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get { return (Color)GetValue(TitleBarColorProperty); }
            set { SetValue(TitleBarColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ApplicationViewTitleBarBackgroundExtensions.TitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(ApplicationViewTitleBarBackgroundExtensions), new PropertyMetadata(Colors.White, (sender, e) =>
        {
            ApplicationViewTitleBarBackgroundExtensions con = (ApplicationViewTitleBarBackgroundExtensions)sender;

            if (e.NewValue is Color value)
            {
                ApplicationViewTitleBarBackgroundExtensions.TitleBar.BackgroundColor =
                ApplicationViewTitleBarBackgroundExtensions.TitleBar.InactiveBackgroundColor =
                ApplicationViewTitleBarBackgroundExtensions.TitleBar.ButtonBackgroundColor =
                ApplicationViewTitleBarBackgroundExtensions.TitleBar.ButtonInactiveBackgroundColor = value;
            }
        }));
         
        #endregion
        
    }
}