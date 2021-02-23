// Core:              ★★★★★
// Referenced:   ★★
// Difficult:         
// Only:              
// Complete:      ★
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provides constant and static methods 
    /// for gets and sets to <see cref="ApplicationViewTitleBar"/> background.
    /// </summary>
    public class ApplicationTitleBarExtension : FrameworkElement
    {
        
        private static readonly ApplicationView View = ApplicationView.GetForCurrentView();

        #region DependencyProperty

        /// <summary> Color of <see cref="ApplicationViewTitleBar"/>. </summary>
        public Color TitleBarColor
        {
            get => (Color)base.GetValue(TitleBarColorProperty);
            set => SetValue(TitleBarColorProperty, value);
        }
        /// <summary> Identifies the <see cref = "ApplicationTitleBarExtension.TitleBarColor" /> dependency property. </summary>
        public static readonly DependencyProperty TitleBarColorProperty = DependencyProperty.Register(nameof(TitleBarColor), typeof(Color), typeof(ApplicationTitleBarExtension), new PropertyMetadata(Colors.Gray, (sender, e) =>
        {
            ApplicationTitleBarExtension control = (ApplicationTitleBarExtension)sender;

            if (e.NewValue is Color value)
            {
                control.Color = value;
            }
        }));

        #endregion


        /// <summary> Gets or sets <see cref="ApplicationTitleBarExtension"/>'s color. </summary>
        public Color Color
        {
            get => this.color;
            set
            {
                View.TitleBar.BackgroundColor = value;
                View.TitleBar.InactiveBackgroundColor = value;
                View.TitleBar.ButtonBackgroundColor = value;
                View.TitleBar.ButtonInactiveBackgroundColor = value;

                this.color = value;
            }
        }
        private Color color = Colors.Gray;

    }
}