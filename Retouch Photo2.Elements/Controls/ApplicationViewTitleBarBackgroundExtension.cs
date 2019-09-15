using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    public class ApplicationViewTitleBarBackgroundExtension: DependencyObject
    {
        private static readonly ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
        
        #region DependencyProperty
        
        /// <summary> Color of <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get { return (Color)GetValue(TitleBarColorProperty); }
            set { SetValue(TitleBarColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ApplicationViewTitleBarBackgroundExtension.TitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(ApplicationViewTitleBarBackgroundExtension), new PropertyMetadata(Colors.White, (sender, e) =>
        {
            ApplicationViewTitleBarBackgroundExtension con = (ApplicationViewTitleBarBackgroundExtension)sender;

            if (e.NewValue is Color value)
            {
                ApplicationViewTitleBarBackgroundExtension.TitleBar.BackgroundColor =
                ApplicationViewTitleBarBackgroundExtension.TitleBar.InactiveBackgroundColor =
                ApplicationViewTitleBarBackgroundExtension.TitleBar.ButtonBackgroundColor =
                ApplicationViewTitleBarBackgroundExtension.TitleBar.ButtonInactiveBackgroundColor = value;
            }
        }));
         
        #endregion
        
    }
}