using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provides constant and static methods 
    /// for gets and sets to <see cref="ApplicationViewTitleBar"/> background.
    /// </summary>
    public class ApplicationViewTitleBarBackgroundExtension : DependencyObject
    {
        private static readonly ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;

        #region DependencyProperty

        /// <summary> Color of <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get => (Color)GetValue(TitleBarColorProperty);
            set => SetValue(TitleBarColorProperty, value);
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
    }
}